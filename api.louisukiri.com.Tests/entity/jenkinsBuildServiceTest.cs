﻿using System.Collections.Generic;
using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.entity;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;


namespace api.louisukiri.com.Tests.entity
{
  [TestFixture]
  public class JenkinsBuildServiceTest
  {
    public Mock<JenkinsBuildService> _sut;

    public JenkinsBuildService sut
    {
      get { return _sut.Object; }
    }
    [TestFixtureSetUp]
    public void setup()
    {
      _sut = new Mock<JenkinsBuildService>();
    }
    [Test]
    public void Given200ResponseAddSuccessfulLastExecution()
    {
        Job testJob = new Job();
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
            .Returns(
            new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            }
            );

        var res = sut.build(testJob);
        Assert.IsTrue(res.Executions.Count > 0);
        Assert.IsTrue(res.SuccesffullyRan);
    }
    [Test]
    public void ExecutionsIsInitializedToEmptyCollectino()
    {
        Job testJob = new Job();
        Assert.IsNotNull(testJob.Executions);
    
    }
    [Test]
    public void GivenNon200ResponseBuildAddFailedLastExecution()
    {
        Job testJob = new Job();
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
            .Returns(
            new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.NotFound
            }
            );
        int Executions = testJob.Executions.Count;
        var res = sut.build(testJob);
        Assert.AreEqual(Executions + 1, testJob.Executions.Count);
        Assert.IsFalse(res.SuccesffullyRan);
    }
    [Test]
    public void GivenExceptionAddFailedLastExecution()
    {
        Job testJob = new Job();
        _sut.Setup(z => z.trigger(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>()))
            .Returns((string a)=>{
                throw new Exception("error");
                }
            );
        int Executions = testJob.Executions.Count;
        var res = sut.build(testJob);
        Assert.AreEqual(Executions + 1, testJob.Executions.Count);
        Assert.IsFalse(res.SuccesffullyRan);
    }
  }
}
