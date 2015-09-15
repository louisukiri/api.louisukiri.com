using System;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.services;
using cicd.infrastructure;
using cicd.infrastructure.dtos;
using cicdDomain;
using Moq;
using NUnit.Framework;
using Octokit;
using System.Collections.Generic;

namespace api.louisukiri.com.Tests.service
{
    [TestFixture]
    public partial class cicdApplicationserviceTest : cicdAppServiceTestBase
    {
        [SetUp]
        public void _setup()
        {
            base.setup();
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
        public void CicdApplicationServiceRequiresVcAppService()
        {
            CICDService sut = getCICDService();
            Assert.IsNotNull(sut.VersionControlService);
        }
        [Test]
        public void CicdApplicationServiceRequiresCommunicationBot()
        {
            CICDService sut = getCICDService();
            Assert.IsNotNull(sut.CommBot);
        }
        [Test]
        public void WhenTriggerAndIsPushActivityCallBuildPushForEachJob()
        {
            base.setup();
            _buildService.Setup(z => z.Build(It.IsAny<Job>(), It.IsAny<cicd.domain.context.trigger.entity.Branch>()));
            IList<Job> listOfJobs = RandomNumberOfPushJobs();
            _versionControlService.Setup(z => z.GetSettings(It.IsAny<cicd.domain.context.trigger.entity.Branch>()))
                .Returns(new Settings()
                {
                    Jobs= listOfJobs
                        
                });
            //request = new RequestPayload(RequestTrigger.Push, testInfrastructure.GitHubPushContent);
            CICDService sut = getCICDService();
            var testRequest =  request;
            testRequest.Branch =
                cicd.domain.context.trigger.entity.Branch.CreateFrom(testInfrastructure.GitHubPushActivity);
            var res = sut.trigger(testRequest);

            _buildService.Verify(z => z.Build(It.IsAny<Job>(), It.IsAny<cicd.domain.context.trigger.entity.Branch>()), Times.Exactly(listOfJobs.Count));
        }

        private IList<Job> RandomNumberOfPushJobs()
        {
            List<Job> jobs = new List<Job>();
            int rand = new Random().Next(2,10);
            for (int i = 0; i <= rand; i++)
            {
                jobs.Add(new Job(){Level = BranchLevel.Unknown, Trigger = RequestTrigger.Push});
            }
            return jobs;
        }
        [Test]
        public void WhenTriggerGetSettings()
        {
            base.setup();
            _buildService.Setup(z => z.Build(It.IsAny<Job>(), It.IsAny<cicd.domain.context.trigger.entity.Branch>()));
            int jobs = SetupGetSettingsWithRandomNumberOfJobs();

            DoSutTrigger();

            _versionControlService.Verify(z => z.GetSettings(It.IsAny<cicd.domain.context.trigger.entity.Branch>()), Times.Once());
        }
        [Test]
        public void WhenTriggerAndIsPullActivityCallBuildTests()
        {
            _buildService.Setup(z => z.Build(It.IsAny<Job>(), It.IsAny<cicd.domain.context.trigger.entity.Branch>()));
            int jobs = SetupGetSettingsWithRandomNumberOfJobs();

            //request = new RequestPayload(RequestTrigger.Push, testInfrastructure.GitHubPushContent);

            DoSutTrigger(testInfrastructure.GitHubPullRequest);

            _buildService.Verify(z => z.Build(It.IsAny<Job>(), It.IsAny<cicd.domain.context.trigger.entity.Branch>()), Times.Exactly(jobs));
        }

        private void DoSutTrigger(GitHubPullRequest pull_request = null)
        {
            var sut = getCICDService();
            var testRequest = request;
            testRequest.Branch =
                cicd.domain.context.trigger.entity.Branch.CreateFrom(testInfrastructure.GitHubPushActivity);
            testRequest.Activity.pull_request = pull_request;
            var res = sut.trigger(testRequest);
        }

        private int SetupGetSettingsWithRandomNumberOfJobs()
        {
            IList<Job> listOfJobs = RandomNumberOfPushJobs();
            _versionControlService.Setup(z => z.GetSettings(It.IsAny<cicd.domain.context.trigger.entity.Branch>()))
                .Returns(new Settings()
                {
                    Jobs = listOfJobs
                });
            return listOfJobs.Count;
        }

        [Test]
        public void WhenTriggerAndIsPullActivitySetPullRequestState()
        {
            base.setup();
            int listOfJobsCount = SetupGetSettingsWithRandomNumberOfJobs();
            _versionControlService.Setup(z => z.SetPullRequestStatus(It.IsAny<cicd.domain.context.trigger.entity.Branch>(), It.IsAny<NewCommitStatus>()));

            //request = new RequestPayload(RequestTrigger.Push, testInfrastructure.GitHubPushContent);
            DoSutTrigger(testInfrastructure.GitHubPullRequest);

            _versionControlService.Verify(z => z.SetPullRequestStatus(It.IsAny<cicd.domain.context.trigger.entity.Branch>(), It.IsAny<NewCommitStatus>()), Times.Exactly(listOfJobsCount));
        }
        [Test]
        public void WhenTriggerAndRequestIsNullReturnJobWithException()
        {
          CICDService sut = getCICDService();
          var res = sut.trigger(null);

          Assert.IsFalse(res.LastExecution.IsSuccessful);
          Assert.IsTrue(res.LastExecution.Messages.Count > 0);
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
          Mock<CICDService> mockSut = getCICDServiceMock();
          mockSut.Setup(z => z.trigger(It.IsAny<RequestPayload>()))
            .Returns(testInfrastructure.getJob(true));
          CICDService sut = mockSut.Object;

          var res = sut.run(testInfrastructure.getRequestPayload());
          Assert.IsInstanceOf<SuccessfulRequest>(res);
        }

      #region private members

      #endregion
    }
    #region cicdAppServiceTestBase

    public class cicdAppServiceTestBase
    {
        protected Mock<IJobRepo> _jobRepo;
        protected Mock<IBuildService> _buildService;
        protected Mock<IVcActionService> _vcService;
        protected Mock<IVersionControlService> _versionControlService;
        protected Mock<IVcSettingsService> _vcSetting;
        protected Mock<IBot> BotMock;

        public RequestPayload request
        {
            get { return new RequestPayload(RequestTrigger.Push, testInfrastructure.GitHubPushContent); }
        }

        public TestdataPayload testdata
        {
            get { return new TestdataPayload(testInfrastructure.TestdataContent);}
        }

        public void setup()
        {
            _jobRepo = new Mock<IJobRepo>();
            _buildService = new Mock<IBuildService>();
            _vcService = new Mock<IVcActionService>();
            _versionControlService = new Mock<IVersionControlService>();
            _vcSetting = new Mock<IVcSettingsService>();
            BotMock = new Mock<IBot>();
        }
        protected Mock<CICDService> getCICDServiceMock()
        {
            return new Mock<CICDService>(_jobRepo.Object, _buildService.Object, _versionControlService.Object, BotMock.Object);
        }
        protected CICDService getCICDService()
        {
            return getCICDService(_jobRepo.Object, _buildService.Object,
                _versionControlService.Object, BotMock.Object);
        }
        protected CICDService getCICDService(IJobRepo jobRepo,IBuildService buildService, IVersionControlService versionControllerService, IBot comBot)
        {
            return new CICDService(jobRepo, buildService, versionControllerService, comBot);
        }
    }
#endregion
}
