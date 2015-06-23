using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cicd.domain.cicd.domain.abstracts;

namespace cicdDomain.cicd.domain.entity
{
  public class JenkinsBuildServer: IBuildServer
  {
    public void buildJob(string name)
    {
      throw new NotImplementedException();
    }
  }
}
