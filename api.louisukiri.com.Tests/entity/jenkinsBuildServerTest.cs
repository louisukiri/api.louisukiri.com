using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.entity;
using Moq;
using NUnit.Framework;


namespace api.louisukiri.com.Tests.entity
{
  [TestFixture]
  public class JenkinsBuildServerTest
  {
    public Mock<JenkinsBuildServer> _sut;
    public Mock<IBuildServerRest> buildServerRest;

    public JenkinsBuildServer sut
    {
      get { return _sut.Object; }
    }
    [TestFixtureSetUp]
    public void setup()
    {
      buildServerRest = new Mock<IBuildServerRest>();
      buildServerRest.Setup(z => z.trigger(It.IsAny<string>()));
      _sut = new Mock<JenkinsBuildServer>(buildServerRest.Object);
    }
    [Test]
    public void RequiresIBuildServerRest()
    {
      JenkinsBuildServer server = new JenkinsBuildServer(buildServerRest.Object);
      Assert.IsNotNull(server.BuildServerRest);
    }
    [Test]
    public void SendsJobToServerUsingRestApi()
    {
      _sut.Object.buildJob("test");
      buildServerRest.Verify(z=> z.trigger(It.IsAny<string>()));
    }
  }
}
