using System;
using System.Collections;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.entity.bot;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace api.louisukiri.com.Tests.entity
{
    [TestFixture]
    public class SlackBotTest
    {
        private SlackBot _sut;
        [SetUp]
        public void Setup()
        {
            _sut = new SlackBot();
        }
        [Test]
        public void RequiresBrainWithDefaultCtor()
        {
            Assert.IsNotNull(_sut.Brain);
        }
        [Test]
        public void RequiresBrainGivenIBrainInCtor()
        {
            var brainMock = new Mock<IBrain>();
            var sut = new SlackBot(brainMock.Object);

            Assert.IsNotNull(sut.Brain);
        }
        #region trigger Tests
        [Test]
        public void TriggerGivenNullEventReturnException()
        {
            Assert.Throws<ArgumentException>(() => _sut.Trigger(null));
        }
        #endregion
    }
}
