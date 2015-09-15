using System;
using System.Collections.Generic;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.values;

namespace godaddy.domains.cicd.Tests.helpers
{

    public class TestEvent : IEvent
    {
        public TestEvent(string Name="", EventType type = default(EventType), string What="", string where="")
        {
            this.Name = Name;
            this.Type = type;
            this.What = What;
            this.Where = Where;
        }

        public string Name { get; set; }

        public EventType Type { get; set; }

        public string What { get; set; }

        public string Where { get; set; }

        public IDictionary<string, string> Results { get; set; }


        public string Destination { get; set; }
    }
}
