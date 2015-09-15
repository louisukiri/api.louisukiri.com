
using cicd.domain.context.trigger.entity;
using cicdDomain;
using NUnit.Framework;

namespace api.louisukiri.com.Tests.entity
{
    [TestFixture]
    public class BranchTest
    {
        [Test]
        public void CreateFromGivenNullPayloadReturnsNull()
        {
            Branch result = Branch.CreateFrom(default(pushactivity));
            Assert.IsNull(result);
        }
        [Test]
        public void CreateFromGivenPayloadReturnsExpectedName()
        {
            Branch result = Branch.CreateFrom(testInfrastructure.GitHubPushActivity);

            Assert.AreEqual("master", result.Name);
        }
        [Test]
        public void CreateFromGivenPayloadReturnsExpectedParent()
        {
            pushactivity activity = testInfrastructure.GitHubPushActivity;
            activity.SetBase("test");
            Branch result = Branch.CreateFrom(activity);

            Assert.AreEqual("test", result.Parent);
        }
        [Test]
        public void CreateFromGivenPayloadReturnsExpectedServer()
        {
            pushactivity activity = testInfrastructure.GitHubPushActivity;
            Branch result = Branch.CreateFrom(activity);

            Assert.AreEqual("https://github.secureserver.net/lukiri/CI.git", result.Server.GitFullUriString);
        }
        [Test]
        public void CreateFromGivenPayloadLevelIsUnknown()
        {
            pushactivity activity = testInfrastructure.GitHubPushActivity;
            Branch result = Branch.CreateFrom(activity);

            Assert.AreEqual(BranchLevel.Unknown, result.Level);
        }
        [Test]
        public void CreateFromGivenBaseRefIsMasterIsVersionBranchIsTrue()
        {
            pushactivity activity = testInfrastructure.GitHubPushActivity;
            activity.SetBase("master");
            activity.SetRepoMasterBranch("master");
            Branch result = Branch.CreateFrom(activity);

            Assert.IsTrue(result.IsVersionBranch);
        }
    }

}
