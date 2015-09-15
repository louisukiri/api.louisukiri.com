using System.Linq;
using cicd.domain.context.trigger.entity;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using cicd.infrastructure;

namespace cicd.domain.context.trigger.@class
{
    public struct TestStats
    {
        public int Totals { get; private set; }
        public int Errors { get; private set; }
        public int Failures { get; private set; }
        public string Name { get; private set; }

        public TestStats(int totals, int errors, int failures, string name = null)
        : this()
        {
            Totals = totals;
            Errors = errors;
            Failures = failures;
            Name = name;
        }
        
    }
    public class NUnitTestResult
    {
        private IList<TestStats> _statsCollection;
        public IReadOnlyCollection<TestStats> Stats {
            get
            {
                return  new ReadOnlyCollection<TestStats>(_statsCollection);
            }
        }
        public string ProjectName { get; private set; }
        public Branch Branch { get; private set; }
        //public string Environment { get; private set; }
        public int BuildNumber { get; private set; }
        public string JobName { get; private set; }
        private NUnitTestResult()
        {
            _statsCollection = new List<TestStats>();
        }
        public static NUnitTestResult CreateFrom(TestdataPayload payload)
        {
            if (payload == null)
            {
                return null;
            }
            var result = new NUnitTestResult();
            for (int i = 0; i < payload.Testdata.data.Count(); i++)
            {
                var currVal = payload.Testdata.data[i];
                result._statsCollection.Add(new TestStats(
                   int.Parse(currVal[0]),
                   int.Parse(currVal[1]),
                   int.Parse(currVal[2]),
                   currVal[3].Replace(".xml","")
                   ));
                result.Branch = entity.Branch.CreateFrom(payload);
                result.BuildNumber = int.Parse(payload.Testdata.info["BuildNumber"]);
                result.ProjectName = payload.Testdata.info["ProjectName"];
                result.JobName = payload.Testdata.info["JobName"];
            }
            return result;
        }
    }
}
