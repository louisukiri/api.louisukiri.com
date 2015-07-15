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
    public virtual Job trigger(RequestPayload request)
    {
      if (request == null)
      {
        return JobFactory.FailedJob("invalid request");
      }
      Job job = JobRepo.getJobBy(request.Activity.Id);
      if(!job.SuccesffullyRan)
      {
          return job;
      }
        if (request.Activity.type == RequestTrigger.Branch)
        {
            return BuildService.buildSeed(job, request.Activity);
        }
        return BuildService.buildPush(job, request.Activity);
    }

    public IDomainResult run(RequestPayload rqPayload)
    {
      if (rqPayload == null)
      {
        return ResultFactory.FailResult("invalid payload");
      }
      var result = trigger(rqPayload);
      if (!result.SuccesffullyRan)
      {
        return ResultFactory.FailResult(result.LastExecution != null &&
            result.LastExecution.Messages.Any()?
            result.LastExecution.Messages.First()
            : "Unknown Error"
            );
      }
      return ResultFactory.getJobResult(result);
    }

  }
}
