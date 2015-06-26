using System;
using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.entity;
using NUnit.Framework;

namespace api.louisukiri.com.Tests.entity
{
  [TestFixture]
  public class BuildServerRestTest
  {
    private IBuildServerRest sut;

    [TestFixtureSetUp]
    public void setup()
    {
      //sut = new JenkinsBuildServer("http://louisjenkins.dc1.corp.gd:8080/");
    }
    [Test]
    public void TriggerRestConnectionMakesReturnsNone201ReturnException()
    {
      var a = sut.trigger("louis 22");
      Assert.IsTrue(a.IsSuccessStatusCode);
    }
  }
}
