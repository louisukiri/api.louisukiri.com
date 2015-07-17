using System;
using api.louisukiri.com.Tests.helpers;
using System.Net;
using System.Net.Http;
using api.louisukiri.com.Controllers;
using cicdDomain;
using NUnit.Framework;

namespace api.louisukiri.com.Tests
{
    
    public class calling_api
    {
        [Test,Ignore]
        public void testWorks()
        {
            var server = new virtualServer();
            var resp = server.getAPIResponse("/v1/test");

            Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
        }
        [Test]
        public void selectPushActionGivenPushPayload()
        {
            var server = new virtualServer();
            var response = new HttpResponseMessage();
            var req = server.getRequestMessageWithPartialUri("/v1/push");
            req.Method = HttpMethod.Post;
            req.Content = new StringContent(testInfrastructure.GitHubPushContent);
            req.Content.Headers.Clear();
            req.Content.Headers.Add("content-type", "application/json");
 
            Type t = server.ControllerType(req, out response);

            Assert.AreEqual(t, typeof(TriggersController));
            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            //Assert.AreEqual("", response.Content.ReadAsStringAsync().Result);

        }
        [Test, Ignore, Category("Integrated Test")]
        public void pushIntegratedTest()
        {
          var server = new virtualServer();
          var req = server.getRequestMessageWithPartialUri("/v1/push");
          req.Method = HttpMethod.Post;
          req.Content = new StringContent(testInfrastructure.GitHubPushContent);
          req.Content.Headers.Clear();
          req.Content.Headers.Add("content-type", "application/json");

          var response = server.getAPIResponse(req);

          Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

          //Assert.AreEqual("", response.Content.ReadAsStringAsync().Result);

        }
        [Test]
        public void returnNotFoundWhenMissingPushContent()
        {
            var server = new virtualServer();
            var response = new HttpResponseMessage();
            var req = server.getRequestMessageWithPartialUri("/v1/push");
            req.Method = HttpMethod.Post;
            ////req.Content = new StringContent(GitHubPushContent);
            //req.Content.Headers.Clear();
            //req.Content.Headers.Add("content-type", "application/json");

            Type t = server.ControllerType(req, out response);

            Assert.AreEqual(t, typeof(TriggersController));
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}
