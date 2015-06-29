﻿using System;
using System.Collections.Generic;
using cicdDomain;
using cicdDomain.cicd.domain.entity;
using cicdDomain.cicd.domain.factory;
using cicdDomain.cicd.infrastructure.dtos;
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
    public void JobResultReturnsSuccesfulJobWithMessage()
    {
      Job job = testInfrastructure.getJob();
      job.Executions.Add(new Execution(true, DateTime.Now, new List<string>{"test successmessage"} ));

      var result = ResultFactory.getJobResult(job);
      Assert.IsInstanceOf<SuccessfulRequest>(result);
      Assert.AreNotEqual(string.Empty, result.message);
    }
  }
}