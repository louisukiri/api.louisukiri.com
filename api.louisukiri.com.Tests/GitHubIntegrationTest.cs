using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Octokit;
using Octokit.Internal;
using cicdDomain;

namespace api.louisukiri.com.Tests
{
    [TestClass]
    public class GitHubIntegrationTest
    {
        [TestMethod, Ignore]
        public void CanConnectWithOctoKit()
        {
            var github = new GitHubClient(new ProductHeaderValue("CI"), new Uri("https://github.secureserver.net"));
            github.Credentials = new Credentials(testInfrastructure.APIToken);

            //var repo = github.Repository.Get("lukiri", "CI").Result;
            NewCommitStatus status = new NewCommitStatus();
            status.Context = "Context";
            status.Description = "description string";
            status.State = CommitState.Success;
            var b = github.Repository.Commits.Get("lukiri", "CI", "version-7").Result;
            
            var c = github.Repository.CommitStatus.Create("lukiri", "CI", b.Sha, status)
                .Result
                ;


            Assert.IsNotNull(b);
        }
        [TestMethod, Ignore]
        public void CanUploadWithOctokit()
        {
            var github = new GitHubClient(new ProductHeaderValue("CI"), new Uri("https://github.secureserver.net"));
            github.Credentials = new Credentials(testInfrastructure.APIToken);
            //var a = github.Repository.Content.CreateFile("lukiri", "CI", "branch-config-2.json",
            //    new CreateFileRequest("Branch configuration", "{'environment':'staging'}"){ Branch = "version-7"}).Result;

        }
        [TestMethod, Ignore]
        public void CanGetFileWithOctokit()
        {
            var github = new GitHubClient(new ProductHeaderValue("CI"), new Uri("https://github.secureserver.net"));
            github.Credentials = new Credentials(testInfrastructure.APIToken);
            
            var a = github.Repository.Content
                .GetAllContents("lukiri", "CI", "branch-config-2.json?ref=version-7")
                .Result
                .First()
                .DownloadUrl;
            
                //.CreateFile("lukiri", "CI", "branch-config-2.json",
                //new CreateFileRequest("Branch configuration", "{'environment':'staging'}") { Branch = "version-7" }).Result;

        }
    }
}
