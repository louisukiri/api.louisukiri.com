using System;
using cicdDomain;
using NUnit.Framework;
using cicdDomain.cicd.domain.service;
using cicd.domain.cicd.domain.abstracts;
using Moq;

namespace api.louisukiri.com.Tests
{
  [TestFixture]
  public class cicdserviceTest
  {
    private CICDService sut;
    private Mock<IBuildServer> buildServer;

    [TestFixtureSetUp]
    public void setup()
    {
      buildServer = new Mock<IBuildServer>();
    }
    [Test]
    public void givenIdentifierCallsBuildServerService()
    {
      string name = testInfrastructure.random;
      sut.send(name);

      
    }
  }
}
