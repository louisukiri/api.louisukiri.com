using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.services.domain;
using cicdDomain;
using Newtonsoft.Json;
using NUnit.Framework;
using Octokit;
using Moq;

namespace api.louisukiri.com.Tests.service
{
    [TestFixture]
    public class VcSettingsServiceTest
    {
        public Mock<VcSettingsService> SutMock(IVersionControlServerRepo repo=null)
        {
            Mock<VcSettingsService> mock;
            if(repo==null)
                mock    = new Mock<VcSettingsService>();
            else
            {
                mock = new Mock<VcSettingsService>(repo);
            }
            mock.Setup(z => z.DoPullRequest(
                It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<NewCommitStatus>()
                , It.IsAny<string>()
                ));
            return mock;
        }
        [Test]
        public void SetPullRequestGivenEmptyBranchSetBranchToMaster()
        {
            var mock = new Mock<VcSettingsService>();
            mock.Setup(z => z.DoPullRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NewCommitStatus>(), It.IsAny<string>()));

            mock.Object.SetPullRequest(testInfrastructure.BrnValidBranch, getStatus());
            mock.Verify(z => z.DoPullRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), "master", It.IsAny<NewCommitStatus>(), It.IsAny<string>()));
        }
        [Test]
        public void SetPullRequestGivenErrorFromDoPullRequestReturnFalse()
        {
            var mock = new Mock<VcSettingsService>();
            mock.Setup(z => z.DoPullRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NewCommitStatus>(), It.IsAny<string>()))
                .Returns((string a, string b, string c, string d, NewCommitStatus e, string f) =>
                {
                    throw new Exception("err out");
                })
                ;

            var result = mock.Object.SetPullRequest(testInfrastructure.BrnValidBranch, getStatus());
            Assert.IsFalse(result);
        }
        [Test]
        public void SetPullRequestGivenValidInputsAlwaysDoPullRequest()
        {
            var mock = SutMock();
            mock.Object.SetPullRequest(testInfrastructure.BrnValidBranch, getStatus());
            mock.Verify(z => z.DoPullRequest(
                  It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<NewCommitStatus>()
                , It.IsAny<string>()
                ));
        }
        [Test]
        public void SetPullRequestGivenValidInputsReturnTrue()
        {
            var mock = SutMock();
            var result = mock.Object.SetPullRequest(testInfrastructure.BrnValidBranch, getStatus());

            Assert.IsTrue(result);
        }
        [Test]
        public void GetSettingsGivenJsonSettingsStringReturnExpectedValues()
        {
            var vcRepo = new Mock<IVersionControlServerRepo>();
            vcRepo.Setup(z => z.FileContent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<Action<RepositoryContent>>()
                ))
                .Returns(Task.FromResult<string>(testInfrastructure.BrnValidBranchString))
                ;
            var mock = new Mock<VcSettingsService>(vcRepo.Object);
            mock.CallBase = true;
            var sut = mock.Object;
            
            var server = new VerControlServer(testInfrastructure.RepoUriUrl, "fakeauth");
            var result = sut.GetBranchSettings(testInfrastructure.BrnValidBranch)
                ;

            Assert.AreEqual("test2 val",result.Branch.BaseBranch);
        }
        [Test]
        public void GetSettingsGivenNullSettingsStringReturnNull()
        {
            var vcRepo = new Mock<IVersionControlServerRepo>();
            vcRepo.Setup(z => z.FileContent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<Action<RepositoryContent>>()
                ))
                .Returns(Task.FromResult<string>(null))
                ;
            var sut = new Mock<VcSettingsService>(vcRepo.Object).Object;
            var server = new VerControlServer(testInfrastructure.RepoUriUrl, "fakeauth");
            var result = sut.GetBranchSettings(testInfrastructure.BrnValidBranch)
                ;

            Assert.IsNull(result);
        }
        [Test]
        public void GetSettingsGivenEmptySettingsStringReturnNull()
        {
            var vcRepo = new Mock<IVersionControlServerRepo>();
            vcRepo.Setup(z => z.FileContent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<Action<RepositoryContent>>()
                ))
                .Returns(Task.FromResult(string.Empty))
                ;
            var sut = new Mock<VcSettingsService>(vcRepo.Object).Object;
            var server = new VerControlServer(testInfrastructure.RepoUriUrl, "fakeauth");
            var result = sut.GetBranchSettings(testInfrastructure.BrnValidBranch)
                ;

            Assert.IsNull(result);
        }
        [Test]
        public void GetSettingsGivenInvalidJsonSettingsStringReturnNull()
        {
            var vcRepo = new Mock<IVersionControlServerRepo>();
            vcRepo.Setup(z => z.FileContent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<Action<RepositoryContent>>()
                ))
                .Returns(Task.FromResult("this is not a json ``` string"))
                ;
            var sut = new Mock<VcSettingsService>(vcRepo.Object).Object;
            var server = new VerControlServer(testInfrastructure.RepoUriUrl, "fakeauth");
            var result = sut.GetBranchSettings(testInfrastructure.BrnValidBranch);

            Assert.IsNull(result);
        }
        [Test]
        public void GetSettingsGivenUnexpectedBranchNameUpdatesBranchName()
        {
            var vcRepo = new Mock<IVersionControlServerRepo>();
            vcRepo.Setup(z => z.FileContent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<Action<RepositoryContent>>()
                ))
                .Returns(Task.FromResult<string>(testInfrastructure.BrnValidBranchString))
                ;
            vcRepo.Setup(z => z.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("{'test':'test'}")
                ;
            var mock = new Mock<VcSettingsService>(vcRepo.Object);
            mock.CallBase = true;
            var sut = mock.Object;

            var activity = testInfrastructure.GitHubPushActivity;
            activity.SetName("testName");
            var branch = cicd.domain.context.trigger.entity.Branch.CreateFrom(activity);

            var result = sut.GetBranchSettings(branch)
                ;

            Assert.AreEqual("testName", result.Branch.Name);
        }
        [Test]
        public void GetSettingsGivenUnexpectedBranchNameUpdatesBaseBranch()
        {
            var vcRepo = new Mock<IVersionControlServerRepo>();
            vcRepo.Setup(z => z.FileContent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<Action<RepositoryContent>>()
                ))
                .Returns(Task.FromResult<string>(testInfrastructure.BrnValidBranchString))
                ;
            vcRepo.Setup(z => z.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("{'test':'test'}")
                ;
            var mock = new Mock<VcSettingsService>(vcRepo.Object);
            mock.CallBase = true;
            var sut = mock.Object;

            var activity = testInfrastructure.GitHubPushActivity;
            activity.SetName("testName");
            var branch = cicd.domain.context.trigger.entity.Branch.CreateFrom(activity);

            var result = sut.GetBranchSettings(branch)
                ;

            Assert.AreEqual("master", result.Branch.BaseBranch);
        }
        [Test]
        public void GetSettingsGivenUnexpectedBranchNameUpdatesPath()
        {
            var vcRepo = new Mock<IVersionControlServerRepo>();
            vcRepo.Setup(z => z.FileContent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<Action<RepositoryContent>>()
                ))
                .Returns(Task.FromResult<string>(testInfrastructure.BrnValidBranchString))
                ;
            vcRepo.Setup(z => z.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("{'test':'test'}")
                ;
            var mock = new Mock<VcSettingsService>(vcRepo.Object);
            mock.CallBase = true;
            var sut = mock.Object;

            var activity = testInfrastructure.GitHubPushActivity;
            activity.SetName("testName");
            var branch = cicd.domain.context.trigger.entity.Branch.CreateFrom(activity);

            var result = sut.GetBranchSettings(branch)
                ;

            Assert.AreEqual("master/testName", result.Branch.Path);
        }
        [Test]
        public void GetSettingsGivenUnexpectedBranchNameUpdatesSettingsFile()
        {
            var vcRepo = new Mock<IVersionControlServerRepo>();
            vcRepo.Setup(z => z.FileContent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<Action<RepositoryContent>>()
                ))
                .Returns(Task.FromResult<string>(testInfrastructure.BrnValidBranchString))
                ;
            vcRepo.Setup(z => z.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var mock = new Mock<VcSettingsService>(vcRepo.Object);
            mock.CallBase = true;
            var sut = mock.Object;

            var activity = testInfrastructure.GitHubPushActivity;
            activity.SetName("testName");
            var branch = cicd.domain.context.trigger.entity.Branch.CreateFrom(activity);

            var result = sut.GetBranchSettings(branch)
                ;

            vcRepo.Verify(z => z.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

        }
        [Test]
        public void CreatingSettingsGivenInvalidJsonThrows()
        {
            var vcRepo = new Mock<IVersionControlServerRepo>();
            vcRepo.Setup(
                z =>
                    z.Insert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var sut = getSut(repo: vcRepo.Object);
            Assert.Throws<JsonReaderException>(() => sut.CreateSettingsFile(getVcs(), "branch", "non json content"));
        }
        [Test]
        public void CreatingSettingsGivenValidJsonReturnsExpectedDictionary()
        {
            var vcRepo = new Mock<IVersionControlServerRepo>();
            vcRepo.Setup(
                z =>
                    z.Insert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(Task.Run(() => { }))
                        ;

            var sut = getSut(repo: vcRepo.Object);
            var result = sut.CreateSettingsFile(getVcs(), "branch", "{'Branch':{'Level':'Test','BaseBranch':'Test'}}");

            Assert.AreEqual("Test", result.Branch.Level );
        }

        [Test]
        public async Task UpdatingSettingsGivenBranchWithSettingsUpdatesRepo()
        {
            var vcRepo = new Mock<IVersionControlServerRepo>();
            vcRepo.Setup(
                z =>
                    z.Insert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            vcRepo.Setup(
                z =>
                    z.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                        //.Verifiable()
                        ;

            var mock = SutMock(repo: vcRepo.Object);
            mock.CallBase = true;
            mock.Setup(
                z =>
                    z.GetSettingsFile(It.IsAny<cicd.domain.context.trigger.entity.Branch>(),
                        It.IsAny<Action<RepositoryContent>>()))
                .Returns(
                    (cicd.domain.context.trigger.entity.Branch b, Action<RepositoryContent> c) =>
                    {
                        if(c!=null)
                            c(new RepositoryContent("","","testSha", 1, ContentType.File, null, null, null, null, "","","",null));
                        return Task.FromResult(testInfrastructure.StrValidSettingsJson);
                    });


            await Task.Run(() =>
            {
                mock.Object.UpdateBranchSetting(testInfrastructure.BrnValidBranch, testInfrastructure.StgValidSettings);
                vcRepo.Verify(
                z =>
                    z.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            });

        }
        [Test]
        public async Task UpdatingSettingsGivenBranchWithoutSettingsCallRepoInsert()
        {
            var vcRepo = new Mock<IVersionControlServerRepo>();
            vcRepo.Setup(
                z =>
                    z.Insert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            vcRepo.Setup(
                z =>
                    z.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                //.Verifiable()
                        ;

            var mock = SutMock(repo: vcRepo.Object);
            mock.CallBase = true;
            mock.Setup(
                z =>
                    z.GetSettingsFile(It.IsAny<cicd.domain.context.trigger.entity.Branch>(),
                        It.IsAny<Action<RepositoryContent>>()))
                .Returns(
                    (cicd.domain.context.trigger.entity.Branch b, Action<RepositoryContent> c) =>
                    {
                        if (c != null)
                            c(new RepositoryContent("", "", "", 1, ContentType.File, null, null, null, null, "", "", "", null));
                        return Task.FromResult<string>(null);
                    });


            await Task.Run(() =>
            {
                mock.Object.UpdateBranchSetting(testInfrastructure.BrnValidBranch, testInfrastructure.StgValidSettings);
                vcRepo.Verify(
                z => z.Insert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                        ;

            });
        }
        [Test]
        public async Task UpdatingSettingsGivenBranchWithSettingsReturnsExpectedSettings()
        {
            var vcRepo = new Mock<IVersionControlServerRepo>();
            vcRepo.Setup(
                z =>
                    z.Insert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            vcRepo.Setup(
                z =>
                    z.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                //.Verifiable()
                        ;

            var mock = SutMock(repo: vcRepo.Object);
            mock.CallBase = true;
            mock.Setup(
                z =>
                    z.GetSettingsFile(It.IsAny<cicd.domain.context.trigger.entity.Branch>(),
                        It.IsAny<Action<RepositoryContent>>()))
                .Returns(
                    (cicd.domain.context.trigger.entity.Branch b, Action<RepositoryContent> c) =>
                    {
                        if (c != null)
                            c(new RepositoryContent("", "", "testSha", 1, ContentType.File, null, null, null, null, "", "", "", null));
                        return Task.FromResult(testInfrastructure.StrValidSettingsJson);
                    });


            await Task.Run(() =>
            {
                mock.Object.UpdateBranchSetting(testInfrastructure.BrnValidBranch, testInfrastructure.StgValidSettings);
                vcRepo.Verify(
                z =>
                    z.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            });

        }
        [Test]
        public void CtorCanAcceptCommunicatorBotForCommunication()
        {
            var commBot = new Mock<IBot>();
            var vcRepo = new Mock<IVersionControlServerRepo>();
            var vcStgMock = new Mock<VcSettingsService>(vcRepo.Object,commBot.Object);

            Assert.IsNotNull(vcStgMock.Object.CommBot);
        }
        #region private methods
        private VcSettingsService getSut(IVersionControlServerRepo repo=null)
        {
            if (repo == null)
            {
                return new VcSettingsService();
            }
            return new VcSettingsService(repo);
        }
        private VerControlServer getVcs(string id="https://test.com/user/testid.git", string token="testtoken")
        {
            return new VerControlServer(id, token);
        }
        private NewCommitStatus getStatus()
        {
            return getStatus(CommitState.Success);
        }
        private NewCommitStatus getStatus(CommitState state)
        {
            return new NewCommitStatus { State=state };
        }
        #endregion
    }
    public class VcSettingsServiceGettingDefaultSettings
    {
        public Mock<VcSettingsService> SutMock()
        {
            var mock = new Mock<VcSettingsService>();
            mock.Setup(z => z.DoPullRequest(
                It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<string>()
                , It.IsAny<NewCommitStatus>()
                , It.IsAny<string>()
                ));
            return mock;
        }
        [Test]
        public void GivenNullBranchReturnEmptyString()
        {
            var mock = SutMock();
            var result = mock.Object.GetDefaultSettingsFor(null);

            Assert.IsNullOrEmpty(result);
        }
        [Test]
        public void GivenValidBranchReturnJsonStringWithExpectedBaseBranch()
        {
            var activity = testInfrastructure.GitHubPushActivity;
            activity.SetBase("test");
            var branch = cicd.domain.context.trigger.entity.Branch.CreateFrom(activity);
            var mock = SutMock();
            var result = mock.Object.GetDefaultSettingsFor(branch);
            var settings = JsonConvert.DeserializeObject<Settings>(result);

            Assert.AreEqual("test", settings.Branch.BaseBranch);
        }
        [Test]
        public void GivenValidBranchDefaultContainsJobDefinition()
        {
            var activity = testInfrastructure.GitHubPushActivity;
            activity.SetBase("test");
            var branch = cicd.domain.context.trigger.entity.Branch.CreateFrom(activity);
            var mock = SutMock();
            var result = mock.Object.GetDefaultSettingsFor(branch);
            var settings = JsonConvert.DeserializeObject<Settings>(result);

            Assert.IsNotNull(settings.Jobs);
        }
        [Test]
        public void GivenValidBranchReturnExpectedBranchName()
        {
            var activity = testInfrastructure.GitHubPushActivity;
            activity.SetName("TestName");
            activity.SetBase("TestBaseBranchName");
            var branch = cicd.domain.context.trigger.entity.Branch.CreateFrom(activity);
            var mock = SutMock();
            var result = mock.Object.GetDefaultSettingsFor(branch);
            var settings = JsonConvert.DeserializeObject<Settings>(result);

            Assert.AreEqual("TestName", settings.Branch.Name);
        }
        [Test]
        public void GivenValidBranchReturnExpectedBranchPath()
        {
            var activity = testInfrastructure.GitHubPushActivity;
            activity.SetName("TestName");
            activity.SetBase("TestBaseBranchName");
            var branch = cicd.domain.context.trigger.entity.Branch.CreateFrom(activity);
            var mock = SutMock();
            var result = mock.Object.GetDefaultSettingsFor(branch);
            var settings = JsonConvert.DeserializeObject<Settings>(result);

            Assert.AreEqual("TestBaseBranchName/TestName", settings.Branch.Path);
        }
        [Test]
        public void GivenBranchWithParentMasterReturnEnvVersion()
        {
            var activity = testInfrastructure.GitHubPushActivity;
            activity.SetBase("master");
            var branch = cicd.domain.context.trigger.entity.Branch.CreateFrom(activity);
            var mock = SutMock();
            var result = mock.Object.GetDefaultSettingsFor(branch);
            var settings = JsonConvert.DeserializeObject<Settings>(result);

            Assert.AreEqual("Version", settings.Branch.Level);
        }
        [Test]
        public void GivenBranchWithParentNotMasterReturnEnvOther()
        {
            var activity = testInfrastructure.GitHubPushActivity;
            activity.SetBase("other");
            var branch = cicd.domain.context.trigger.entity.Branch.CreateFrom(activity);
            var mock = SutMock();
            var result = mock.Object.GetDefaultSettingsFor(branch);
            var settings = JsonConvert.DeserializeObject<Settings>(result);

            Assert.AreEqual("other", settings.Branch.Level);
        }
        [TestFixture]
        public class SettingsParameters
        {
            private cicd.domain.context.trigger.entity.Branch branch;
            public Mock<VcSettingsService> SutMock()
            {
                var mock = new Mock<VcSettingsService>();
                mock.Setup(z => z.DoPullRequest(
                    It.IsAny<string>()
                    , It.IsAny<string>()
                    , It.IsAny<string>()
                    , It.IsAny<string>()
                    , It.IsAny<NewCommitStatus>()
                    , It.IsAny<string>()
                    ));
                return mock;
            }
            private Settings settings;
            [SetUp]
            public void Setup()
            {
                branch = testInfrastructure.BrnValidBranch;
                var mock = SutMock();
                var result = mock.Object.GetDefaultSettingsFor(branch);
                settings = JsonConvert.DeserializeObject<Settings>(result);
            }

            [Test]
            public void ContainsComputerName()
            {
                Assert.IsTrue(settings.Jobs.First().Parameters.Any(z => z.Key == "COMPUTERNAME"));
            }
            [Test]
            public void ContainsDeployFrom()
            {
                Assert.IsTrue(settings.Jobs.First().Parameters.Any(z => z.Key == "DEPLOYFROM"));
            }

            public IEnumerable<TestCaseData> ExpectedParamsSource
            {
                get
                {
                    branch = testInfrastructure.BrnValidBranch;
                    yield return new TestCaseData("GITURL", branch.Server.GitSshUrlString, branch).SetName("ContainsExpectedGitUrl");
                    yield return new TestCaseData("GITBRANCH", branch.Name, branch).SetName("ContainsExpectedGitBranch");
                    yield return new TestCaseData("PROJECTNAME", branch.Name, branch).SetName("ContainsExpectedProjectName");
                }
            }
            
            [TestCaseSource("ExpectedParamsSource")]
            public void ContainsExpectedValuesFromParams(string key, string expected, cicd.domain.context.trigger.entity.Branch branch)
            {
                var mock = SutMock();
                var result = mock.Object.GetDefaultSettingsFor(branch);
                settings = JsonConvert.DeserializeObject<Settings>(result);
                Assert.AreEqual(expected, settings.Jobs.First().Parameters.First(z=> z.Key==key).Value);
            }
            [Test]
            public void ContainsProjectDirectory()
            {
                Assert.IsTrue(settings.Jobs.First().Parameters.Any(z => z.Key == "PROJECTDIRECTORY"));
            }
            [Test]
            public void ContainsNunitFileName()
            {
                Assert.IsTrue(settings.Jobs.First().Parameters.Any(z => z.Key == "NUNITFILENAME"));
            }
            [Test]
            public void ContainsOpenCoverReport()
            {
                Assert.IsTrue(settings.Jobs.First().Parameters.Any(z => z.Key == "OPENCOVERREPORT"));
            }
            [Test]
            public void ContainsTestPathDir()
            {
                Assert.IsTrue(settings.Jobs.First().Parameters.Any(z => z.Key == "TESTPATHDIR"));
            }
            [Test]
            public void ContainsReportPathDir()
            {
                Assert.IsTrue(settings.Jobs.First().Parameters.Any(z => z.Key == "REPORTPATHDIR"));
            }
            [Test]
            public void ContainsSolutionPath()
            {
                Assert.IsTrue(settings.Jobs.First().Parameters.Any(z => z.Key == "SOLUTIONPATH"));
            }
            [Test]
            public void ContainsSiteName()
            {
                Assert.IsTrue(settings.Jobs.First().Parameters.Any(z => z.Key == "SITENAME"));
            }
            [Test]
            public void ContainsSitePath()
            {
                Assert.IsTrue(settings.Jobs.First().Parameters.Any(z => z.Key == "SITEPATH"));
            }
            [Test]
            public void ContainsReportApi()
            {
                Assert.IsTrue(settings.Jobs.First().Parameters.Any(z => z.Key == "REPORTAPI"));
            }
        }
    }
}
