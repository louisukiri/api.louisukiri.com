using System;
using cicdDomain.cicd.domain.entity;
using cicdDomain.cicd.infrastructure;
using NUnit.Framework;

namespace api.louisukiri.com.Tests.entity
{
  [TestFixture]
  public class pushactivityTest
  {
    [Test]
    public void TypeReturnsPushIfPusherExists()
    {
      pushactivity sut = getActivity(getVCUser("test@test.com","test"));

      Assert.AreEqual(RequestTrigger.Push, sut.type);
    }
    [Test]
    public void TypeReturnsUnknownIfPushDoesntExist()
    {
      pushactivity sut = getActivity();
      Assert.AreEqual(RequestTrigger.Unknown, sut.type);
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
    private pushactivity getActivity(VersionControlUser user=null, bool created=false, SourceControlRepository repo=null, string refDir="", string baseRefDir="")
    {
      return new pushactivity {pusher = user, created = created, repository = repo, @ref = refDir, base_ref = baseRefDir};
    }
    private VersionControlUser getVCUser(string email = "", string name = "")
    {
      return new VersionControlUser {email = email, name = name};
    }
    private SourceControlRepository getSourceControlRepository(string url = "")
    {
      return new SourceControlRepository {url = url};
    }
    #endregion
  }
}
