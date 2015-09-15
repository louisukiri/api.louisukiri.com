using System;
using cicd.domain.context.trigger.entity;
using cicdDomain;
using NUnit.Framework;

namespace godaddy.domains.cicd.Tests.entity
{
    [TestFixture]
    public class VerControlServerTest
    {
        private VerControlServer _sut;
        [Test]
        public void VerControlServerAlwaysHasId()
        {
            _sut = GetServer();
            Assert.IsNotNull(_sut.Id);
        }
        [Test]
        public void VerControlServerCtorGivenInvalidUrlAsIdThrows()
        {
            Assert.Throws<UriFormatException>(() => GetServer(id: "invalidUrl.git"));
        }
        [Test]
        public void VerControlServerGivenIdWithoutGitEndingThrows()
        {
            Assert.Throws<ArgumentException>(() =>  GetServer(id: "git@github.secureserver.net:user/app"));
        }
        [Test]
        public void VerControlServerGivenSshIdDerivesOwnerFromId()
        {
            _sut = GetServer(id: "git@github.secureserver.net:user/app.git", token: "okjim");
            Assert.AreEqual("user", _sut.Auth.Owner);
        }
        [Test]
        public void VerControlServerGivenUriIdDerivesOwnerFromId()
        {
            _sut = GetServer(id: testInfrastructure.RepoUriUrl, token: "okjim");
            Assert.AreEqual("user", _sut.Auth.Owner);
        }
        [Test]
        public void VerControlServerAlwaysHasAuth()
        {
            _sut = GetServer();
            Assert.AreNotEqual(default(VersionControlAuth), _sut.Auth);
        }
        [Test]
        public void GitFullUriReturnsIdWhenIdIsHttpUri()
        {
            string testUrl = testGitUrl;
            _sut = GetServer(id: testUrl);

            Assert.AreEqual(testUrl, _sut.GitFullUriString);
        }
        [Test]
        public void GitFullUriReturnsUriWhenIdIsSshUrl()
        {
            _sut = GetServer(id: testInfrastructure.RepoSshUrl);

            Assert.AreEqual("https://github.test.net/user/app.git", _sut.GitFullUriString);
        }
        [Test]
        public void GitUriReturnsUriWhenIdIsSshUrl()
        {
            _sut = GetServer(id: testInfrastructure.RepoSshUrl);

            Assert.AreEqual("https://github.test.net/user/app.git", _sut.GitFullUriString);
        }
        [Test]
        public void HostNameReturnsExpectedValueForSshIds()
        {
            _sut = GetServer(id: testInfrastructure.RepoSshUrl);

            Assert.AreEqual("https://github.test.net", _sut.HostName);
        }
        [Test]
        public void HostNameReturnsExpectedValueForUris()
        {
            _sut = GetServer(id: testInfrastructure.RepoUriUrl);

            Assert.AreEqual("https://github.test.net", _sut.HostName);
        }
        [Test]
        public void AppNameReturnsExpectedAppNameFromGitSshUrl()
        {
            _sut = GetServer(id: testInfrastructure.RepoSshUrl);

            Assert.AreEqual("app", _sut.AppName);
        }
        [Test]
        public void GitSshUrlReturnsExpectedGivenUriConstructor()
        {
            _sut = GetServer(testInfrastructure.RepoUriUrl);
            Assert.AreEqual("git@github.test.net:user/app.git", _sut.GitSshUrlString);
        }
        [Test]
        public void GitSshUrlReturnsExpectedGivenSshConstructor()
        {
            _sut = GetServer(testInfrastructure.RepoSshUrl);
            Assert.AreEqual("git@github.test.net:user/app.git", _sut.GitSshUrlString);
        }
        [Test]
        public void AppNameReturnsExpectedAppNameFromGitUri()
        {
            _sut = GetServer(id: testInfrastructure.RepoUriUrl);

            Assert.AreEqual("app", _sut.AppName);
        }
        #region private methods

        private string testGitUrl
        {
            get { return "https://test.com/user/testId.git"; }
        }
        private VerControlServer GetServer(string id = null, string token = null)
        {
            return new VerControlServer(id??testGitUrl, token??"testtoken");
        }
        #endregion
    }
}
