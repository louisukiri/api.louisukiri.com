using System.Collections.Generic;
using System.Linq;
using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.entity;
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
    }
    [Test]
    public void Given200ResponseAddSuccessfulLastExecution()
    {
        Job testJob = new Job();
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
            .Returns(
            new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            }
            );

        var res = sut.build(testJob, new pushactivity());
        Assert.IsTrue(res.Executions.Count > 0);
        Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void ExecutionsIsInitializedToEmptyCollectino()
    {
        Job testJob = new Job();
        Assert.IsNotNull(testJob.Executions);
    
    }
    [Test]
    public void GivenNon200ResponseBuildAddFailedLastExecution()
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
      var res = sut.build(testJob, new pushactivity());
      Assert.AreEqual(Executions + 1, testJob.Executions.Count);
      Assert.IsFalse(res.SuccesffullyRan);
    }
    [Test]
    public void GivenJobWithEmptyGitUrlParameterAddUrlParameterFromActivity()
    {
      pushactivity req = new pushactivity { repository = new SourceControlRepository { url = "http://test.foo" } };
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
      var res = sut.build(testJob, req);

      Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void GivenJobWithEmptyBranchNameParameterAddBranchParameterFromJob()
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
      var res = sut.build(testJob, activity);

      Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void GivenExceptionAddFailedLastExecution()
    {
        Job testJob = new Job();
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
            .Returns((string a)=>{
                throw new Exception("error");
                }
            );
        int Executions = testJob.Executions.Count;
        var res = sut.build(testJob, new pushactivity());
        Assert.AreEqual(Executions + 1, testJob.Executions.Count);
        Assert.IsFalse(res.SuccesffullyRan);
    }
  }
}
