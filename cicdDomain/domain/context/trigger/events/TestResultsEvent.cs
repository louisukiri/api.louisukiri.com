using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.@class;
using cicd.domain.context.trigger.values;
using cicd.infrastructure;
using Octokit;

namespace cicd.domain.context.trigger.events
{
    public class TestResultsEvent:IEvent
    {
        private readonly NUnitTestResult _testResult;
        private readonly IDictionary<string, string> _results;  
        public TestResultsEvent(NUnitTestResult testResult)
        {
            Name = testResult.Branch.Name;
            _testResult = testResult;
            _results = new Dictionary<string, string>();
            foreach (var stats in testResult.Stats)
            {
                Results.Add(stats.Name + " *Errors* ", stats.Errors.ToString());
                Results.Add(stats.Name + " *Failures* ", stats.Failures.ToString());
                Results.Add(stats.Name + " *Totals* ", stats.Totals.ToString());
                //Results.Add(stats.Name + " Name", stats.Name);
            }
        }
        public string Name { get; private set; }
        public string What { get { return _testResult.Branch.Name; } }
        public EventType Type
        {
            get { return EventType.TestResults; }
        }
        public string Where
        {
            get { return string.Empty; }
        }
        public IDictionary<string, string> Results
        {
            get { return _results; }
        }


        public string Destination
        {
            get { return string.Empty; }
        }
    }
}
