

using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.entity;
using cicdDomain.cicd.domain.service;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        public void WhenRepoReturnsJobWithExceptionReturnJobWithException()
        {
            string id = "testID";
            _jobRepo.Setup(z => z.getJobBy(It.IsAny<string>()))
                .Returns(getJob(false));

            CICDService sut = getCICDService();
            var res = sut.trigger();

            Assert.IsFalse(res.LastExecution.isSuccessful);
            Assert.IsTrue(res.LastExecution.Messages.Count > 0);
        }
        [Test]
        public void WhenRepoReturnsJobWithExceptionDontBuild()
        {
            string id = "testID";
            _jobRepo.Setup(z => z.getJobBy(It.IsAny<string>()))
                .Returns(getJob(false));
            _buildService.Setup(z => z.build(It.IsAny<Job>()));

            CICDService sut = getCICDService();
            var res = sut.trigger();

            _buildService.Verify(z => z.build(It.IsAny<Job>()), Times.Never());
        }
        //[Test]
        //public void WhenTriggerSuccessfulReturnTriggerResult()
        //{
        //    _buildService.Setup(z => z.build())
        //        .Returns(getJob());

        //    CICDService sut = getCICDService();
        //    Job result = sut.trigger();

        //    Assert.IsTrue(result.LastExecution.isSuccessful);
        //}
        //[Test]
        //public void WhenBuildFailsReturnUnsuccessfulJob()
        //{
        //    _buildService.Setup(z => z.build())
        //        .Returns(getJob(false));

        //    CICDService sut = getCICDService();
        //    Job result = sut.trigger();

        //    Assert.IsFalse(result.LastExecution.isSuccessful);
        //}
        private Job FailedJob()
        {
            return getJob(false);
        }
        private Job getJob(bool successful=true)
        {
            List<string> messages = new List<string>();
            if (!successful)
            {
                messages.Add("Error");
            }
            return new Job
                {
                    Executions = new List<Execution> { 
                        new Execution(successful, DateTime.Now, messages)
                    }
                };
        }
        private CICDService getCICDService()
        {
            return new CICDService(_jobRepo.Object, _buildService.Object);
        }
    }
}
