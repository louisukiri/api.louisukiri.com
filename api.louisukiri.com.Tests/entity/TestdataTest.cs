using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cicd.domain.context.trigger.entity;
using NUnit.Framework;
using cicdDomain;

namespace api.louisukiri.com.Tests.entity
{
    [TestFixture]
    public class TestdataTest
    {
        [Test]
        public void HasErrorsReturnsTrueIfAnyErrorBitNot0()
        {
            Testdata sut = testInfrastructure.TestdataWithError;

            Assert.IsTrue(sut.HasErrors);
        }
        [Test]
        public void HasErrorsReturnsFalseIfAllErrorBits0()
        {
            Testdata sut = testInfrastructure.Testdata;

            Assert.IsFalse(sut.HasErrors);
        }
    }
}
