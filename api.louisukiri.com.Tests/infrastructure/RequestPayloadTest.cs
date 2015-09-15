using System;
using cicd.infrastructure;
using cicdDomain;
using NUnit.Framework;

namespace api.louisukiri.com.Tests.infrastructure
{
  [TestFixture]
  public class RequestPayloadTest
  {
    [Test]
    public void ThrowWhenInitializedWithEmptyPayload()
    {
      Assert.Throws<ArgumentNullException>(delegate { var a = new RequestPayload(RequestTrigger.Pull, ""); });
    }
    [Test]
    public void WhenConstructionInitializeActivity()
    {
        var sut = testInfrastructure.getRequestPayload();
        Assert.IsNotNull(sut.Activity);
    }
    [Test]
    public void WhenConstructionInitializeRequestTrigger()
    {
        var sut = testInfrastructure.getRequestPayload();
        Assert.IsNotNull(sut.Trigger);
    }
    [Test]
    public void WhenConstructionInitializeThrowErrorIfTriggerIsUnexpected()
    {
        Assert.Throws<ArgumentException>(delegate { testInfrastructure.getRequestPayload(RequestTrigger.Pull); });
    }
    [Test]
    public void WhenConstructionInitializeThrowErrorIfActivityIsNull()
    {
        Assert.Throws<ArgumentException>(delegate { var a = new RequestPayload(RequestTrigger.Pull, "{'badjsonstring':'okjim'}"); });
    }
    [Test]
    public void WhenCtorGivenPullPayloadPullPropertyNotNull()
    {
        var sut = testInfrastructure.getRequestPayload(testInfrastructure.GithubPullRequestContent, RequestTrigger.Pull);

        Assert.IsNotNull(sut.Activity.pull_request);
    }
    [Test]
    public void WhenCtorSetBranch()
    {
        var sut = testInfrastructure.getRequestPayload(testInfrastructure.GithubPullRequestContent, RequestTrigger.Pull);

        Assert.AreNotEqual(default(RequestTrigger), sut.Branch);
    }
    [Test]
    public void WhenCtorSetTrigger()
    {
        var sut = testInfrastructure.getRequestPayload(testInfrastructure.GithubPullRequestContent, RequestTrigger.Pull);

        Assert.IsNotNull(sut.Trigger);
    }
  }
}
