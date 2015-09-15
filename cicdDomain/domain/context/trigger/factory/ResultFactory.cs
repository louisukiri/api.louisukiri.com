using System.Linq;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.entity;
using cicd.infrastructure.dtos;

namespace cicd.domain.context.trigger.factory
{
  public static class ResultFactory
  {
    public static IJobRunResult FailResult(string message)
    {
      return new FailedRequest{ message = message};
    }

    public static IJobRunResult getJobResult(Job job)
    {
      string message = string.Empty;
      job.Executions.ToList().ForEach(z => message += z);
      IJobRunResult result = job.SuccesffullyRan? 
          new SuccessfulRequest{message = message}
          : FailResult(message)
          ;

      return result;
    }
  }
}
