using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace WebLog
{
    public class WebLogModel
    {
        public WebLogModel()
        {
            this.RequestContentText = string.Empty;
            this.RequestClientIP = string.Empty;
            this.ResponseContentText = string.Empty;
        }
        public long ID { get; set; }

        public DateTime RequestTime { get; set; }
        public Uri RequestUri { get; set; }
        public HttpMethod RequestMethod { get; set; }
        public HttpRequestHeaders RequestHeaders { get; set; }
        public HttpContentHeaders RequestContentHeaders { get; set; }
        public string RequestContentText { get; set; }
        public Version RequestVersion { get; set; }
        public string RequestClientIP { get; set; }

        public DateTime ResponseTime { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; }
        public HttpContentHeaders ResponseHeaders { get; set; }
        public string ResponseContentText { get; set; }
        public Version ResponseVersion { get; set; }

        public string RequestHost {
            get
            {
                return this.RequestUri.Host;
            }
        }
        public string RequestUriText
        {
            get
            {
                return this.RequestUri.ToString();
            }
        }
        public string SchemeText
        {
            get
            {
                return this.RequestUri.Scheme;
            }
        }
        public string SiteText
        {
            get
            {
                return this.RequestUri.Authority;
            }
        }
        public string PathText
        {
            get
            {
                return this.RequestUri.LocalPath;
            }
        }
        public string QueryText
        {
            get
            {
                return this.RequestUri.Query;
            }
        }

        public string RequestMethodText
        {
            get
            {
                return this.RequestMethod.Method;
            }
        }
        public string RequestHeadersText
        {
            get
            {
                if(this.RequestHeaders == null)
                {
                    return string.Empty;
                }

                StringBuilder bld = new StringBuilder();

                foreach (var header in this.RequestHeaders)
                {
                    bld.AppendLine($"{header.Key}:{string.Join(",",header.Value)}");
                }

                foreach (var header in this.RequestContentHeaders)
                {
                    bld.AppendLine($"{header.Key}:{string.Join(",", header.Value)}");
                }

                return bld.ToString();
            }
        }
        public string RequestVersionText
        {
            get
            {
                return this.RequestVersion.ToString();
            }
        }

        public string ResponseStatusCodeText
        {
            get
            {
                return this.ResponseStatusCode.ToString();
            }
        }
        public string ResponseHeadersText
        {
            get
            {
                if (this.ResponseHeaders == null)
                {
                    return string.Empty;
                }

                StringBuilder bld = new StringBuilder();

                foreach (var header in this.ResponseHeaders)
                {
                    bld.AppendLine($"{header.Key}:{string.Join(",", header.Value)}");
                }
                return bld.ToString();
            }
        }
        public string ResponseVersionText
        {
            get
            {
                return this.ResponseVersion.ToString();
            }
        }
    }
}