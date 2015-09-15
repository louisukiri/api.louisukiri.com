using System;
using System.Collections.Generic;
using cicd.domain.context.trigger.entity;
using Newtonsoft.Json;

namespace cicd.infrastructure
{
    public class TestdataPayload
    {
        private string requestBody;
        private Testdata _testdata;
        public Testdata Testdata {
            get { return _testdata; }
        }

        public TestdataPayload(string requestBody)
        {
            if (string.IsNullOrWhiteSpace(requestBody))
            {
                throw new ArgumentException("Request Body");
            }
            this._testdata = JsonConvert.DeserializeObject<Testdata>(requestBody);
            if (!TestdataValid(_testdata))
            {
                throw new ArgumentException("Request Body");
            }
            this.requestBody = requestBody;
        }
        private bool TestdataValid(Testdata testdata)
        {
            if (testdata.data == null || testdata.info == null)
            {
                return false;
            }
            foreach (IList<string> sList in testdata.data)
            {
                for (int i = 0; i < 3; i++)
                {
                    int val = default(int);
                    if (!int.TryParse(sList[i], out val))
                        return false;
                }
            }

            return testdata.info.ContainsKey("GitUrl") && testdata.info.ContainsKey("SourceControlBranch");
        }
    }
}
