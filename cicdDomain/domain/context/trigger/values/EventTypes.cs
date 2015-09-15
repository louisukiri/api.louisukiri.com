using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cicd.domain.context.trigger.values
{
    public enum EventType
    {
        Unknown=0,
        Build = 1,
        TestResults=2,
        Deploy = 3,
        PreTest=4,
        PreBuild=5,
        PreDeploy=6,
        PostTest=7,
        PostBuild=8,
        PostDeploy=9,
        BuildResults=10
    }
}
