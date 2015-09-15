using System;
using cicd.infrastructure;
using cicdDomain;
using NUnit.Framework;

namespace api.louisukiri.com.Tests.infrastructure
{
    [TestFixture]
    public class TestdataPayloadTest
    {
        private TestdataPayload sut;
        [TestFixtureSetUp]
        public void setup()
        {
            
        }

        [Test]
        public void TestdataPayloadCTORThrowsGivenEmptyPayload()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var a = new TestdataPayload(string.Empty);
            });
        }
        [Test]
        public void TestdataPayloadCTORThrowsGivenNullPayload()
        {
            Assert.Throws<ArgumentException>(() =>
            {
               var a = new TestdataPayload(null);
            });
        }
        [Test]
        public void TestdataPayloadCTORSetsPropertyGivenValidParam()
        {
            TestdataPayload sut = new TestdataPayload(testInfrastructure.TestdataContent);
            Assert.IsNotNull(sut.Testdata);
        }
        [Test]
        public void TestdataPayloadCTORGivenValidParamSetsExpectedData()
        {
            TestdataPayload sut = new TestdataPayload(testInfrastructure.TestdataContent);

            Assert.IsNotNull(sut.Testdata.data);
            Assert.AreEqual("6", sut.Testdata.data[0][2]);
            Assert.AreEqual("64", sut.Testdata.data[1][2]);
        }
        [Test]
        public void TestdataPayloadCTORGivenValidParamSetsExpectedInfo()
        {
            TestdataPayload sut = new TestdataPayload(testInfrastructure.TestdataContent);

            Assert.IsNotNull(sut.Testdata.info);
            Assert.AreEqual("PleaseUpdate", sut.Testdata.info["ProjectName"]);
        }
        [Test]
        public void TestdataPayloadCTORGivenInvalidPayloadThrow()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var a = new TestdataPayload("{'ok jim':'bad json'}");
            });
        }
        [Test]
        public void TestdataPayloadCTORGivenInvalidTypesInPayloadReturnException()
        {
            //if any of the first three entries in payload.data arrays isn't a number
            //it's an error
            string payload = @"
{
    'data':  [
                 [
                     '0',
                     'a',
                     '6',
                     'BusinessObjects.DB.Tests-nunitTestResults.xml'
                 ],
                 [
                     '0',
                     '0',
                     '64',
                     'BusinessObjects.Domains.Tests-nunitTestResults.xml'
                 ]
             ],
    'info':  {
                 'BuildNumber':  '27',
                 'ProjectName':  'PleaseUpdate',
                 'SourceControlBranch':  'DOMWARGS-3452-1',
                 'DeployEnvironment':  'dev',
                 'JobName':  'DOM-Sites-DCC50-Dev-CD'
             }
} 
";
            Assert.Throws<ArgumentException>(() =>
            {
                var a = new TestdataPayload(payload);
            });
        }
        [Test]
        public void TestdataPayloadCTORGivenInfoWithoutGitUrlThrows()
        {
            //if any of the first three entries in payload.data arrays isn't a number
            //it's an error
            string payload = @"
{
    'data':  [
                 [
                     '0',
                     '0',
                     '6',
                     'BusinessObjects.DB.Tests-nunitTestResults.xml'
                 ]
             ],
    'info':  {
                 'BuildNumber':  '27',
                 'ProjectName':  'PleaseUpdate',
                 'SourceControlBranch':  'DOMWARGS-3452-1',
                 'DeployEnvironment':  'dev',
                 'JobName':  'DOM-Sites-DCC50-Dev-CD'
             }
} 
";
            Assert.Throws<ArgumentException>(() =>
            {
                var a= new TestdataPayload(payload);
            });
        }
        [Test]
        public void TestdataPayloadCTORGivenInfoWithoutBranchThrows()
        {
            //if any of the first three entries in payload.data arrays isn't a number
            //it's an error
            string payload = @"
{
    'data':  [
             ],
    'info':  {
                 'BuildNumber':  '27',
                 'ProjectName':  'PleaseUpdate',
                 'DeployEnvironment':  'dev',
                 'JobName':  'DOM-Sites-DCC50-Dev-CD',
                 'GitUrl': 'okayJim'
             }
} 
";
            Assert.Throws<ArgumentException>(() =>
            {
                var a = new TestdataPayload(payload);
            });
        }
    }
}