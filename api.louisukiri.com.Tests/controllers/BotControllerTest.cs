using System.Net;
using api.louisukiri.com.Controllers;
using cicd.domain.context.trigger.abstracts;
using cicdDomain;
using godaddy.domains.cicd.Controllers;
using Moq;
using NUnit.Framework;

namespace api.louisukiri.com.Tests.controllers
{
    [TestFixture]
    public class BotControllerTest
    {
        Mock<IBot> _mockBot;
        private BotController _sut;
        [SetUp]
        public void Setup()
        {
            _mockBot = new Mock<IBot>();
            _mockBot.Setup(z => z.Trigger(It.IsAny<IEvent>()));
            _sut = new BotController(_mockBot.Object);
        }
        [Test]
        public void PostGivenNullReturnsBadRequest()
        {
            _mockBot.Setup(z => z.Trigger(It.IsAny<IEvent>()));
            var sut = new BotController(_mockBot.Object);
            var res = sut.Post(null);

            Assert.AreEqual(HttpStatusCode.BadRequest, res.StatusCode);
        }
        [Test]
        public void PostGivenValidCallsBotTrigger()
        {
            var MockBot = new Mock<IBot>();
            MockBot.Setup(z => z.Trigger(It.IsAny<IEvent>()));
            var sut = new BotController(MockBot.Object);
            var res = sut.Post(testInfrastructure.GenericApiDeployEvent);

            MockBot.Verify(z => z.Trigger(It.IsAny<IEvent>()), Times.Once());
        }
        [Test]
        public void PostGivenInvalidEventReturnsBadRequest()
        {
            var MockBot = new Mock<IBot>();
            MockBot.Setup(z => z.Trigger(It.IsAny<IEvent>()));
            var sut = new BotController(MockBot.Object);
            var genericApiEvent = testInfrastructure.GenericApiDeployEvent;
            genericApiEvent.Type = testInfrastructure.random;
            var res = sut.Post(genericApiEvent);

            Assert.AreEqual(HttpStatusCode.BadRequest, res.StatusCode);
        }
    }
}
