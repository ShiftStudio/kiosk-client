using MyBaseLib.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace MyBaseLib.Network
{
    public abstract class HttpApiBase
    {
        protected HttpHelper _httpHelper = new HttpHelper();
        private DispatcherTimer _timerRequestTimeOut = new DispatcherTimer();

        public event EventHandler<HttpHelperEventArgs> ResponseFailed;

        public event EventHandler<HttpApiResponseBase> ResponseSucceeded;

        public event EventHandler<HttpHelperUploadProgressEventArgs> RequestProgressChanged;

        public HttpApiBase()
        {
            this._httpHelper.ResponseFailed += this._httpHelper_ResponseFailed; // new EventHandler<HttpHelperEventArgs>(this._httpHelper_ResponseFailed);
            this._httpHelper.ResponseSucceeded += this._httpHelper_ResponseSucceeded; // new EventHandler<HttpHelperEventArgs>(this._httpHelper_ResponseSucceeded);
            this._httpHelper.RequestProgressChanged += this._httpHelper_RequestProgressChanged;//new EventHandler<HttpHelperUploadProgressEventArgs>(this._httpHelper_RequestProgressChanged);

            this.RequestTimeOut = TimeSpan.FromSeconds(100.0);
            this.AsyncContext = new Dictionary<string, object>();
        }

        protected abstract void _GenerateRequest(HttpHelper httpHelper, HttpApiRequestBase httpApiRequest);

        protected abstract HttpApiResponseBase _GenerateResponse(HttpHelperEventArgs httpHelperEventArgs);

        private void _httpHelper_RequestProgressChanged(object sender, HttpHelperUploadProgressEventArgs e)
        {
            this._timerRequestTimeOut.Stop();
            if (this.RequestProgressChanged != null)
            {
                this.RequestProgressChanged(this, e);
            }
        }

        private void _httpHelper_ResponseFailed(object sender, HttpHelperEventArgs e)
        {
            this._timerRequestTimeOut.Stop();
            StringBuilder builder = new StringBuilder();
            builder.Append("http request 실패 \n");
            builder.Append(string.Format("http request url : {0} \n", this._httpHelper.RequestUri.ToString()));
            DebugEx.WriteLine(builder.ToString());
            if (this.ResponseFailed != null)
            {
                this.ResponseFailed(this, e);
            }
        }

        private void _httpHelper_ResponseSucceeded(object sender, HttpHelperEventArgs e)
        {
            this._timerRequestTimeOut.Stop();
            this.HttpApiResponse = this._GenerateResponse(e); // 이벤트아니여도 반환할수도 ㅎㅎ
            StringBuilder builder = new StringBuilder();
            builder.Append("http request 성공 \n");
            builder.Append(string.Format("http request url : {0} \n", this._httpHelper.RequestUri.ToString()));
            builder.Append(string.Format("http response : \n{0} \n", JsonConvert.SerializeObject(this.HttpApiResponse)));
            DebugEx.WriteLine(builder.ToString());
            if (this.ResponseSucceeded != null)
            {
                this.ResponseSucceeded(this, this.HttpApiResponse);
            }
        }

        private void _timerRequestTimeOut_Tick(object sender, EventArgs e)
        {
            this._timerRequestTimeOut.Stop();
            StringBuilder builder = new StringBuilder();
            builder.Append("http request 실패 \n");
            builder.Append(string.Format("http request url : {0} \n", this._httpHelper.RequestUri.ToString()));
            DebugEx.WriteLine(builder.ToString());
            if ((this.HttpApiResponse == null) && (this.ResponseFailed != null))
            {
                HttpHelperEventArgs args = new HttpHelperEventArgs
                {
                    HttpWebResponseObj = null,
                    Message = "HttpHelper.ResponseFailed",
                    Content = null,
                    StreamReaderObj = null,
                    ExceptionObj = new TimeoutException(),
                    Cookies = _httpHelper.RequestCookies
                };
                this.ResponseFailed(this, args);
                //TimeOutException맞나?
            }
        }

        public void Cancel()
        {
            this._httpHelper.Cancel();
        }

        public void Send(HttpApiRequestBase httpApiRequest)
        {
            //DebugEx.Assert(!this._isSended, "HttpApiBase.Send는 한개의 인스턴스당 한번만 호출가능.\n추가로 Send를 하고 싶다면 Api객체를 한개 더 만들어야 함.");
            //this._httpHelper.RequestHeaders.Clear();
            //this._httpHelper.RequestCookies.Clear();
            this._httpHelper.RequestBodyStrings.Clear();
            this._httpHelper.RequestBodyJsonObjects.Clear();
            this._httpHelper.RequestBodyBinarys.Clear();
            this._httpHelper.RequestBodyFileInfos.Clear();

            this.HttpApiRequest = httpApiRequest;
            this._GenerateRequest(this._httpHelper, this.HttpApiRequest);
            this.RequestUrlStr = this._httpHelper.RequestUri.ToString();
            this._httpHelper.Execute();

            this._timerRequestTimeOut.Interval = this.RequestTimeOut;
            this._timerRequestTimeOut.Tick += new EventHandler(this._timerRequestTimeOut_Tick);
            this._timerRequestTimeOut.Start();

            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("http request url : {0} \n", this._httpHelper.RequestUri.ToString()));
            if (!this._httpHelper.RequestUri.ToString().Contains("upload"))
            {
                builder.Append(string.Format("http request : {0} \n", JsonConvert.SerializeObject(httpApiRequest)));
                DebugEx.WriteLine(builder.ToString());
            }
        }

        public Dictionary<string, object> AsyncContext { get; private set; }

        public HttpApiRequestBase HttpApiRequest { get; private set; }

        public HttpApiResponseBase HttpApiResponse { get; protected set; }

        public TimeSpan RequestTimeOut { get; set; }

        public string RequestUrlStr { get; set; }
    }
}