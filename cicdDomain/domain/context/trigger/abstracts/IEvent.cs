using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cicd.domain.context.trigger.abstracts
{
    public interface IEvent
    {
        string Name { get; }
        values.EventType Type { get;}
        string What { get; }
        string Where { get; }
        IDictionary<string,string> Results { get; }
        string Destination { get;}
    }
}
