using cicd.domain.context.trigger.entity;
using NUnit.Framework;

namespace api.louisukiri.com.Tests.entity
{
    [TestFixture]
    public class SettingsTest
    {
        [Test]
        public void OnEqualReturnsFalseGivenDifferentLevels()
        {
            var a = new Settings
            {
                Branch = new BranchSetting
                {
                    BaseBranch = "master"
                    ,
                    Level = "Version"
                }
            };
            var b = new Settings
            {
                Branch = new BranchSetting
                {
                    BaseBranch = "master",
                    Level = "Version2"
                }
            };
            Assert.IsFalse(a==b);
        }
        [Test]
        public void OnEqualReturnsFalseGivenDifferentBaseBranch()
        {
            var a = new Settings
            {
                Branch = new BranchSetting
                {
                    BaseBranch = "master",
                    Level = "Version"
                }
            };
            var b = new Settings
            {
                Branch = new BranchSetting
                {
                    BaseBranch = "master2",
                    Level = "Version"
                }
            };

            Assert.AreNotEqual(a, b);
        }
    }
}
