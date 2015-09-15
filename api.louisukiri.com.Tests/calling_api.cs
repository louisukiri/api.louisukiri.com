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
        public void CallingPushSelectPushActionGivenValidPayload()
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
        [Test]
        public void CallingTestDataSelectTestDataActionGivenValidPayload()
        {
            var server = new virtualServer();
            var response = new HttpResponseMessage();
            var req = server.getRequestMessageWithPartialUri("/v1/push");
            req.Method = HttpMethod.Post;
            req.Content = new StringContent(testInfrastructure.TestdataContent);
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
        }
        [Test, Ignore, Category("Integrated Test")]
        public void TestdataIntegratedTest()
        {
            var server = new virtualServer();
            var req = server.getRequestMessageWithPartialUri("/v1/testdata");
            req.Method = HttpMethod.Post;
            req.Content = new StringContent(testInfrastructure.TestdataContent);
            req.Content.Headers.Clear();
            req.Content.Headers.Add("content-type", "application/json");

            var response = server.getAPIResponse(req);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [Test]
        public void CallingPushReturn404WhenMissingPushContent()
        {
            var server = new virtualServer();
            var response = new HttpResponseMessage();
            var req = server.getRequestMessageWithPartialUri("/v1/push");
            req.Method = HttpMethod.Post;
            Type t = server.ControllerType(req, out response);

            Assert.AreEqual(t, typeof(TriggersController));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Test]
        public void CallingTestResultsReturn404WhenMissingTestData()
        {
            var server = new virtualServer();
            var response = new HttpResponseMessage();
            var req = server.getRequestMessageWithPartialUri("/v1/testdata");
            req.Method = HttpMethod.Post;
            ////req.Content = new StringContent(GitHubPushContent);
            //req.Content.Headers.Clear();
            //req.Content.Headers.Add("content-type", "application/json");

            Type t = server.ControllerType(req, out response);

            Assert.AreEqual(t, typeof(TriggersController));
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Test, Ignore]
        public void CallingPushReturn404WhenInvalidPayload()
        {
            //the question what is a valid push payload needs to be answered first
            var server = new virtualServer();
            var response = new HttpResponseMessage();
            var req = server.getRequestMessageWithPartialUri("/v1/push");
            req.Method = HttpMethod.Post;
            req.Content = new StringContent(testInfrastructure.BadPushPayload);
            req.Content.Headers.Clear();
            req.Content.Headers.Add("content-type", "application/json");

            Type t = server.ControllerType(req, out response);

            Assert.AreEqual(t, typeof(TriggersController));
        }

    }
}
