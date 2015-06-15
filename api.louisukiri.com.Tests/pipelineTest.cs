using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using api.louisukiri.com.Tests.helpers;

namespace api.louisukiri.com.Tests
{
    [TestClass]
    public class pipelineTest
    {
        [TestMethod]
        public void callsPushMethodWhenPushJsonInBody()
        {
            var t = new virtualServer();

            var res = t.getAPIResponse("/index/5");
        }
    }
}
