using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.entity.bot;
using cicd.domain.context.trigger.factory;
using cicd.domain.context.trigger.repository;
using cicd.domain.context.trigger.services.domain;
using cicd.infrastructure;
using Octokit;

namespace cicd.domain.context.trigger.services
{
  public class CICDService
  {
    #region properties
      public IJobRepo JobRepo { get; private set; }
      public IBot CommBot { get; private set; }
      public IBuildService BuildService { get; private set; }
      public IVersionControlService VersionControlService { get; private set; }
      public Settings Settings { get; set; } 
    #endregion
    #region constructors
    public CICDService()
    {
      CommBot = new SlackBot();
      Init(new JobRepository(), new JenkinsBuildService(CommBot), new VcAppService(CommBot), CommBot);
    }
    public CICDService(IJobRepo jobRepo, IBuildService buildService, IVersionControlService vcService = null, IBot communicationBot=null)
    {
        Init(jobRepo, buildService, vcService, communicationBot);
    }

    private void Init(IJobRepo jobRepo, IBuildService buildService, IVersionControlService vcService, IBot communicationBot)
    {
          CommBot = communicationBot;
          JobRepo = jobRepo;
          BuildService = buildService;
          //RequestFactory = _requestFactory ?? new RequestFactory();
          VersionControlService = vcService ?? new VcAppService(CommBot);
      }

      #endregion
    public virtual Job trigger(RequestPayload request)
    {
      if (request == null)
      {
        return JobFactory.FailedJob("invalid request");
      }
      Settings = VersionControlService.GetSettings(request.Branch);
      IList<Job> jobs = Settings.Jobs.Where(z=>
          //z.Level == request.Branch.Level &&
           z.Trigger == request.Trigger
          ).ToList();
      Job lastJob = JobFactory.FailedJob("No valid jobs exists for requested level and trigger");
      foreach (Job job in jobs)
      {
          if (request.Activity.type == RequestTrigger.Pull)
          {
              SetPullRequestStatus(request);
          }
          lastJob = BuildService.Build(job, request.Branch);
      }
      return lastJob;
    }

      private void SetPullRequestStatus(RequestPayload request)
      {
          VersionControlService.SetPullRequestStatus(request.Branch,
              new NewCommitStatus {State = CommitState.Pending});
      }
      public IJobRunResult run(RequestPayload rqPayload)
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
