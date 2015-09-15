using cicdDomain.cicd.domain.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cicdDomain.cicd.domain.abstracts
{
    public interface IBuildService
    {
        Job buildSeed(Job job, pushactivity activity);
        Job buildPush(Job job, pushactivity activity);
    }
}
