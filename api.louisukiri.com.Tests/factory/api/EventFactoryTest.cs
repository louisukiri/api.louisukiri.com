using System.Collections;
using System.Collections.Generic;
using cicd.domain.context.trigger.events;
using cicd.domain.context.trigger.values;
using cicdDomain;
using godaddy.domains.cicd.Factory;
using godaddy.domains.cicd.Models;
using NUnit.Framework;

namespace godaddy.domains.cicd.Tests.factory.api
{
    [TestFixture]
    public class EventFactoryTest
    {
        [Test]
        public void GenericApiConvRetsDeployedEventGivenTypeDeploy()
        {
            var result = EventFactory.FromGenericApiEvent(testInfrastructure.GenericApiDeployEvent);
            Assert.IsInstanceOf(typeof(DeployEvent), result);
        }
        [Test]
        public void GenericApiConvGivenDeployedEventRetsExpectedWhat()
        {
            var genEvent = testInfrastructure.GenericApiDeployEvent;
            var result = EventFactory.FromGenericApiEvent(genEvent);

            Assert.AreEqual(genEvent.What, result.What);
        }
        [Test]
        public void GenericApiConvGivenDeployedEventRetsExpectedWhere()
        {
            var genEvent = testInfrastructure.GenericApiDeployEvent;
            var result = EventFactory.FromGenericApiEvent(genEvent);

            Assert.AreEqual(genEvent.Where, result.Where);
        }

        public IEnumerable<TestCaseData> GetTypeData
        {
            get
            {
                var ev = testInfrastructure.GenericApiDeployEvent;
                var pretestEv = ev.Copy();
                pretestEv.Type = "pretest";
                yield return new TestCaseData(ev, EventType.Deploy).SetName("DeployReturnsExpectedType");
                
                yield return new TestCaseData(pretestEv,EventType.PreTest).SetName("PreTestReturnsExpectedType");
            }
        }

        [TestCaseSource("GetTypeData")]
        public void GenericApiConvGivenDeployedEventRetsExpectedType(GenericApiEvent Event, EventType expected)
        {
            var result = EventFactory.FromGenericApiEvent(Event);
            Assert.AreEqual(expected, result.Type);
        }
        public IEnumerable<TestCaseData> GetNameData
        {
            get
            {
                var ev = testInfrastructure.GenericApiDeployEvent;
                yield return new TestCaseData(ev).SetName("DeployReturnsExpectedName");
                yield return new TestCaseData(ev).SetName("PreTestReturnsExpectedName");
            }
        }
        [TestCaseSource("GetNameData")]
        public void GenericApiConvGivenDeployedEventRetsExpectedName(GenericApiEvent Event)
        {
            var result = EventFactory.FromGenericApiEvent(Event);

            Assert.AreEqual(Event.Name, result.Name);
        }
        [Test]
        public void GenericApiConvGivenUnknownTypeRetsNull()
        {
            var genEvent = testInfrastructure.GenericApiDeployEvent;
            genEvent.Type = testInfrastructure.random;
            var result = EventFactory.FromGenericApiEvent(genEvent);

            Assert.IsNull(result);
        }
    }

    public static class GenericEventExtension
    {
        public static GenericApiEvent Copy(this GenericApiEvent Event)
        {
            return new GenericApiEvent
            {
                Name = Event.Name
                ,Destination = Event.Destination
                ,Results = Event.Results
                ,Type = Event.Type
                ,What = Event.What
                ,Where = Event.Where
            };
        }
    }
}
