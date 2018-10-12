using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebLog
{
    public class LogHandler: DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var logMetadata = buildRequestMetadata(request);
            var response = await base.SendAsync(request, cancellationToken);
            logMetadata = buildResponseMetadata(logMetadata, response);
            sendToLog(logMetadata);
            return response;
        }

        private WebLogModel buildRequestMetadata(HttpRequestMessage request)
        {
            WebLogModel log = new WebLogModel
            {
                RequestTime = DateTime.Now,
                RequestUri = request.RequestUri,
                RequestMethod = request.Method,
                RequestHeaders = request.Headers,
                RequestContentText = (request.Content != null)?request.Content.ReadAsStringAsync().Result:string.Empty,
                RequestContentHeaders = request.Content.Headers,
                RequestVersion = request.Version,
                RequestClientIP = request.GetClientIpAddress()
            };

            return log;
        }

        private WebLogModel buildResponseMetadata(WebLogModel logMetadata, HttpResponseMessage response)
        {
            logMetadata.ResponseTime = DateTime.Now;
            logMetadata.ResponseStatusCode = response.StatusCode;
            if(response.Content != null)
            {
                logMetadata.ResponseHeaders = response.Content.Headers;
                logMetadata.ResponseContentText = (response.Content != null) ?response.Content.ReadAsStringAsync().Result:string.Empty;
            }
            logMetadata.ResponseVersion = response.Version;
            return logMetadata;
        }

        private bool sendToLog(WebLogModel logMetadata)
        {
            Log log = new Log();

            log.Save(logMetadata);

            return true;
        }
    }
}