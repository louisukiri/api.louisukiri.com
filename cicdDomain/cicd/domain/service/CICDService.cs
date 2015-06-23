using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cicd.domain.cicd.domain.abstracts;

namespace cicdDomain.cicd.domain.service
{
  public class CICDService
  {
    private IBuildServer buildServer;
    public CICDService(IBuildServer _buildServer)
    {
      buildServer = _buildServer;
    }
    public void send(string name)
    {
      buildServer.buildJob(name);
    }
  }
}
