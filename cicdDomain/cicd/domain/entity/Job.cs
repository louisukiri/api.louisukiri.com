using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cicdDomain.cicd.domain.entity
{
    public class Execution
    {
        public bool isSuccessful { get; private set; }
        public ICollection<string> Messages { get; private set; }
        public DateTime ExecutionDate { get; private set; }
        public Execution(bool _success, DateTime _executionDate, ICollection<string> _messages)
        {
            isSuccessful = _success;
            Messages = _messages;
            ExecutionDate = _executionDate;
        }
    }
    public class Job
    {
        public virtual ICollection<Execution> Executions { get; set; }
        public virtual string name { get; set; }
        public virtual string uri { get; set; }
        public virtual string path { get; set; }
        public Job()
        {
            Executions = new List<Execution>();
        }
        
        public virtual Execution LastExecution
        {
            get
            {
                if(Executions == null)
                {
                    return null;
                }

                return Executions.OrderByDescending(z=> z.ExecutionDate).First();
            }
        }

        public bool SuccesffullyRan { 
            get 
            {
                if(Executions!= null 
                   && (Executions.Count == 0
                    || !Executions.Any(z=> !z.isSuccessful))
                    
                    )
                {
                    return true;
                }
                return false;
            } 
        }

        public void AddRun(bool successful, ICollection<string> Messages)
        {
            this.Executions.Add(new Execution(successful, DateTime.Now, Messages));
        }

        public List<KeyValuePair<string, string>> parameters { get; set; }
    }
}
