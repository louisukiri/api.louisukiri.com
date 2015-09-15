
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.services;
using cicdDomain;
using Moq;
using NUnit.Framework;

namespace api.louisukiri.com.Tests.service
{
    [TestFixture]
    public class VcApplicationServiceTest
    {
        [SetUp]
        public void setup()
        {
            
        }
        [Test]
        public void CtorCanAcceptCommunicationBot()
        {
            var mockIbot = new Mock<IBot>();
            var sut = new VcAppService(mockIbot.Object);

            Assert.IsNotNull(sut.ComBot);
        }
        [Test]
        public void TestResultWillCommunicateResultsToBot()
        {
            var mockIbot = new Mock<IBot>();
            mockIbot.Setup(z => z.Trigger(It.IsAny<IEvent>()));
            var sut = new VcAppService(mockIbot.Object);
            sut.TestResult(testInfrastructure.TestDataValidPayload);

            mockIbot.Verify(z=> z.Trigger(It.IsAny<IEvent>()));
        }
    }
}
