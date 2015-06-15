using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace api.louisukiri.com.Tests.helpers
{
    public class virtualServer
    {
        HttpClient _client;
        HttpServer _server;
        HttpConfiguration _config;
        IHttpRouteData _routeData;
        IHttpControllerSelector _controllerSelector;
        HttpControllerContext _controllerContext;
        string _baseUrl;
        public virtualServer(string baseUrl = "http://test.com/api")
        {
           _config = new HttpConfiguration();
            _config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            //setup configuration
            WebApiConfig.Register(_config);

            //setup server 
            _server = new HttpServer(_config);
            _client = new HttpClient(_server);
            _baseUrl = baseUrl;
        }
        public HttpConfiguration Config
        {
            get
            {
                return _config;
            }
        }
        public HttpServer Server
        {
            get{ return _server; }
        }
        public HttpClient Client
        {
            get{ return _client; }
        }
        public HttpResponseMessage getAPIResponse(string relativeUri)
        {
            return getAPIResponse(relativeUri, HttpMethod.Get);
        }
        public HttpResponseMessage getAPIResponse(string URI, HttpMethod method)
        {
            var req = getRequestMessage(_baseUrl + URI, method);
            return getAPIResponse(req);
        }
        public HttpResponseMessage getAPIResponse(HttpRequestMessage request)
        {
            return _client.SendAsync(request).Result;
        }
        public Type ControllerType(HttpRequestMessage request, out HttpResponseMessage response)
        {
            response = getAPIResponse(request);
            _routeData = _config.Routes.GetRouteData(request);
            _controllerSelector = new DefaultHttpControllerSelector(_config);
            _controllerContext = new HttpControllerContext(_config, _routeData, request);
            var descriptor = _controllerSelector.SelectController(request);
            _controllerContext.ControllerDescriptor = descriptor;
            return descriptor.ControllerType;
        }
        public string ActionName(HttpRequestMessage request, out HttpResponseMessage response)
        {

            if(!(_controllerContext != null && _controllerContext.ControllerDescriptor!=null))
            {
                ControllerType(request, out response);
            }
            else
            {
                response = null;
            }
            var actionSelector = new ApiControllerActionSelector();
            var descriptor = actionSelector.SelectAction(_controllerContext);

            return descriptor.ActionName.ToLower();
        }
        public HttpRequestMessage getRequestMessageWithPartialUri(string Uri)
        {
            return getRequestMessage(_baseUrl + Uri);
        }
        public static HttpRequestMessage getRequestMessage(string baseUri)
        {
            return getRequestMessage(baseUri, HttpMethod.Get);
        }
        public static HttpRequestMessage getRequestMessage(string baseUri, HttpMethod method)
        {
            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri(baseUri);
            message.Method = method;

            return message;
        }
    }
}
