using System.Collections.Generic;
using System.Linq;
using cicdDomain;
using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.entity;
using cicdDomain.cicd.domain.service;
using cicdDomain.cicd.infrastructure;
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

    public JenkinsBuildService sut
    {
      get { return _sut.Object; }
    }

    private string env;
    [TestFixtureSetUp]
    public void setup()
    {
        _sut = new Mock<JenkinsBuildService>();
        env = "";
        //setTriggerMethodWithBadRequest();
    }
    [Test]
    public void ExecutionsIsInitializedToEmptyCollectino()
    {
        Job testJob = new Job();
        Assert.IsNotNull(testJob.Executions);
    }
    [Test]
    public void BuildSeedAndTriggerReturns200AddSuccessfulLastExecution()
    {
        Job testJob = new Job();
        SetTriggerMethod();

        var res = sut.buildSeed(testJob, new pushactivity());
        Assert.IsTrue(res.Executions.Count > 0);
        Assert.IsTrue(res.SuccesffullyRan);
    }

      [Test]
    public void BuildSeedAndTriggerReturnsNon200AddFailedLastExecution()
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
      var res = sut.buildSeed(testJob, new pushactivity());
      Assert.AreEqual(Executions + 1, testJob.Executions.Count);
      Assert.IsFalse(res.SuccesffullyRan);
    }
    [Test]
    public void BuildSeedGivenJobWithEmptyGitUrlParameterAddUrlParameterFromActivity()
    {
      pushactivity req = new pushactivity { repository = new SourceControlRepository { clone_url = "http://test.foo"} };
      Job testJob = new Job();
      testJob.parameters.Add(new KeyValuePair<string, string>("GitUrl", ""));
      SetTriggerMethodWithBadRequest("GitUrl");
      var res = sut.buildSeed(testJob, req);

      Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void BuildSeedGivenJobWithEmptyEnvParamAndIsStagingAddStagEnvParam()
    {
        string paramKey = "Environment";
        pushactivity req = new pushactivity { repository = new SourceControlRepository { url = "http://test.foo", master_branch = "master" }, base_ref = "ref/heads/master" };
        Job testJob = new Job();
        testJob.parameters.Add(new KeyValuePair<string, string>(paramKey, ""));
        SetTriggerMethodWithBadRequest(paramKey);
        var res = sut.buildSeed(testJob, req);
        Assert.IsTrue(res.SuccesffullyRan);
        Assert.AreEqual("staging", env);
    }
    [Test]
    public void BuildSeedForDevJobAddsJobToRepo()
    {
        string paramKey = "Environment";
        pushactivity req = testInfrastructure.GetMasterBranchActivity;
        Job testJob = new Job();
        testJob.parameters.Add(new KeyValuePair<string, string>(paramKey, ""));
        SetTriggerMethodWithBadRequest(paramKey);
        var res = sut.buildSeed(testJob, req);
        Assert.IsTrue(res.SuccesffullyRan);
        Assert.AreEqual("staging", env);
    }
    [Test]
    public void BuildSeedGivenJobWithEmptyEnvParamAndIsStagingAddDevEnvParam()
    {
      string paramKey = "Environment";
      pushactivity req = new pushactivity { repository = new SourceControlRepository { url = "http://test.foo", master_branch = "master" }, base_ref = "ref/heads/master2" };
      Job testJob = new Job();
      testJob.parameters.Add(new KeyValuePair<string, string>(paramKey, ""));
      SetTriggerMethodWithBadRequest(paramKey);
      var res = sut.buildSeed(testJob, req);
      Assert.IsTrue(res.SuccesffullyRan);
      Assert.AreEqual("development", env);
    }
    [Test]
    public void BuildSeedGivenJobWithEmptyBranchNameParameterAddBranchParameterFromJob()
    {
      pushactivity activity = new pushactivity {@ref = "ref/heads/test"};
      Job testJob = new Job();
      testJob.parameters.Add(new KeyValuePair<string, string>("BranchName", ""));
      SetTriggerMethodWithBadRequest("BranchName");
      var res = sut.buildSeed(testJob, activity);

      Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void BuildSeedExceptionThrownInTriggerAddFailedLastExecution()
    {
        Job testJob = new Job();
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>()))
            .Returns((string a) =>
            {
                throw new Exception("error");
            }
            );
        int Executions = testJob.Executions.Count;
        var res = sut.buildSeed(testJob, new pushactivity());
        Assert.AreEqual(Executions + 1, testJob.Executions.Count);
        Assert.IsFalse(res.SuccesffullyRan);
    }
    [Test]
    public void BuildSeedAlwaysAddJobNameToParameters()
    {
        Job testJob = new Job();
        testJob.name = "test-job";
        var activity = new pushactivity {created = true, pusher = new VersionControlUser()};
        var res = sut.buildSeed(testJob, activity);

        Assert.IsTrue(testJob.parameters.Any(z=> z.Key=="JobName"));
    }
    [Test]
    public void BuildSeedAlwaysAddsDevJobNameToParameters()
    {
        Job testJob = new Job();
        testJob.name = "test-job";
        var activity = new pushactivity { created = true, pusher = new VersionControlUser() };
        var res = sut.buildSeed(testJob, activity);

        Assert.IsTrue(testJob.parameters.Any(z => z.Key == "DevJobName"));
    }
    [Test]
    public void BuildPushAndTriggerReturns200AddSuccessfulLastExecution()
    {
        Job testJob = new Job();
        SetTriggerMethod();

        var res = sut.buildPush(testJob, new pushactivity());
        Assert.IsTrue(res.Executions.Count > 0);
        Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void BuildPushForDevBranchCallsStaticJobPath()
    {
        string staticDevPath = "job/DOM-SITES-DEV-BUILD";
        Job testJob = new Job();
        SetTriggerMethod();

        var res = sut.buildPush(testJob, new pushactivity());

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
        var res = sut.buildPush(testJob, new pushactivity());
        Assert.AreEqual(Executions + 1, testJob.Executions.Count);
        Assert.IsFalse(res.SuccesffullyRan);
    }
    [Test]
    public void BuildPushGivenJobWithEmptyGitUrlParameterAddUrlParameterFromActivity()
    {
        pushactivity req = new pushactivity { repository = new SourceControlRepository { clone_url = "http://test.foo" } };
        Job testJob = new Job();
        testJob.parameters.Add(new KeyValuePair<string, string>("GitUrl", ""));
        SetTriggerMethodWithBadRequest("GitUrl");
        var res = sut.buildPush(testJob, req);

        Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void BuildPushGivenJobWithEmptyEnvParamAndIsStagingAddStagEnvParam()
    {
        string paramKey = "Environment";
        pushactivity req = new pushactivity { repository = new SourceControlRepository { url = "http://test.foo", master_branch = "master" }, base_ref = "ref/heads/master" };
        Job testJob = new Job();
        testJob.parameters.Add(new KeyValuePair<string, string>(paramKey, ""));
        SetTriggerMethodWithBadRequest(paramKey);
        var res = sut.buildPush(testJob, req);
        Assert.IsTrue(res.SuccesffullyRan);
        Assert.AreEqual("staging", env);
    }
    [Test]
    public void BuildPushGivenJobWithEmptyEnvParamAndIsStagingAddDevEnvParam()
    {
        string paramKey = "Environment";
        pushactivity req = new pushactivity { repository = new SourceControlRepository { url = "http://test.foo", master_branch = "master" }, base_ref = "ref/heads/master2" };
        Job testJob = new Job();
        testJob.parameters.Add(new KeyValuePair<string, string>(paramKey, ""));
        SetTriggerMethodWithBadRequest(paramKey);
        var res = sut.buildPush(testJob, req);
        Assert.IsTrue(res.SuccesffullyRan);
        Assert.AreEqual("development", env);
    }
    [Test]
    public void BuildPushGivenJobWithEmptyBranchNameParameterAddBranchParameterFromJob()
    {
        pushactivity activity = new pushactivity { @ref = "ref/heads/test" };
        Job testJob = new Job();
        testJob.parameters.Add(new KeyValuePair<string, string>("BranchName", ""));
        SetTriggerMethodWithBadRequest("BranchName");


        var res = sut.buildPush(testJob, activity);

        Assert.IsTrue(res.SuccesffullyRan);
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
        var res = sut.buildPush(testJob, new pushactivity());
        Assert.AreEqual(Executions + 1, testJob.Executions.Count);
        Assert.IsFalse(res.SuccesffullyRan);
    }
    [Test]
    public void BuildPushNeverAddJobNameToParameters()
    {
        Job testJob = new Job();
        testJob.name = "test-job";
        var res = sut.buildPush(testJob, new pushactivity());

        Assert.IsFalse(testJob.parameters.Any(z => z.Key == "JobName"));
    }
    [Test]
    public void GetBuildProjectNameGivenJobRetStringThatStartsWithDom_Sites()
    {
        var activity = new pushactivity { };
        activity.@ref = "ref/heads/test-job";
        string res = sut.GetBuildProjectName(activity);
        Assert.IsTrue(res.ToLower().StartsWith("dom-sites"));
    }
    [Test]
    public void GetBuildProjectNameGivenJobRetStringThatContainsJobName()
    {
        var activity = new pushactivity {};
        activity.@ref = "ref/heads/test-job";
        string res = sut.GetBuildProjectName(activity);
        Assert.IsTrue(res.ToLower().Contains(activity.Branch));
    }
    #region private methods
    private void SetTriggerMethod(string path = "")
    {
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

    private void SetTriggerMethodWithBadRequest(string EnvKey = "")
    {
        env = "";
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
