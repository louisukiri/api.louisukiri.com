using System.Collections.Generic;
using System.Linq;
using System.Net;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.services;
using cicd.domain.context.trigger.services.domain;
using cicdDomain;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;
using NUnit.Framework.Constraints;


namespace api.louisukiri.com.Tests.entity
{
  [TestFixture]
  public class JenkinsBuildServiceTest
  {
    public Mock<JenkinsBuildService> _sut;
      private Mock<IBot> _bot;

    public JenkinsBuildService sut
    {
      get { return _sut.Object; }
    }
    [SetUp]
    public void setup()
    {
        _bot = new Mock<IBot>();
        _sut = new Mock<JenkinsBuildService>(_bot.Object);
    }
    [Test]
    public void ExecutionsIsInitializedToEmptyCollectino()
    {
        Job testJob = new Job();
        Assert.IsNotNull(testJob.Executions);
    }
    [Test]
    public void CtorAcceptsCommunicationBotForCommunication()
    {
        var commBot = new Mock<IBot>();
        var sutMock = new Mock<JenkinsBuildService>(commBot.Object);

        Assert.IsNotNull(sutMock.Object.CommBot);
    }
    #region Push Tests
    [Test]
    public void BuildPushAndTriggerReturns200AddSuccessfulLastExecution()
    {
        Job testJob = new Job();
        SetTriggerMethod();

        var res = sut.Build(testJob, testInfrastructure.BrnValidBranch);
        Assert.IsTrue(res.Executions.Count > 0);
        Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void BuildPushForDevBranchCallsJobPath()
    {
        Job testJob = new Job(){Path="jobs/test"};
        string staticDevPath = testJob.Path;
        SetTriggerMethod();

        var res = sut.Build(testJob, testInfrastructure.BrnValidBranch);

        _sut.Verify(z=> z.trigger(It.IsAny<string>(), It.IsAny<string>(), staticDevPath, It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>()));
    }
    [Test]
    public void BuildPushAndTriggerReturnsNon200AddFailedLastExecution()
    {
        Job testJob = new Job();
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>()))
            .Returns(
            new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.NotFound
            }
            );
        int Executions = testJob.Executions.Count;
        var res = sut.Build(testJob, testInfrastructure.BrnValidBranch);
        Assert.AreEqual(Executions + 1, testJob.Executions.Count);
        Assert.IsFalse(res.SuccesffullyRan);
    }
    [Test]
    public void BuildPushGivenJobWithEmptyGitUrlParameterAddUrlParameterFromBranch()
    {
        Job testJob = new Job();
        testJob.Parameters.Add(new KeyValuePair<string, string>("GITBRANCH", ""));
        SetTriggerMethodWithBadRequest("GITBRANCH");
        var res = sut.Build(testJob, testInfrastructure.BrnValidBranch);

        Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void EscapeBackSlashInAnyParameters()
    {
        Job testJob = new Job();
        testJob.Parameters.Add(new KeyValuePair<string, string>("TestParam", "\\a\\b"));
        testJob.Parameters.Add(new KeyValuePair<string, string>("TestParam2", "\\\\ab\\cdef"));

        var res = sut.Build(testJob, testInfrastructure.BrnValidBranch);
        Assert.AreEqual("\\\\\\\\a\\\\\\\\b", testJob.Parameters.First(z => z.Key == "TestParam").Value);
        Assert.AreEqual("\\\\\\\\\\\\\\\\ab\\\\\\\\cdef", testJob.Parameters.First(z => z.Key == "TestParam2").Value);
    }
    [Test]
    public void BuildPushExceptionThrownInTriggerAddFailedLastExecution()
    {
        Job testJob = new Job();
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>()))
            .Returns((string a) =>
            {
                throw new Exception("error");
            }
            );
        int Executions = testJob.Executions.Count;
        var res = sut.Build(testJob, testInfrastructure.BrnValidBranch);
        Assert.AreEqual(Executions + 1, testJob.Executions.Count);
        Assert.IsFalse(res.SuccesffullyRan);
    }
    [Test]
    public void BuildPushNeverAddJobNameToParameters()
    {
        Job testJob = new Job();
        testJob.Name = "test-job";
        var res = sut.Build(testJob, testInfrastructure.BrnValidBranch);

        Assert.IsFalse(testJob.Parameters.Any(z => z.Key == "JobName"));
    }
    [Test]
    public void BuildSendsEventToCommBotWhenAvailable()
    {
        var testJob = new Job{ Name = "test-job"};
        var combotMock = new Mock<IBot>();
        combotMock.Setup(z => z.Trigger(It.IsAny<IEvent>()));
        var SutMock = new Mock<JenkinsBuildService>(combotMock.Object);
        SutMock.Setup(
            z =>
                z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>()))
            .Returns(new HttpResponseMessage{StatusCode = HttpStatusCode.OK});

        var Sut = SutMock.Object;
        var res = Sut.Build(testJob, testInfrastructure.BrnValidBranch);

        combotMock.Verify(z=> z.Trigger(It.IsAny<IEvent>()));
    }
#endregion
    #region private methods
    private void SetTriggerMethod(string path = "")
    {
        _sut = new Mock<JenkinsBuildService>(_bot.Object);
        if (string.IsNullOrWhiteSpace(path))
            _sut.Setup(
                z =>
                    z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                            It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>()))
                .Returns(
                    new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    }
                );
        else
        {
            _sut.Setup(
            z =>
                z.trigger(It.IsAny<string>(), It.IsAny<string>(), path,
                        It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>()))
            .Returns(
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                }
            );
        }
    }
    private string env = string.Empty;
    private void SetTriggerMethodWithBadRequest(string EnvKey = "")
    {
        env = "";
        _sut = new Mock<JenkinsBuildService>(_bot.Object);
        _sut.Setup(
            z =>
                z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>()))
            .Returns((string a, string b, string c, List<KeyValuePair<string, string>> d, string e) =>
            {
                HttpResponseMessage msg = null;
                var exists = d.Any(z => z.Key == EnvKey && !string.IsNullOrWhiteSpace(z.Value));
                if (exists)
                {
                    msg = new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                    env = d[0].Value;
                }
                return msg ?? new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }
            );
    }
    #endregion
  }
}
