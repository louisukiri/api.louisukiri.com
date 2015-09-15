using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.entity.bot;
using cicd.domain.context.trigger.values;
using cicdDomain;
using godaddy.domains.cicd.Tests.helpers;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace api.louisukiri.com.Tests.entity
{
    [TestFixture]
    public class SlackBotBrainTest
    {
        private SlackBotBrain _sut;
        private Mock<IBot> _bot;
        private Mock<IPersonality> _personality;
        private readonly string _randomPrep = testInfrastructure.random;
        private readonly string _randomAction = testInfrastructure.random;
        private readonly string _randomNoun = testInfrastructure.random;
        private readonly string _randomConjPres = testInfrastructure.random;
        private readonly string _randomActivityNoun = testInfrastructure.random;
        [SetUp]
        public void Setup()
        {
            _bot = new Mock<IBot>();
            _bot.Setup(z => z.Say(It.IsAny<string>()));
            
            _personality = new Mock<IPersonality>();
            _personality.Setup(z => z.PrepositionAt(It.IsAny<EventType>())).Returns(_randomPrep);
            _personality.SetupGet(z => z[It.IsAny<string>()]).Returns(_randomAction);
            _personality.Setup(z => z[It.IsAny<EventType>()]).Returns(_randomAction);
            _personality.Setup(z => z.ActivityNoun(It.IsAny<EventType>())).Returns(_randomActivityNoun);
            _personality.Setup(z => z.ConjMe(It.IsAny<EventType>())).Returns(_randomConjPres);
            _sut = GetSut(_bot.Object, new SlackBotPersonality());
        }
        [Test]
        public void RequiresIBot()
        {
            var sut = GetSut(new Mock<IBot>().Object);
            Assert.IsNotNull(sut.Bot);
        }
        [Test]
        public void RequireIPersonality()
        {
            var sut = GetSut(new Mock<IBot>().Object, new Mock<IPersonality>().Object);
            Assert.IsNotNull(sut.Personality);
        }
        //[Test]
        //public void ProcessingGivenEventRunEventsProcess()
        //{
        //    var eventMock = new Mock<IEvent>();
        //    string res = "";
        //    eventMock.Setup(z => z.Run());

        //    _sut.Process(eventMock.Object);

        //    eventMock.Verify(z=> z.Run());
        //}
        //[Test]
        //public void ProcessingGivenValidEventTemplateTokensAreReplacedByPersonalityValues()
        //{
        //    var eventMock = new Mock<IEvent>();
        //    var personalityMock = new Mock<IPersonality>();
        //    var botMock = new Mock<IBot>();
        //    var ran = testInfrastructure.random;
        //    var failures = testInfrastructure.random;
        //    var success = testInfrastructure.random;
        //    var verbMe = testInfrastructure.random;
        //    eventMock.Setup(z => z.Run())
        //        .Returns("{Ran} tests on. There were {Failures} and {Success}");
        //    personalityMock.SetupGet(z => z["Ran"]).Returns(ran);
        //    personalityMock.SetupGet(z => z["Failures"]).Returns(failures);
        //    personalityMock.SetupGet(z => z["Success"]).Returns(success);
        //    personalityMock.SetupGet(z => z["verb-me"]).Returns(verbMe);
        //    personalityMock.Setup(z => z.Talk(It.IsAny<string>())).Returns((string content) => content);
        //    string expected = string.Format("{3} {0} tests on. There were {1} and {2}", ran, failures, success, verbMe);
        //    botMock.Setup(z => z.Say(expected));

        //    var sut = GetSut(botMock.Object, personalityMock.Object);
        //    sut.Process(eventMock.Object);

        //    botMock.Verify(z=> z.Say(expected));
        //}
        //[Test]
        //public void ProcessingGivenBlankEventTemplateBotSaysNothing()
        //{
        //    var eventMock = new Mock<IEvent>();
        //    var personalityMock = new Mock<IPersonality>();
        //    var botMock = new Mock<IBot>();
        //    eventMock.Setup(z => z.Run())
        //        .Returns("");
        //    const string expected = "";
        //    botMock.Setup(z => z.Say(expected));

        //    var sut = GetSut(botMock.Object, personalityMock.Object);
        //    sut.Process(eventMock.Object);

        //    botMock.Verify(z => z.Say(expected), Times.Never());
        //}
        //[Test]
        //public void ProcessingGivenPersonalityTalkStringIsModifiedBeforeBotTalk()
        //{
        //    var randomString = testInfrastructure.random;
        //    var personality = new Mock<IPersonality>();
        //    var verbMe = testInfrastructure.random;
        //    personality.Setup(z => z["verb-me"]).Returns(verbMe);
        //    personality.Setup(z => z.Talk(It.IsAny<string>()))
        //        .Returns((string content) => { return "YO! " + content; });
        //    var bot = new Mock<IBot>();
        //    bot.Setup(z => z.Say(It.IsAny<string>()));
        //    var @event = new Mock<IEvent>();
        //    @event.Setup(z => z.Run()).Returns(randomString);

        //    var expected = "YO! " + verbMe + " " + randomString;
        //    var sut = GetSut(bot.Object, personality.Object);
        //    sut.Process(@event.Object);

        //    bot.Verify(z=>z.Say(expected));
        //}
        [Test]
        public void ProcessingGivenNullEventSayNothing()
        {
            _bot.Setup(z => z.Say(It.IsAny<string>()));
            _sut.Process(null);
            _bot.Verify(z=> z.Say(It.IsAny<string>()), Times.Never());
        }
        public IEnumerable<TestCaseData> BotSayEventData
        {
            get
            {
                yield return new TestCaseData("testName", EventType.Deploy, "TestName", "", "directionalWhere", "", "to", "deploying", "", null, "I am deploying TestName to directionalWhere").SetName("DeployEventSaysExpected");
                yield return new TestCaseData("testName", EventType.Build, "TestWhat", "testWhere", "", "", "", "", "", null, "I am building TestWhat at testWhere").SetName("BuildEventSaysExpected");
                yield return new TestCaseData("testName", EventType.Build, "TestWhat", "", "", "", "", "", "", null, "I am building TestWhat").SetName("BuildEventWithoutWhatSaysExpected");
                yield return new TestCaseData("testName", EventType.PreTest, "TestWhat", "testWhere", "", "", "", "", "", null, "I am beginning tests on TestWhat at testWhere. Please hold for results...").SetName("PreTestEventSaysExpected");
                yield return new TestCaseData("testName", EventType.TestResults, "TestWhat", "testWhere", "", "", "", "", "", new Dictionary<string, string>() { { "test", "Test" }, { "test2", "Test2" } }, 
@"The Results of testing are:
test:Test
test2:Test2".Replace("\r","")).SetName("ReturnEventSaysExpected");
            }
        }
            
        [TestCaseSource("BotSayEventData")]
        public void GivenEventReturnExpected(string Name = "", 
            EventType type = default(EventType), string What = ""
            ,string Where = "", string directionalWhere=""
            ,string at="at", string to="to",string action="",string ActivityNoun=""
            ,IDictionary<string, string> results=null
            ,string Expected="")
        {

            _bot = new Mock<IBot>();
            _bot.Setup(z => z.Say(It.IsAny<string>()));

           
            _sut = GetSut(_bot.Object, new SlackBotPersonality());

            var eventMock = new Mock<IEvent>();
            eventMock.Setup(z => z.Type).Returns(type);
            eventMock.Setup(z => z.Results).Returns(results);
            eventMock.Setup(z => z.Name).Returns(Name);
            eventMock.Setup(z => z.Where).Returns(Where);
            eventMock.Setup(z => z.What).Returns(What);
            eventMock.Setup(z => z.Destination).Returns(directionalWhere);

            _sut.Process(eventMock.Object);

            _bot.Verify(z => z.Say(Expected));
        }
        #region private methods
        private SlackBotBrain GetSut(IBot bot=null, IPersonality personality=null)
        {
            return new SlackBotBrain(bot,personality);
        }
        #endregion
    }
}
