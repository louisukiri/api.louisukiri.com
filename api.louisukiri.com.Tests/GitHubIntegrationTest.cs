using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Octokit;

namespace api.louisukiri.com.Tests
{
    [TestClass]
    public class GitHubIntegrationTest
    {
        [TestMethod]
        public void CanConnectWithOctoKit()
        {
            var github = new GitHubClient(new ProductHeaderValue("CI"), new Uri("https://github.secureserver.net"));
            github.Credentials = new Credentials("478f46bd1cddea30f4d35cf9ea25cce427da7630");

            var repo = github.Repository.Get("lukiri", "CI").Result;
            NewCommitStatus status = new NewCommitStatus();
            status.Context = "Context";
            status.Description = "description string";
            status.State = CommitState.Failure;
            var b = github.Repository.Commits.Get("lukiri", "CI", "version-7").Result;
            var c = github.Repository.CommitStatus.Create("lukiri", "CI", b.Sha, status)
                .Result
                ;


            Assert.IsNotNull(b);
        }
    }
}
