using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using api.louisukiri.com.Controllers;
using api.louisukiri.com.Tests.helpers;
using cicd.infrastructure.dtos;
using cicdDomain;
using godaddy.domains.cicd.Controllers;
using godaddy.domains.cicd.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace api.louisukiri.com.Tests
{
    [TestClass]
    public class BotApiTest
    {
        [TestMethod, TestCategory("Integrated Test"), Ignore]
        public void CallingBotControllerWithValidEventReturnsExpected()
        {
            var server = new virtualServer();
            var response = new HttpResponseMessage();
            GenericApiEvent _event = testInfrastructure.GenericApiDeployEvent;
            string validEventPayload = JsonConvert.SerializeObject(_event);
            var req = server.getRequestMessageWithPartialUri("/v1/bot/tell");
            req.Method = HttpMethod.Post;
            req.Content = new StringContent(validEventPayload);

            Type t = server.ControllerType(req, out response);

            Assert.AreEqual(t, typeof(BotController));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TestMethod, TestCategory("Integrated Test"), Ignore]
        public void CallingBotControllerWithInValidEventReturnsExpected()
        {
            var server = new virtualServer();
            var response = new HttpResponseMessage();
            const string validEventPayload = @"{""test"",""test2""}";
            var req = server.getRequestMessageWithPartialUri("/v1/bot/tell");
            req.Method = HttpMethod.Post;
            req.Content = new StringContent(validEventPayload);

            Type t = server.ControllerType(req, out response);

            Assert.AreEqual(t, typeof(BotController));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
