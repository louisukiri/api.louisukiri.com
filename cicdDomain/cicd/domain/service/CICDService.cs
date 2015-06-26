using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cicd.domain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.infrastructure.dtos;

namespace cicdDomain.cicd.domain.service
{
  public class CICDService
  {
    #region properties
      private IBuildServer buildServer;
      public IJobRepo JobRepo { get; private set; }
      public IBuildService BuildService { get; private set; }
    #endregion

    public CICDService(IJobRepo _jobRepo, IBuildService _buildService)
    {
        JobRepo = _jobRepo;
        BuildService = _buildService;
    }
    public CICDService(IBuildServer _buildServer)
    {
      buildServer = _buildServer;
    }
    public void send(string name)
    {
      buildServer.buildJob(name);
    }
    public TriggerResult trigger()
    {
        return new TriggerResult { wasSuccessful=true };
    }
  }
}
