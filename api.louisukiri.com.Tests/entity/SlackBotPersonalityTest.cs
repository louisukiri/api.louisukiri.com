using System.Collections.Generic;
using cicd.domain.context.trigger.entity.bot;
using cicd.domain.context.trigger.values;
using cicdDomain;
using NUnit.Framework;

namespace godaddy.domains.cicd.Tests.entity
{
    [TestFixture]
    public class SlackBotPersonalityTest
    {
        private SlackBotPersonality _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new SlackBotPersonality();
        }
        [TestCase(EventType.Build, "building")]
        [TestCase(EventType.Deploy, "deploying")]
        [TestCase(default(EventType), "")]
        public void EventIndexerReturnsExpectedString(EventType type, string value)
        {
            Assert.AreEqual(value, _sut[type]);
        }
        [TestCase(EventType.Build, "building")]
        [TestCase(default(EventType), "")]
        [TestCase(EventType.Deploy, "deploying")]
        public void VerbReturnsExpectedString(EventType type, string value)
        {
            Assert.AreEqual(value, _sut.Verb(type));
        }
        [Test]
        public void CtorGivenKeyValuePairsInsertsIntoTermMap()
        {
            string key1 = testInfrastructure.random;
            string key2 = testInfrastructure.random;
            while (key1 == key2)
            {
                key2 = testInfrastructure.random;
            }
            var keyval1 = new KeyValuePair<string, IList<string>>(key1,new List<string>{"a","b"});
            var keyval2 = new KeyValuePair<string, IList<string>>(key2, new List<string> {"c", "d"});
            var sut = new SlackBotPersonality(keyval1, keyval2);
            var actual = sut[key2];
            Assert.IsTrue(actual=="c" || actual == "d");
        }
        [Test]
        public void CtorGivenExistingKeyOverwritesInTermMap()
        {
            string key1 = testInfrastructure.random;
            string key2 = testInfrastructure.random;
            while (key1 == key2)
            {
                key2 = testInfrastructure.random;
            }
            var keyval1 = new KeyValuePair<string, IList<string>>(key1, new List<string> { "a", "b" });
            var keyval2 = new KeyValuePair<string, IList<string>>(key2, new List<string> { "c", "d" });
            var keyval3 = new KeyValuePair<string, IList<string>>(key2, new List<string> { "1", "2" });
            var sut = new SlackBotPersonality(keyval1, keyval2, keyval3);
            var actual = sut[key2];
            Assert.IsTrue(actual == "1" || actual == "2");
        }
        [Test]
        public void CtorGivenExistingKeyAndIgnoringCaseOverwritesInTermMap()
        {
            string key1 = testInfrastructure.random;
            string key2 = testInfrastructure.random;
            while (key1 == key2)
            {
                key2 = testInfrastructure.random;
            }
            var keyval1 = new KeyValuePair<string, IList<string>>(key1, new List<string> { "a", "b" });
            var keyval2 = new KeyValuePair<string, IList<string>>(key2, new List<string> { "c", "d" });
            var keyval3 = new KeyValuePair<string, IList<string>>(key2.ToUpper(), new List<string> { "1", "2" });
            var sut = new SlackBotPersonality(keyval1, keyval2, keyval3);
            var actual = sut[key2];
            Assert.IsTrue(actual == "1" || actual == "2");
        }
        [Test]
        public void StringIndexerRandomlySelectsOneFromValuesList()
        {
            string key1 = testInfrastructure.random;
            string key2 = testInfrastructure.random;
            while (key1 == key2)
            {
                key2 = testInfrastructure.random;
            }
            var keyval1 = new KeyValuePair<string, IList<string>>(key1, new List<string> { "a", "b" });
            var keyval2 = new KeyValuePair<string, IList<string>>(key2, new List<string> { "c", "d" });
            var sut = new SlackBotPersonality(keyval1, keyval2);
            var rand = 25;
            var actual = "";
            for (int i = 0; i < rand; i++)
            {
                if (i == rand - 1)
                {
                    Assert.Fail("returned value never changed in "+i.ToString()+" tries");
                    break;
                }
                var current = sut[key2];
                if (current != actual && !string.IsNullOrWhiteSpace(actual))
                {
                    Assert.Pass("Differed after " + i.ToString() +" trials; Current is " + current + " and previous was " + actual);
                    break;
                }
                actual = current;
            }
        }

        [Test]
        public void TalkGivenValidContentReturnsUnchangedContent()
        {
            var rand = testInfrastructure.random;
            Assert.AreEqual(rand, _sut.Talk(rand));
        }
        [Test]
        public void TalkGivenNullContentReturnsEmptyString()
        {
            Assert.AreEqual(string.Empty, _sut.Talk(null));
        }
    }
}
