using System;
using cicdDomain;
using cicdDomain.cicd.infrastructure;
using NUnit.Framework;

namespace api.louisukiri.com.Tests.infrastructure
{
  [TestFixture]
  public class RequestPayloadTest
  {
    [Test]
    public void throwWhenInitializedWithEmptyPayload()
    {
      Assert.Throws<ArgumentNullException>(delegate { var a = new RequestPayload(RequestTrigger.Pull, ""); });
    }
    [Test]
    public void whenGettingTypeReturnPushIfPusherExists()
    {
      var sut = new RequestPayload(RequestTrigger.Pull, testInfrastructure.GitHubPushContent);

      Assert.AreEqual(RequestTrigger.Push, sut.getTriggerTypeFromPayloadString());
    }

  }
}
