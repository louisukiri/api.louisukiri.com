using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cicd.domain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.entity;
using cicdDomain.cicd.domain.factory;
using cicdDomain.cicd.domain.repository;
using cicdDomain.cicd.infrastructure;
using cicdDomain.cicd.infrastructure.dtos;

namespace cicdDomain.cicd.domain.service
{
  public class CICDService
  {
    #region properties
      private IBuildServer buildServer;
      public IJobRepo JobRepo { get; private set; }
      public IBuildService BuildService { get; private set; }
      public IRequestFactory RequestFactory { get; set; }
      
    #endregion
    #region constructors
    public CICDService()
      : this(new JobRepository(), new JenkinsBuildService(), new RequestFactory())
    {
      
    }
    public CICDService(IJobRepo _jobRepo, IBuildService _buildService, IRequestFactory _requestFactory = null)
    {
        JobRepo = _jobRepo;
        BuildService = _buildService;
        RequestFactory = _requestFactory ?? new RequestFactory();
    }
    public CICDService(IBuildServer _buildServer, IRequestFactory _requestFactory = null)
    {
      buildServer = _buildServer;
      RequestFactory = _requestFactory ??  new RequestFactory();
    }
    #endregion
    public void send(string name)
    {
      buildServer.buildJob(name);
    }
    public virtual Job trigger(DomainRequest request)
    {
      if (request == null)
      {
        return JobFactory.FailedJob("invalid request");
      }
      Job job = JobRepo.getJobBy(request.jobId);
      if(!job.SuccesffullyRan)
      {
          return job;
      }
      return BuildService.build(job);
    }

    public IDomainResult run(RequestPayload rqPayload)
    {
      if (rqPayload == null)
      {
        ResultFactory.FailResult("invalid payload");
      }
      DomainRequest request = RequestFactory.getRequestFrom(rqPayload);
      if (request == null)
      {
        ResultFactory.FailResult("invalid request");
      }
      var result = trigger(request);
      if (!result.SuccesffullyRan)
      {
        return ResultFactory.FailResult("invalid job");
      }
      return ResultFactory.getJobResult(result);
    }

  }
}
