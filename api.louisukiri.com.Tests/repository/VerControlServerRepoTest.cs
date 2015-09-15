using System;
using cicd.domain.context.trigger.repository;
using Moq;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using cicdDomain;

namespace api.louisukiri.com.Tests.repository
{
    [TestFixture]
    public class VerControlServerRepoTest
    {
        [Test]
        public void GetRepoGivenValidIdReturnsRepo()
        {
            var sut = getRepo();
            var repo = sut.GetRepoBy("git@github.secureserver.net:lukiri/CI.git");

            Assert.IsNotNull(repo);
        }
        //[Test]
        //public void GetSettingGivenRepoWithNoSettingsFileReturnsEmptyDict()
        //{
        //    var sut = getRepo();
        //    var repo = sut.GetRepoBy("testgit");

        //}
        [Test]
        public void GetServerGivenInvalidUrlReturnsNull()
        {
            var sut = getRepo();
            var actual = sut.GetRepoBy("repoUrl", "Auth");

            Assert.AreEqual(null, actual);
        }
        [Test, Category("Integrated Test")]
        public void GetServerGivenValidUrlReturnsExpectedObject()
        {
            var sut = getRepo();
            var actual = sut.GetRepoBy("https://github.secureserver.net/DomainApplications/DCC5.git", testInfrastructure.APIToken);

            Assert.AreEqual("https://github.secureserver.net", actual.HostName);
        }
        [Test]
        public void GetRepoGivenValidGitUriReturnsRepo()
        {
            var sut = getRepo();
            var repo = sut.GetRepoBy("https://github.secureserver.net/lukiri/CI.git");

            Assert.IsNotNull(repo);
        }
        [Test]
        public void GetRepoGivenInvalidIdReturnsNull()
        {
            var sut = getRepo();
            var repo = sut.GetRepoBy("someinvalid id");

            Assert.IsNull(repo);
        }

        [Test, Category("Integrated Test"), Ignore]
        public void FileExistsReturnsTrueIfRequestIsSuccessful()
        {
            VerControlServerRepo sut = new VerControlServerRepo();
            var res = sut.FileContent("https://github.secureserver.net",
                testInfrastructure.APIToken
                ,"lukiri"
                , "CI"
                , "version-7"
                , "branch-config.json"
                ).Result;
            Assert.IsTrue(!string.IsNullOrWhiteSpace(res));
        }
        [Test, Category("Integrated Test"),Ignore]
        public void FileExistsReturnsFalseIfRequestFails()
        {
            VerControlServerRepo sut = new VerControlServerRepo();
            var res = sut.FileContent("https://github.secureserver.net",
                testInfrastructure.APIToken
                , "DomainApplications"
                , "DCC5"
                , "DOMWARGS-3452-1"
                , "README.md2"
                );
            Assert.AreEqual(string.Empty, res);
        }
        [Test, Category("Integrated Test"), Ignore]
        public void WriteGivenExistingFileOverwriteInRepo()
        {
            VerControlServerRepo sut = new VerControlServerRepo();
            sut.Insert(uri: "https://github.secureserver.net",
                token: testInfrastructure.APIToken
                ,owner: "lukiri"
                ,appName: "CI"
                ,branch: "version-7"
                ,fileNameWithPath: "branch-config-3.json"
                ,content:"some more"
                ,description:"update"
                );

        }
        #region private methods
        private VerControlServerRepo getRepo()
        {
            return new VerControlServerRepo();
        }

        private Mock<VerControlServerRepo> getMockedRepo()
        {
            return new Mock<VerControlServerRepo>();
        }
        #endregion
    }
}
