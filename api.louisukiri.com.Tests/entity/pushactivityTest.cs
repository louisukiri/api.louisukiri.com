
using cicd.domain.context.trigger.entity;
using cicd.infrastructure;
using NUnit.Framework;
using Octokit;

namespace api.louisukiri.com.Tests.entity
{
  [TestFixture]
  public class pushactivityTest
  {
      [Test]
      public void TypeReturnsPullIfPull_RequestExists()
      {
          pushactivity sut = getActivity(getVCUser("test@test.com", "test"), pullRequest: new GitHubPullRequest());

          Assert.AreEqual(RequestTrigger.Pull, sut.type);
      }
    [Test]
    public void TypeReturnsPushIfPusherExists()
    {
      pushactivity sut = getActivity(getVCUser("test@test.com","test"));

      Assert.AreEqual(RequestTrigger.Push, sut.type);
    }
    [Test]
    public void TypeReturnsCreateIfCreatedIsTrueAndValidUser()
    {
      pushactivity sut = getActivity(created: true, user: getVCUser("abc@ab.com","a"));
      Assert.AreEqual(RequestTrigger.Branch, sut.type);
    }

    [Test]
    public void IdIsRepoUrlWithTypeAndNonAlphasAreDashes()
    {
      pushactivity sut = getActivity(user: getVCUser("test@t.com", "test"), repo: getSourceControlRepository("Http://test.com"));
      Assert.AreEqual("http-test-com-push", sut.Id);
    }
    [Test]
    public void BranchIsLastValueInRefArray()
    {
      pushactivity sut = getActivity(refDir: "ref/heads/testBranch");
      Assert.AreEqual("testBranch", sut.Branch);
    }
    [Test]
    public void BranchIsLastValueInFromHeadIfPullRequest()
    {
        pushactivity sut = getActivity(pullRequest: new GitHubPullRequest{ head = new RefHead{ @ref="ref/heads/test" }});
        Assert.AreEqual("test", sut.Branch);
    }
    [Test]
    public void BaseBranchIsLastValueInBaseRefArray()
    {
      pushactivity sut = getActivity(baseRefDir: "ref/heads/testBranch");
      Assert.AreEqual("testBranch", sut.BaseBranch);
    }
    [Test]
    public void IsStagingJobIfBaseBranchIsMaster()
    {
      pushactivity sut = getActivity(baseRefDir: "ref/heads/master", repo: getSourceControlRepository(master_branch: "master"));
      Assert.IsTrue(sut.IsStagingBranch);
    }
    [Test]
    public void IsNotStagingIfRepoIsNull()
    {
      pushactivity sut = getActivity(baseRefDir: "ref/heads/master");
      Assert.IsFalse(sut.IsStagingBranch);
    }
    [Test]
    public void BranchIsEmptyStringIfRefHasLT3Sections()
    {
      pushactivity sut = getActivity(refDir: "ref/testBranch");
      Assert.AreEqual(string.Empty, sut.Branch);
    }
    [Test]
    public void BranchIsEmptyStringIfRefHasGT3Sections()
    {
      pushactivity sut = getActivity(refDir: "ref/heads/extra/testBranch");
      Assert.AreEqual(string.Empty, sut.Branch);
    }
    [Test]
    public void BranchIsEmptyStringIfIndex1IsNotHeads()
    {
      pushactivity sut = getActivity(refDir: "ref/Notheads/testBranch");
      Assert.AreEqual(string.Empty, sut.Branch);
    }
    #region private methods
    private pushactivity getActivity(VersionControlUser user=null, bool created=false, SourceControlRepository repo=null, string refDir="", string baseRefDir="", GitHubPullRequest pullRequest=null)
    {
      return new pushactivity {pusher = user, created = created, repository = repo, @ref = refDir, base_ref = baseRefDir, pull_request = pullRequest};
    }
    private VersionControlUser getVCUser(string email = "", string name = "")
    {
      return new VersionControlUser {email = email, name = name};
    }
    private SourceControlRepository getSourceControlRepository(string url = "", string master_branch="")
    {
      return new SourceControlRepository {url = url, master_branch = master_branch};
    }
    #endregion
  }
}
