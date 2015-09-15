using System;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.factory;
using cicdDomain;
using System.Collections.Generic;
using Octokit;
using NUnit.Framework;

namespace api.louisukiri.com.Tests.factory
{
    [TestFixture]
    public class CommitStateFactoryTest
    {
        [Test]
        public void GetStateGivenTestDataWithErrorReturnFail()
        {
            Testdata input = testInfrastructure.TestdataWithError;
            NewCommitStatus status = CommitStateFactory.GetStateFrom(input);
            Assert.AreEqual(CommitState.Failure, status.State);
        }
        [Test]
        public void GetStateGivenTestDataWithoutErrorsReturnSuccess()
        {
            Testdata input = testInfrastructure.Testdata;
            NewCommitStatus status = CommitStateFactory.GetStateFrom(input);
            Assert.AreEqual(CommitState.Success, status.State);
        }
    }
}
