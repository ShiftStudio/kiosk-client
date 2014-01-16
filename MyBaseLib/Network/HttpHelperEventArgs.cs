using System;
using System.IO;
using System.Net;

namespace MyBaseLib.Network
{
    public class HttpHelperEventArgs : EventArgs
    {
        public HttpHelperEventArgs()
        {
            this.Cookies = new CookieContainer();
        }

        public string Content { get; set; }

        public CookieContainer Cookies { get; set; }

        public Exception ExceptionObj { get; set; }

        public HttpWebResponse HttpWebResponseObj { get; set; }

        public string Message { get; set; }

        public StreamReader StreamReaderObj { get; set; }
    }
}

