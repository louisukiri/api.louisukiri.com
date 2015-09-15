using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cicd.domain.context.trigger.abstracts;

namespace cicd.domain.context.trigger.events
{
    public class GenericEvent : IEvent
    {
        public string Name { get; set; }

        public virtual values.EventType Type
        {
            get;
            set;
        }

        public virtual string What
        {
            get;
            set;
        }

        public virtual string Where
        {
            get;
            set;
        }

        public virtual IDictionary<string, string> Results
        {
            get;
            set;
        }

        public string Destination
        {
            get;
            set;
        }
    }
}
