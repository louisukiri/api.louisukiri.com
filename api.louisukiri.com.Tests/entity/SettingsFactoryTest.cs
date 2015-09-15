using System;
using System.Collections.Generic;
using System.Linq;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.factory;
using cicdDomain;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace godaddy.domains.cicd.Tests.entity
{
    [TestFixture]
    public class SettingsFactoryTest
    {
        [Test]
        public void GivenInvalidJsonReturnsNull()
        {
            const string invalidJson = "I am not JSON";
            var result = SettingsFactory.SettingsFromJson(invalidJson);
            Assert.IsNull(result);
        }
        [Test]
        public void GivenValidJsonReturnsJobObject()
        {
            var jsonStringFromSut = JsonConvert.SerializeObject(Sut);
            var result = SettingsFactory.SettingsFromJson(jsonStringFromSut);
            Assert.IsNotNull(result);
        }
        private Settings Sut
        {
            get
            {
                var newSettings = new Settings { Branch = new BranchSetting { BaseBranch = "testbasebranch", Level = "testLevel" } };
                newSettings.Jobs = new List<Job>
                {
                    new Job
                    {
                        Parameters = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("test1",@"\\abc\\ad"),
                            new KeyValuePair<string, string>("testValue",@"another test"),
                            new KeyValuePair<string, string>("test2",@"\\\\zyx\\wv\\u")
                        }
                        ,Executions = new List<Execution> { 
                        new Execution(true, DateTime.Now, new List<string>{ "testMessage"})
                    }
                    }
                };
                return newSettings;
            }
        }
        //[Test] 
        //public void AnyParametersWithBackSlash()
        //{
        //    DateTime baseDate = DateTime.Now;
        //    _sut.Setup(z => z.Executions)
        //        .Returns(new List<Execution> { 
        //            new Execution(true, baseDate, new List<string>())
        //            , new Execution(false, baseDate.AddMinutes(2), new List<string>())
        //            , new Execution(true, baseDate.AddMinutes(-2), new List<string>())
        //        });

        //    Assert.AreEqual(3, Sut.Executions.Count);
        //    Assert.AreEqual(baseDate.AddMinutes(2), Sut.LastExecution.ExecutionDate);
        //}
    }
}
