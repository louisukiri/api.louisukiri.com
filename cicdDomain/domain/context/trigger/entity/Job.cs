using System;
using System.Collections.Generic;
using System.Linq;
using cicd.infrastructure;

namespace cicd.domain.context.trigger.entity
{
    public class Execution
    {
        public bool IsSuccessful { get; private set; }
        public ICollection<string> Messages { get; private set; }
        public DateTime ExecutionDate { get; private set; }
        public Execution(bool success, DateTime executionDate, ICollection<string> messages)
        {
            IsSuccessful = success;
            Messages = messages;
            ExecutionDate = executionDate;
        }
    }
    public class Job
    {
        public virtual ICollection<Execution> Executions { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public string Path { get; set; }
        public string FullPath { get { return Uri + Path; } }
        public List<KeyValuePair<string, string>> Parameters { get; set; }
        public string AuthToken { get; set; }
        public BranchLevel Level { get; set; }
        public RequestTrigger Trigger { get; set; }

        public Job()
        {
            Executions = new List<Execution>();
            Parameters = new List<KeyValuePair<string, string>>();
        }
        
        public virtual Execution LastExecution
        {
            get
            {
                return Executions == null ? null : Executions.OrderByDescending(z=> z.ExecutionDate).First();
            }
        }

        public bool SuccesffullyRan { 
            get
            {
                return Executions!= null 
                       && (Executions.Count == 0
                           || Executions.All(z => z.IsSuccessful));
            }
        }

        public void AddRun(bool successful, ICollection<string> messages)
        {
            Executions.Add(new Execution(successful, DateTime.Now, messages));
        }

    }
}
