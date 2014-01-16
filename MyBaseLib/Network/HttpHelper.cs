using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Threading;
//using System.Net.Browser;

namespace MyBaseLib.Network
{
    public class HttpHelper
    {
        private const int _UPLOAD_BUFFER_SIZE = 1048576;
        private const double _PROGRESS_MAX = 100.0;
        private static readonly TimeSpan _PROGRESS_INTERVAL = TimeSpan.FromMilliseconds(100.0);

        private static bool _isInitialized = false;
        private static double _LastSentBinaryTotalSize = 0.0;
        private static double _progressHop = 10.0;

        private bool _isCanceled;
        private bool _isFinished = true;
        private double _progressCurrent;
        private string _multiPartBoundary = string.Concat("httphelper--multipartboundary--", DateTime.Now.Ticks.ToString());

        private HttpWebRequest _request;
        private HttpWebResponse _response;
        private DispatcherTimer _progressTimer;
        private readonly SynchronizationContext _currentContext = SynchronizationContext.Current;

        public event EventHandler<HttpHelperEventArgs> ResponseFailed;
        public event EventHandler<HttpHelperEventArgs> ResponseSucceeded;
        public event EventHandler<HttpHelperUploadProgressEventArgs> RequestProgressChanged;

        private string _QueryString
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                if (this.RequestBodyStrings.Count > 0)
                {
                    foreach (KeyValuePair<string, string> pair in this.RequestBodyStrings)
                    {
                        builder.Append(pair.Key);
                        builder.Append("=");
                        builder.Append(pair.Value);
                        builder.Append("&");
                    }
                }
                if (this.RequestBodyJsonObjects.Count > 0)
                {
                    foreach (KeyValuePair<string, object> pair in this.RequestBodyJsonObjects)
                    {
                        builder.Append(pair.Key);
                        builder.Append("=");
                        builder.Append(Uri.EscapeUriString(JsonConvert.SerializeObject(pair.Value)));
                        builder.Append("&");
                    }
                }
                if (builder.Length > 0)
                {
                    builder = builder.Remove(builder.Length - 1, 1);
                }
                return builder.ToString();
                /*
                    string str = pair.Value.Replace("&", "%26");
                    writer.Write("{0}={1}", pair.Key, str);
                    if (num < (this.RequestBodyStrings.Count - 1))
                    {
                        writer.Write("&");
                    }
                */
            }
        }

        public string Method { get; set; }

        public Uri RequestUri { get; set; }

        public Dictionary<string, string> RequestHeaders { get; set; }

        public Dictionary<string, string> RequestBodyStrings { get; set; }

        public Dictionary<string, object> RequestBodyJsonObjects { get; set; }

        public Dictionary<string, HttpHelperMultiPartData> RequestBodyBinarys { get; set; }

        public Dictionary<string, FileInfo> RequestBodyFileInfos { get; set; }

        public CookieContainer RequestCookies { get; private set; }

        private Encoding _GetCharacterSet(string charSet)
        {
            Encoding encoding = Encoding.UTF8;
            if (!string.IsNullOrEmpty(charSet))
            {
                charSet = charSet.ToUpper();
                if (charSet == "UTF-8")
                {
                    encoding = Encoding.UTF8;
                }
                else if (charSet == "KSC5601" || charSet == "KS_C_5601-1987")
                {
                    encoding = Encoding.GetEncoding("ks_c_5601-1987");
                }
                else if (charSet == "MS949" || charSet == "CP949")
                {
                    encoding = Encoding.GetEncoding("x-cp20949");
                }
                else if (charSet == "EUC-KR")
                {
                    encoding = Encoding.GetEncoding("euc-kr");
                }
                else if (charSet == "ISO-8859-1")
                {
                    encoding = Encoding.GetEncoding("iso-8859-1");
                }
                else
                {
                    try
                    {
                        encoding = Encoding.GetEncoding(charSet);
                    }
                    catch
                    {
                        encoding = Encoding.UTF8;
                    }
                }
            }
            return encoding;
        }

        private void _StartMagicProgress()
        {
            if (this._currentContext != null)
            {
                SendOrPostCallback d = delegate(object state)
                {
                    double totalDataSize = 1.0;
                    if (this.RequestBodyBinarys != null)
                    {
                        foreach (KeyValuePair<string, HttpHelperMultiPartData> pair in this.RequestBodyBinarys)
                        {
                            totalDataSize += pair.Value.Data.Length;
                        }
                    }
                    if (this.RequestBodyFileInfos != null)
                    {
                        foreach (KeyValuePair<string, FileInfo> pair2 in this.RequestBodyFileInfos)
                        {
                            totalDataSize += pair2.Value.Length;
                        }
                    }

                    //작은거하나 졸라큰거하나 넣으면 작은거 하나가 거의 마무리될때 문제생길듯?
                    //큰거가 안찰듯
                    if (HttpHelper._LastSentBinaryTotalSize > 0.0)
                    {
                        HttpHelper._progressHop *= HttpHelper._LastSentBinaryTotalSize / totalDataSize;  //이전것과의 크기비율만큼 progress속도 지정
                        if (HttpHelper._progressHop < 1.0)
                        {
                            HttpHelper._progressHop = 1.0;
                        }
                        else if (HttpHelper._progressHop > 10.0)
                        {
                            HttpHelper._progressHop = 10.0;
                        }
                    }

                    //용량작으면 그냥 빠르게 처리하겠다는생각.
                    if (totalDataSize > 2.0)
                    {
                        HttpHelper._LastSentBinaryTotalSize = totalDataSize;
                    }

                    //Tick 중복추가 방지
                    this._progressTimer = new DispatcherTimer(); // 클래스 생성때와 같은 쓰레드서 객체 생성?
                    this._progressTimer.Interval = _PROGRESS_INTERVAL;
                    this._progressTimer.Tick += delegate(object s, EventArgs e)
                    {
                        this._progressTimer.Stop();
                        if (!this._isCanceled && !this._isFinished)
                        {
                            double num = 100.0 - this._progressCurrent;
                            if (num <= (HttpHelper._progressHop * 5.0))
                            {
                                HttpHelper._progressHop /= 2.0;
                            }
                            this._progressCurrent += HttpHelper._progressHop;
                            this._progressTimer.Start();
                            if (this.RequestProgressChanged != null)
                            {
                                HttpHelperUploadProgressEventArgs args = new HttpHelperUploadProgressEventArgs("", 100, (int)this._progressCurrent);
                                this.RequestProgressChanged(this, args);
                            }
                        }
                    };
                    this._progressTimer.Start();
                };
                this._currentContext.Post(d, null);
            }
        }

        private void _RequestCallback(IAsyncResult ar)
        {
            if (this._request != null)
            {
                //Thread.Sleep(5000);
                try
                {
                    using (StreamWriter writer = new StreamWriter(this._request.EndGetRequestStream(ar)))
                    {
                        if (this._isCanceled)
                        {
                            return;
                        }

                        if ((this.RequestBodyBinarys.Count == 0) && (this.RequestBodyFileInfos.Count == 0))
                        {
                            // >= > == 뭐가 가장 빠를까 || && 뭐가 빠를까
                            if (this.RequestBodyStrings.Count > 0 || this.RequestBodyJsonObjects.Count > 0)
                            {
                                writer.Write(this._QueryString);
                                writer.Flush();
                            }
                        }
                        else
                        {
                            /*
                            writer.WriteLine("--" + this._multiPartBoundary);
                            writer.WriteLine();
                            writer.Flush();
                            */

                            if (this.RequestBodyStrings.Count > 0)
                            {
                                foreach (KeyValuePair<string, string> pair in this.RequestBodyStrings)
                                {
                                    if (this._isCanceled)
                                    {
                                        return;
                                    }
                                    if (pair.Value != null)
                                    {
                                        writer.WriteLine("--" + this._multiPartBoundary);
                                        writer.WriteLine("Content-Disposition: form-data; name=\"{0}\"", pair.Key);
                                        writer.WriteLine();
                                        writer.WriteLine(pair.Value);
                                        writer.Flush();
                                    }
                                }
                            }
                            if (this.RequestBodyBinarys.Count > 0)
                            {
                                foreach (KeyValuePair<string, HttpHelperMultiPartData> pair in this.RequestBodyBinarys)
                                {
                                    if (this._isCanceled)
                                    {
                                        return;
                                    }
                                    HttpHelperMultiPartData data = pair.Value;
                                    if (data.Data.Length > 0)
                                    {
                                        writer.WriteLine("--" + this._multiPartBoundary);
                                        writer.WriteLine("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", pair.Key, data.FileName);
                                        writer.WriteLine("Content-Transfer-Encoding: binary");
                                        writer.WriteLine("Content-Length: " + data.Data.Length);
                                        writer.WriteLine();
                                        writer.Flush();

                                        byte[] buffer = new byte[HttpHelper._UPLOAD_BUFFER_SIZE];
                                        int num = 0;
                                        while (num < (data.Data.Length / HttpHelper._UPLOAD_BUFFER_SIZE)) // 계속 연산하는가?
                                        {
                                            if (this._isCanceled)
                                            {
                                                return;
                                            }
                                            Buffer.BlockCopy(data.Data, num * HttpHelper._UPLOAD_BUFFER_SIZE, buffer, 0, HttpHelper._UPLOAD_BUFFER_SIZE);
                                            writer.BaseStream.Write(buffer, 0, HttpHelper._UPLOAD_BUFFER_SIZE);
                                            num++;
                                            /*
                                            if (this.RequestProgressChanged != null)
                                            {
                                                HttpHelperUploadProgressEventArgs args = new HttpHelperUploadProgressEventArgs(data.FileName, data.Data.Length, num * HttpHelper._UPLOAD_BUFFER_SIZE);
                                                this.RequestProgressChanged(this, args);
                                            }
                                            */
                                        }
                                        int count = data.Data.Length - (num * HttpHelper._UPLOAD_BUFFER_SIZE);
                                        if (count > 0)
                                        {
                                            Buffer.BlockCopy(data.Data, num * HttpHelper._UPLOAD_BUFFER_SIZE, buffer, 0, count);
                                            writer.BaseStream.Write(buffer, 0, count);
                                            /*
                                            if (this.RequestProgressChanged != null)
                                            {
                                                HttpHelperUploadProgressEventArgs args = new HttpHelperUploadProgressEventArgs(data.FileName, data.Data.Length, data.Data.Length);
                                                this.RequestProgressChanged(this, args);
                                            }
                                            */
                                        }
                                        writer.BaseStream.Flush();
                                        writer.WriteLine();
                                        writer.Flush();
                                    }
                                }
                            }
                            if (this.RequestBodyFileInfos.Count > 0)
                            {
                                if (this._isCanceled)
                                {
                                    return;
                                }
                                foreach (KeyValuePair<string, FileInfo> pair3 in this.RequestBodyFileInfos)
                                {
                                    if (this._isCanceled)
                                    {
                                        return;
                                    }
                                    FileInfo info = pair3.Value;
                                    if (info != null && info.Length > 0)
                                    {
                                        writer.WriteLine("--" + this._multiPartBoundary);
                                        writer.WriteLine("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", pair3.Key, info.Name);
                                        writer.WriteLine("Content-Transfer-Encoding: binary");
                                        writer.WriteLine("Content-Length: " + info.Length);
                                        writer.WriteLine();
                                        writer.Flush();

                                        Stream stream = info.OpenRead();
                                        byte[] buffer = new byte[HttpHelper._UPLOAD_BUFFER_SIZE];
                                        int length = 0;
                                        while(true)
                                        {
                                            if (this._isCanceled)
                                            {
                                                return;
                                            }
                                            length = stream.Read(buffer, 0, HttpHelper._UPLOAD_BUFFER_SIZE);
                                            if (length > 0)
                                            {
                                                writer.BaseStream.Write(buffer, 0, length);
                                            }
                                            {
                                                break;
                                            }
                                        };
                                        writer.BaseStream.Flush();
                                        writer.WriteLine();
                                        writer.Flush();
                                    }
                                }
                            }
                            writer.WriteLine("--" + this._multiPartBoundary + "--");
                            writer.Flush();
                        }
                        if (this._isCanceled)
                        {
                            return;
                        }
                        this._request.BeginGetResponse(new AsyncCallback(this._ResponseCallback), null);
                    }
                }
                catch (Exception exception)
                {
                    if (this._currentContext == null)
                    {
                        this._OnPostException(exception);
                    }
                    else
                    {
                        this._currentContext.Post(new SendOrPostCallback(this._OnPostException), exception);
                    }
                }
            }
        }

        private void _ResponseCallback(IAsyncResult ar)
        {
            if (this._request != null)
            {
                this._isFinished = true;
                try
                {
                    this._response = this._request.EndGetResponse(ar) as HttpWebResponse;
                    if (this._currentContext == null)
                    {
                        this._OnPostResponse(null);
                    }
                    else
                    {
                        this._currentContext.Post(new SendOrPostCallback(this._OnPostResponse), null);
                    }
                }
                catch (Exception exception)
                {
                    if (this._currentContext == null)
                    {
                        this._OnPostException(exception);
                    }
                    else
                    {
                        this._currentContext.Post(new SendOrPostCallback(this._OnPostException), exception);
                    }
                }
            }
        }
        
        //MainThread에서 실행? Context스위칭? 마지막에 해야는거아닌가. 스트림 읽는데 시간 많이걸림.
        //메인쓰레드는 콜드?
        private void _OnPostResponse(object state)
        {
            if (this._response != null)
            {
                if (this._response.Cookies != null)
                {
                    this.RequestCookies.Add(this._response.Cookies);
                }
                if (this._response.StatusCode == HttpStatusCode.OK)
                {
                    if (this.ResponseSucceeded != null)
                    {
                        using (Stream responseStream = this._response.GetResponseStream())
                        {
                            if (responseStream != null)
                            {
                                Encoding responseEncoding = _GetCharacterSet(this._response.CharacterSet);
                                using (StreamReader reader = new StreamReader(responseStream, responseEncoding, true))
                                {
                                    if (reader != null)
                                    {
                                        string content = reader.ReadToEnd(); //읽는동안 바뀜방지
                                        if (this.ResponseSucceeded != null)
                                        {
                                            HttpHelperEventArgs args = new HttpHelperEventArgs
                                            {
                                                HttpWebResponseObj = this._response,
                                                Message = "HttpHelper.ResponseSucceeded",
                                                Content = content,
                                                StreamReaderObj = reader,
                                                ExceptionObj = null,
                                                Cookies = this.RequestCookies
                                            };
                                            this.ResponseSucceeded(this, args); // 이게 끝날떄까지 Class는 Cold?
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (this.ResponseFailed != null)
                {
                    HttpHelperEventArgs args = new HttpHelperEventArgs
                    {
                        HttpWebResponseObj = this._response,
                        Message = "HttpHelper.ResponseFailed",
                        Content = this._response.StatusCode.ToString(),
                        StreamReaderObj = null,
                        ExceptionObj = null,
                        Cookies = this.RequestCookies
                    };
                    this.ResponseFailed(this, args);
                }
            }
        }

        private void _OnPostException(object state)
        {
            if (this.ResponseFailed != null)
            {
                Exception exception = state as Exception;
                if (exception != null)
                {
                    HttpHelperEventArgs e = new HttpHelperEventArgs
                    {
                        Message = "HttpHelper.ResponseFailed",
                        Content = exception.Message,
                        StreamReaderObj = null,
                        ExceptionObj = exception,
                        Cookies = this.RequestCookies
                    };
                    this.ResponseFailed(this, e);
                }
            }
        }

        public HttpHelper()
        {
            if (!_isInitialized)
            {
                //WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
                //WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);
                System.Net.ServicePointManager.Expect100Continue = false;
                _isInitialized = true;
            }
            this.Method = "POST";
            this.RequestHeaders = new Dictionary<string, string>();
            this.RequestBodyStrings = new Dictionary<string, string>();
            this.RequestBodyJsonObjects = new Dictionary<string, object>();
            this.RequestBodyBinarys = new Dictionary<string, HttpHelperMultiPartData>();
            this.RequestBodyFileInfos = new Dictionary<string, FileInfo>();
            this.RequestCookies = new CookieContainer();
        }

        public void Execute()
        {
            if (this._isFinished)
            {
                this._request = null;
                this._response = null;
                this._progressCurrent = 0.0;
                this._isCanceled = false;
                this._isFinished = false;
                if (this.RequestUri != null)
                {
                    try
                    {
                        if (this.Method == "GET")
                        {
                            if (this.RequestBodyStrings.Count > 0 || this.RequestBodyJsonObjects.Count > 0)
                            {
                                this.RequestUri = new Uri(string.Concat(this.RequestUri.AbsoluteUri, "?", this._QueryString));
                            }
                        }

                        this._request = WebRequest.Create(this.RequestUri) as HttpWebRequest;
                        this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        this._request.Method = this.Method;
                        foreach (KeyValuePair<string, string> pair2 in this.RequestHeaders)
                        {
                            if (string.Equals(pair2.Key, "Accept", StringComparison.OrdinalIgnoreCase))
                            {
                                this._request.Accept = pair2.Value;
                            }
                            else if (string.Equals(pair2.Key, "User-Agent", StringComparison.OrdinalIgnoreCase))
                            {
                                this._request.UserAgent = pair2.Value;
                            }
                            else if (string.Equals(pair2.Key, "Content-Type"))
                            {
                                this._request.ContentType = pair2.Value;
                            }
                            else
                            {
                                this._request.Headers[pair2.Key] = pair2.Value;
                            }
                        }
                        this._request.CookieContainer = this.RequestCookies;

                        if (this.Method == "GET")
                        {
                            this._request.BeginGetResponse(new AsyncCallback(this._ResponseCallback), null);
                        }
                        else if (this.Method == "POST")
                        {
                            if (string.IsNullOrEmpty(this._request.ContentType))
                            {
                                if (this.RequestBodyBinarys.Count == 0 && this.RequestBodyFileInfos.Count == 0)
                                {
                                    this._request.ContentType = "application/x-www-form-urlencoded";
                                }
                                else
                                {
                                    this._request.ContentType = "multipart/form-data; boundary=" + this._multiPartBoundary;
                                }
                            }
                            this._request.BeginGetRequestStream(new AsyncCallback(this._RequestCallback), null);
                        }
                        this._StartMagicProgress();
                    }
                    catch (Exception exception)
                    {
                        if (this.ResponseFailed != null)
                        {
                            HttpHelperEventArgs e = new HttpHelperEventArgs
                            {
                                Message = "HttpHelper.ResponseFailed",
                                Content = exception.Message,
                                StreamReaderObj = null,
                                ExceptionObj = exception,
                                Cookies = this.RequestCookies
                            };
                            this.ResponseFailed(this, e);
                        }
                    }
                }
            }
        }

        public void Cancel()
        {
            this._isCanceled = true;
            this._isFinished = true;
            if (this._request != null)
            {
                try
                {
                    this._request.Abort();
                }
                catch
                {
                }
                this._request = null;
            }
            if (this._response != null)
            {
                try
                {
                    this._response.Close();
                }
                catch
                {
                }
                this._response = null;
            }
        }
    }
}

