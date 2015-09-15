using System.Linq;
using cicd.domain.context.trigger.@class;
using cicd.infrastructure;
using cicdDomain;
using NUnit.Core;
using NUnit.Framework;

namespace api.louisukiri.com.Tests.entity
{
    [TestFixture]
    public class NUnitTestResultTest
    {
        private TestdataPayload payload;
        private NUnitTestResult sut;
        [SetUp]
        public void Setup()
        {
            payload = testInfrastructure.TestDataValidErrorPayload;
            sut = NUnitTestResult.CreateFrom(payload);
        }
        [Test]
        public void ConvertingGivenNullPayloadReturnNull()
        {
            var result = NUnitTestResult.CreateFrom(null);
            Assert.IsNull(result);
        }
        [Test]
        public void ConvertingGivenPayloadReturnExpectedError()
        {
            Assert.AreEqual(1, sut.Stats.First().Errors);
            Assert.AreEqual(0, sut.Stats.ElementAt(1).Errors);

        }
        [Test]
        public void ConvertingGivenPayloadReturnExpectedFailure()
        {
            Assert.AreEqual(5, sut.Stats.First().Failures);
            Assert.AreEqual(11, sut.Stats.ElementAt(2).Failures);

        }
        [Test]
        public void ConvertingGivenPayloadReturnExpectedTotals()
        {
            Assert.AreEqual(6, sut.Stats.First().Totals);
            Assert.AreEqual(0, sut.Stats.ElementAt(2).Totals);

        }
        [Test]
        public void ConvertingGivenPayloadReturnExpectedName()
        {
            Assert.AreEqual("BusinessObjects.DB.Tests-nunitTestResults", sut.Stats.First().Name);
            Assert.AreEqual("BusinessObjects.Domains.Tests-nunitTestResults", sut.Stats.ElementAt(1).Name);
        }
        [Test]
        public void ConvertingGivenPayloadReturnExpectedBranch()
        {
            Assert.IsNotNull(sut.Branch);
        }
        [Test]
        public void ConvertingGivenPayloadReturnExpectedBuildNumber()
        {
            Assert.AreEqual(27, sut.BuildNumber);
        }
        [Test]
        public void ConvertingGivenPayloadReturnExpectedProjectName()
        {
            Assert.AreEqual("PleaseUpdate", sut.ProjectName);
        }
        [Test]
        public void ConvertingGivenPayloadReturnExpectedJobName()
        {
            Assert.AreEqual("DOM-Sites-DCC50-Dev-CD", sut.JobName);
        }
    }
}
