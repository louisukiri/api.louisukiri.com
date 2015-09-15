using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.entity.bot;
using cicd.domain.context.trigger.events;
using cicdDomain;
using NUnit.Framework;

namespace api.louisukiri.com.Tests.integratedTests
{
    [TestFixture]
    public class SlackBotIntegrationTest
    {
        [Test,Ignore]
        public void TestSlackTalk()
        {
            var slackBot = new SlackBot();
            var buildevent = new BuildEvent(testInfrastructure.getJob(), testInfrastructure.BrnValidBranch);

            Assert.DoesNotThrow(()=> slackBot.Trigger(buildevent));
        }
    }
}
