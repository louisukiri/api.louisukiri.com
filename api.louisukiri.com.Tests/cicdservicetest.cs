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
      buildServer.Setup(z => z.buildJob(It.IsAny<string>()));
      sut = new CICDService(buildServer.Object);
    }
    [Test]
    public void givenIdentifierCallsBuildServerService()
    {
      string name = testInfrastructure.random;
      sut.send(name);

      buildServer.Verify(z=> z.buildJob(name));
    }
  }
}
