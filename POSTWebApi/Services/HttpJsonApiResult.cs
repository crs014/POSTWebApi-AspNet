using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace POSTWebApi.Services
{
    public class HttpJsonApiResult<P> : IHttpActionResult
    {

        private HttpRequestMessage _request;

        public P DataValue;
        public HttpStatusCode StatusCode { get; set; }

        public HttpJsonApiResult(P value, HttpRequestMessage request, HttpStatusCode httpStatusCode)
        {
            StatusCode = httpStatusCode;
            DataValue = value;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage()
            {
                Content = new ObjectContent(
                    typeof(HttpJsonApiDataTemplate<P>), new HttpJsonApiDataTemplate<P>()
                    {
                        Data = DataValue,
                        HttpStatus = StatusCode
                    },
                    new JsonMediaTypeFormatter()),
                StatusCode = StatusCode,
                RequestMessage = _request
            };

            return Task.FromResult(response);
        }

        protected class HttpJsonApiDataTemplate<H>
        {
            public H Data { get; set; }
            public HttpStatusCode HttpStatus { get; set; }
        }
    }
}