using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.values;

namespace cicd.domain.context.trigger.events
{
    public class DeployEvent : IEvent
    {
        public string Name { get; set; }
        public EventType Type
        {
            get { return EventType.Deploy; }
        }
        public string What { get; set; }
        public string Where { get; set; }
        public string Destination { get; set; }
        public IDictionary<string, string> Results { get; set; }
    }
}
