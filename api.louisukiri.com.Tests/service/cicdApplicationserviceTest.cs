

using cicdDomain;
using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.entity;
using cicdDomain.cicd.domain.service;
using cicdDomain.cicd.infrastructure;
using cicdDomain.cicd.infrastructure.dtos;
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
        Mock<IRequestFactory> _requestFactory;

      public RequestPayload request;
        [TestFixtureSetUp]
        public void setup()
        {
            _jobRepo = new Mock<IJobRepo>();
            _buildService = new Mock<IBuildService>();
          _requestFactory = new Mock<IRequestFactory>();
          request = new RequestPayload(RequestTrigger.Push, testInfrastructure.GitHubPushContent);
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
        public void CicdApplicationServiceRequiresRequestFactory()
        {
          CICDService sut = getCICDService();
          Assert.IsNotNull(sut.RequestFactory);
        }
        [Test]
        public void WhenRepoReturnsJobWithExceptionReturnJobWithException()
        {
          string id = "testID";
          _jobRepo.Setup(z => z.getJobBy(It.IsAny<string>()))
              .Returns(testInfrastructure.getJob(false));

          CICDService sut = getCICDService();
          var res = sut.trigger(request);

          Assert.IsFalse(res.LastExecution.isSuccessful);
          Assert.IsTrue(res.LastExecution.Messages.Count > 0);
        }
        [Test]
        public void WhenRequestIsNullReturnJobWithException()
        {
          string id = "testID";
          _jobRepo.Setup(z => z.getJobBy(It.IsAny<string>()))
              .Returns(testInfrastructure.getJob(false));

          CICDService sut = getCICDService();
          var res = sut.trigger(null);

          Assert.IsFalse(res.LastExecution.isSuccessful);
          Assert.IsTrue(res.LastExecution.Messages.Count > 0);
        }
        [Test]
        public void WhenRepoReturnsJobWithExceptionDontBuild()
        {
            string id = "testID";
            _jobRepo.Setup(z => z.getJobBy(It.IsAny<string>()))
                .Returns(testInfrastructure.getJob(false));
            _buildService.Setup(z => z.build(It.IsAny<Job>(), It.IsAny<pushactivity>()));

            CICDService sut = getCICDService();
            var res = sut.trigger(request);

            _buildService.Verify(z => z.build(It.IsAny<Job>(),It.IsAny<pushactivity>()), Times.Never());
        }
        [Test, Ignore]
        public void WhenCallingRunReturnInvalidResultIfPayloadStringNullOrEmpty()
        {
          CICDService sut = getCICDService();
          RequestPayload rqPayload = new RequestPayload(RequestTrigger.Branch
            , testInfrastructure.GitHubPushContent);

          IDomainResult ppayload = sut.run(rqPayload);

          Assert.IsTrue((ppayload as FailedRequest) != null);
        }
        [Test]
        public void CallingRunAndRequestPayloadIsNullReturnNullResult()
        {
          CICDService sut = getCICDService();
          var res = sut.run(null);
          Assert.IsInstanceOf<FailedRequest>(res);
        }
        [Test]
        public void CallingRunAndRequestObjectIsNullReturnFailedResult()
        {
          CICDService sut = getCICDService();
          var res = sut.run(null);
          Assert.IsInstanceOf<FailedRequest>(res);
        }
        [Test]
        public void CallingRunAndFailedJobReturnFailedRequest()
        {
          DomainRequest validRes = new DomainRequest();
          _requestFactory.Setup(z => z.getRequestFrom(It.IsAny<RequestPayload>()))
            .Returns(validRes);

          Mock<CICDService> mockSut = getCICDServiceMock();
          mockSut.Setup(z => z.trigger(It.IsAny<RequestPayload>()))
            .Returns(testInfrastructure.getJob(false));
          CICDService sut = mockSut.Object;

          var res = sut.run(testInfrastructure.getRequestPayload());
          Assert.IsInstanceOf<FailedRequest>(res);
        }
        [Test]
        public void CallingRunAndSuccessfulJobReturnsSuccessfulRequest()
        {
          DomainRequest validRes = new DomainRequest();
          _requestFactory.Setup(z => z.getRequestFrom(It.IsAny<RequestPayload>()))
            .Returns(validRes);

          Mock<CICDService> mockSut = getCICDServiceMock();
          mockSut.Setup(z => z.trigger(It.IsAny<RequestPayload>()))
            .Returns(testInfrastructure.getJob(true));
          CICDService sut = mockSut.Object;

          var res = sut.run(testInfrastructure.getRequestPayload());
          Assert.IsInstanceOf<SuccessfulRequest>(res);
        }
      #region private members
        private Mock<CICDService> getCICDServiceMock()
        {
          return new Mock<CICDService>(_jobRepo.Object, _buildService.Object, _requestFactory.Object);
        }
        private CICDService getCICDService()
        {
            return new CICDService(_jobRepo.Object, _buildService.Object, _requestFactory.Object);
        }
      #endregion
    }
}
