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
    public void whenConstructionInitializeActivity()
    {
        var sut = testInfrastructure.getRequestPayload();
        Assert.IsNotNull(sut.Activity);
    }
    [Test]
    public void whenConstructionInitializeRequestTrigger()
    {
        var sut = testInfrastructure.getRequestPayload();
        Assert.IsNotNull(sut.Trigger);
    }
    [Test]
    public void whenConstructionInitializeThrowErrorIfTriggerIsUnexpected()
    {
        Assert.Throws<ArgumentException>(delegate { testInfrastructure.getRequestPayload(RequestTrigger.Pull); });
    }
    [Test]
    public void whenConstructionInitializeThrowErrorIfActivityIsNull()
    {
        Assert.Throws<ArgumentException>(delegate { var a = new RequestPayload(RequestTrigger.Pull, "{'badjsonstring':'okjim'}"); });
    }
  }
}
