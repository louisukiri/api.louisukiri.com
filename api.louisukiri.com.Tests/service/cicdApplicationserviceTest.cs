

using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.service;
using cicdDomain.cicd.infrastructure.dtos;
using Moq;
using NUnit.Framework;
namespace api.louisukiri.com.Tests.service
{
    [TestFixture]
    public partial class cicdApplicationserviceTest
    {
        Mock<IJobRepo> _jobRepo;
        Mock<IBuildService> _buildService;

        [TestFixtureSetUp]
        public void setup()
        {
            _jobRepo = new Mock<IJobRepo>();
            _buildService = new Mock<IBuildService>();
        }
        [Test]
        public void CicdApplicationServiceRequiresJobRepo()
        {
            CICDService sut = getCICDService();
            Assert.IsNotNull(sut.JobRepo);
        }
        [Test]
        public void CicdApplicationServiceRequiresBuildService()
        {
            CICDService sut = getCICDService();
            Assert.IsNotNull(sut.BuildService);
        }
        [Test]
        public void WhenTriggerSuccessfulReturnTriggerResult()
        {
            CICDService sut = getCICDService();
            TriggerResult result = sut.trigger();

            Assert.IsTrue(result.wasSuccessful);
        }
        [Test]
        public void WhenBuildFailsReturnUnsuccessfulTriggerResult()
        {
            CICDService sut = getCICDService();
            TriggerResult result = sut.trigger();

            Assert.IsTrue(result.wasSuccessful);
        }
        [Test]
        public void WhenBuildFailsReturnTriggerResultMessages()
        {
            CICDService sut = getCICDService();
            TriggerResult result = sut.trigger();

            Assert.IsTrue(result.wasSuccessful);
        }
        private CICDService getCICDService()
        {
            return new CICDService(_jobRepo.Object, _buildService.Object);
        }
    }
}
