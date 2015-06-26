

using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.service;
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
        public void cicdApplicationServiceRequiresJobRepo()
        {
            CICDService sut = getCICDService();
            Assert.IsNotNull(sut.JobRepo);
        }
        [Test]
        public void cicdApplicationServiceRequiresBuildService()
        {
            CICDService sut = getCICDService();
            Assert.IsNotNull(sut.BuildService);
        }
        //[Test]
        //public void whenTriggerReturn
        private CICDService getCICDService()
        {
            return new CICDService(_jobRepo.Object, _buildService.Object);
        }
    }
}
