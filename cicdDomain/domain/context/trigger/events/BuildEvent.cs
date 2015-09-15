using System;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.values;

namespace cicd.domain.context.trigger.events
{
    public class BuildEvent : IEvent
    {
        private readonly Job _job;

        public BuildEvent(Job job, Branch branch)
        {
            _job = job;
            _name = "Branch " + branch.Name;
            
        }
        private readonly string _name = "build";
        public string Name
        {
            get { return  _name; }
        }

        public string What {
            get { return Name; }
        }

        public EventType Type
        {
            get { return EventType.Build; }
        }
        public string Where
        {
            get { return _job.FullPath; }
        }


        public System.Collections.Generic.IDictionary<string, string> Results
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        public string Destination
        {
            get { return string.Empty; }
        }
    }
}
