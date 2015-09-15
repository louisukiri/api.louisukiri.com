using System;
using System.Collections.Generic;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.factory;
using cicd.infrastructure.dtos;
using cicdDomain;
using NUnit.Framework;

namespace api.louisukiri.com.Tests.factory
{
  [TestFixture]
  public class ResultFactoryTest
  {
    [Test]
    public void FailResultReturnsFailedTestWithMessage()
    {
      var result = ResultFactory.FailResult("test");
      Assert.IsInstanceOf<FailedRequest>(result);
      Assert.AreEqual("test", result.message);
    }
    [Test]
    public void SuccessfulJobReturnsDomainRequestWithMessage()
    {
        Job job = testInfrastructure.getJob();
        job.Executions.Add(new Execution(true, DateTime.Now, new List<string> { "test successmessage" }));

        var result = ResultFactory.getJobResult(job);
        Assert.IsInstanceOf<SuccessfulRequest>(result);
        Assert.AreNotEqual(string.Empty, result.message);
    }
    [Test]
    public void FailedJobReturnsFailedRequestWithMessage()
    {
        Job job = testInfrastructure.getJob(false);
        job.Executions.Add(new Execution(true, DateTime.Now, new List<string> { "test successmessage" }));

        var result = ResultFactory.getJobResult(job);
        Assert.IsInstanceOf<FailedRequest>(result);
        Assert.AreNotEqual(string.Empty, result.message);
    }
  }
}
