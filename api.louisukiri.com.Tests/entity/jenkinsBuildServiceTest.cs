using System.Collections.Generic;
using System.Linq;
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
    [TestFixtureSetUp]
    public void setup()
    {
        _sut = new Mock<JenkinsBuildService>(); 
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
             .Returns((string a, string b, string c, List<KeyValuePair<string, string>> d) =>
             {
                 HttpResponseMessage msg = null;
                 var exists = d.Any(z => z.Key == "BranchName" && !string.IsNullOrWhiteSpace(z.Value));
                 if (exists)
                 {
                     msg = new HttpResponseMessage
                     {
                         StatusCode = System.Net.HttpStatusCode.OK
                     };
                 }
                 return msg ?? new HttpResponseMessage
                 {
                     StatusCode = System.Net.HttpStatusCode.BadRequest
                 };

             }
          );
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
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
            .Returns(
            new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            }
            );

        var res = sut.buildSeed(testJob, new pushactivity());
        Assert.IsTrue(res.Executions.Count > 0);
        Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void BuildSeedAndTriggerReturnsNon200AddFailedLastExecution()
    {
      Job testJob = new Job();
      _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
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
      _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
          .Returns((string a, string b, string c, List<KeyValuePair<string, string>> d) =>
          {
            HttpResponseMessage msg = null;
            var exists = d.Any(z => z.Key == "GitUrl" && !string.IsNullOrWhiteSpace(z.Value));
            if (exists)
            {
              msg = new HttpResponseMessage
              {
                StatusCode = System.Net.HttpStatusCode.OK
              };
            }
            return msg ?? new HttpResponseMessage
            {
              StatusCode = System.Net.HttpStatusCode.BadRequest
            };

          }
       );
      var res = sut.buildSeed(testJob, req);

      Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void BuildSeedGivenJobWithEmptyEnvParamAndIsStagingAddStagEnvParam()
    {
      string paramKey = "Environment";
      pushactivity req = new pushactivity { repository = new SourceControlRepository { url = "http://test.foo", master_branch = "master" }, base_ref = "ref/heads/master" };
      Job testJob = new Job();
      string env = "";
      testJob.parameters.Add(new KeyValuePair<string, string>(paramKey, ""));
      _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
          .Returns((string a, string b, string c, List<KeyValuePair<string, string>> d) =>
          {
            HttpResponseMessage msg = null;
            var exists = d.Any(z => z.Key == paramKey && !string.IsNullOrWhiteSpace(z.Value));
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
      string env = "";
      testJob.parameters.Add(new KeyValuePair<string, string>(paramKey, ""));
      _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
          .Returns((string a, string b, string c, List<KeyValuePair<string, string>> d) =>
          {
            HttpResponseMessage msg = null;
            var exists = d.Any(z => z.Key == paramKey && !string.IsNullOrWhiteSpace(z.Value));
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
      _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
          .Returns((string a, string b, string c, List<KeyValuePair<string, string>> d) =>
          {
            HttpResponseMessage msg = null;
            var exists = d.Any(z => z.Key == "BranchName" && !string.IsNullOrWhiteSpace(z.Value));
            if (exists)
            {
              msg = new HttpResponseMessage
              {
                StatusCode = System.Net.HttpStatusCode.OK
              };
            }
            return msg ?? new HttpResponseMessage
            {
              StatusCode = System.Net.HttpStatusCode.BadRequest
            };

          }
       );
      var res = sut.buildSeed(testJob, activity);

      Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void BuildSeedExceptionThrownInTriggerAddFailedLastExecution()
    {
        Job testJob = new Job();
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
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
        var res = sut.buildSeed(testJob, new pushactivity());

        Assert.IsTrue(testJob.parameters.Any(z=> z.Key=="JobName"));
    }
    [Test]
    public void BuildPushdAndTriggerReturns200AddSuccessfulLastExecution()
    {
        Job testJob = new Job();
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
            .Returns(
            new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            }
            );

        var res = sut.buildPush(testJob, new pushactivity());
        Assert.IsTrue(res.Executions.Count > 0);
        Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void BuildPushdCallTriggerWithJobNameAsPath()
    {
        Job testJob = new Job{ name = "Test"};
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), "job/DOM-SITES-Test", It.IsAny<List<KeyValuePair<string, string>>>()))
            .Returns(
            new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            }
            );

        var res = sut.buildPush(testJob, new pushactivity());

        _sut.Verify(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), "job/DOM-SITES-Test", It.IsAny<List<KeyValuePair<string, string>>>()));
    }
    [Test]
    public void BuildPushAndTriggerReturnsNon200AddFailedLastExecution()
    {
        Job testJob = new Job();
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
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
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
            .Returns((string a, string b, string c, List<KeyValuePair<string, string>> d) =>
            {
                HttpResponseMessage msg = null;
                var exists = d.Any(z => z.Key == "GitUrl" && !string.IsNullOrWhiteSpace(z.Value));
                if (exists)
                {
                    msg = new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                }
                return msg ?? new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            }
         );
        var res = sut.buildPush(testJob, req);

        Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void BuildPushGivenJobWithEmptyEnvParamAndIsStagingAddStagEnvParam()
    {
        string paramKey = "Environment";
        pushactivity req = new pushactivity { repository = new SourceControlRepository { url = "http://test.foo", master_branch = "master" }, base_ref = "ref/heads/master" };
        Job testJob = new Job();
        string env = "";
        testJob.parameters.Add(new KeyValuePair<string, string>(paramKey, ""));
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
            .Returns((string a, string b, string c, List<KeyValuePair<string, string>> d) =>
            {
                HttpResponseMessage msg = null;
                var exists = d.Any(z => z.Key == paramKey && !string.IsNullOrWhiteSpace(z.Value));
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
        string env = "";
        testJob.parameters.Add(new KeyValuePair<string, string>(paramKey, ""));
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
            .Returns((string a, string b, string c, List<KeyValuePair<string, string>> d) =>
            {
                HttpResponseMessage msg = null;
                var exists = d.Any(z => z.Key == paramKey && !string.IsNullOrWhiteSpace(z.Value));
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
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
            .Returns((string a, string b, string c, List<KeyValuePair<string, string>> d) =>
            {
                HttpResponseMessage msg = null;
                var exists = d.Any(z => z.Key == "BranchName" && !string.IsNullOrWhiteSpace(z.Value));
                if (exists)
                {
                    msg = new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                }
                return msg ?? new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            }
         );
        var res = sut.buildPush(testJob, activity);

        Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void BuildPushExceptionThrownInTriggerAddFailedLastExecution()
    {
        Job testJob = new Job();
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
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
    public void BuildPushAlwaysAddJobNameToParameters()
    {
        Job testJob = new Job();
        testJob.name = "test-job";
        var res = sut.buildPush(testJob, new pushactivity());

        Assert.IsTrue(testJob.parameters.Any(z => z.Key == "JobName"));
    }
    [Test]
    public void GetBuildProjectNameGivenJobRetStringThatStartsWithDom_Sites()
    {
        Job testJob = new Job();
        testJob.name = "test-job";
        string res = sut.GetBuildProjectName(testJob);
        Assert.IsTrue(res.ToLower().StartsWith("dom-sites"));
    }
    [Test]
    public void GetBuildProjectNameGivenJobRetStringThatContainsJobName()
    {
        Job testJob = new Job();
        testJob.name = "test-job";
        string res = sut.GetBuildProjectName(testJob);
        Assert.IsTrue(res.ToLower().Contains(testJob.name));
    }
  }
}
