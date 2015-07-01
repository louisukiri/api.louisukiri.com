using Moq;
using cicdDomain.cicd.domain.entity;
using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace api.louisukiri.com.Tests.entity
{
    [TestFixture]
    public class JobTest
    {
        Mock<Job> _sut;
        Job Sut
        {
            get { return _sut.Object; }
        }
        [TestFixtureSetUp]
        public void setup()
        {
            _sut = new Mock<Job>();
            _sut.CallBase = true;
        }
        [Test] 
        public void LastExecutionReturnsTheMostRecentExecution()
        {
            DateTime baseDate = DateTime.Now;
            _sut.Setup(z => z.Executions)
                .Returns(new List<Execution> { 
                    new Execution(true, baseDate, new List<string>())
                    , new Execution(false, baseDate.AddMinutes(2), new List<string>())
                    , new Execution(true, baseDate.AddMinutes(-2), new List<string>())
                });

            Assert.AreEqual(3, Sut.Executions.Count);
            Assert.AreEqual(baseDate.AddMinutes(2), Sut.LastExecution.ExecutionDate);
        }
        [Test]
        public void SuccessfullyRanReturnsTrueWhenExecutionsEmpty()
        {
            //_sut.CallBase = true;
            //_sut.Setup(z => z.Executions).Returns(() => { return null; });
            var baseDate = DateTime.Now;
            _sut.Setup(z => z.Executions)
                .Returns(new List<Execution> ());
            Assert.IsTrue(Sut.SuccesffullyRan);
        }
        [Test]
        public void SuccessfullyRanReturnsFalseWhenExecutionsNull()
        {
            //_sut.CallBase = true;
            _sut.Setup(z => z.Executions).Returns(() => { return null; });
            Assert.IsFalse(Sut.SuccesffullyRan);
        }
        [Test]
        public void SuccessfullyRanReturnsTrueWhenAllExecutionsSuccessfull()
        {
            DateTime baseDate = DateTime.Now;
            _sut.Setup(z => z.Executions)
                .Returns(new List<Execution> { 
                    new Execution(true, baseDate, new List<string>())
                    , new Execution(true, baseDate.AddMinutes(2), new List<string>())
                    , new Execution(true, baseDate.AddMinutes(-2), new List<string>())
                });
            //_sut.CallBase = true;
            //_sut.Setup(z => z.Executions).Returns(() => { return null; });
            Assert.IsTrue(Sut.SuccesffullyRan);
        }
        [Test]
        public void SuccessfullyRanReturnsFalseWhenAnyExecutionFaile()
        {
            DateTime baseDate = DateTime.Now;
            _sut.Setup(z => z.Executions)
                .Returns(new List<Execution> { 
                    new Execution(true, baseDate, new List<string>())
                    , new Execution(false, baseDate.AddMinutes(2), new List<string>())
                    , new Execution(true, baseDate.AddMinutes(-2), new List<string>())
                });
            //_sut.CallBase = true;
            //_sut.Setup(z => z.Executions).Returns(() => { return null; });
            Assert.IsFalse(Sut.SuccesffullyRan);
        }
        [Test]
        public void AddingExecutionInsertsIntoArray()
        {
            bool successful=true;
            ICollection<string> Messages = new List<string>();
            int Executions = Sut.Executions.Count;
            Sut.AddRun(successful, Messages);

            Assert.AreEqual(Executions + 1, Sut.Executions.Count);
        }
    }
}
