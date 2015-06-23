using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cicd.domain.cicd.domain.abstracts
{
    public interface IBuildServer
    {
      void buildJob(string name);
    }
}
