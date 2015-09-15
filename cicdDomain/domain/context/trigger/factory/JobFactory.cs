using System.Collections.Generic;
using cicd.domain.context.trigger.entity;

namespace cicd.domain.context.trigger.factory
{
  public static class JobFactory
  {
     
    public static Job FailedJob(string Message)
    {
      Job job = new Job();
      job.AddRun(false, new List<string>{ Message});

      return job; 
    }
    public static Job SuccessfulJob(string Message="")
    {
      Job job = new Job();
      job.AddRun(true, new List<string>{Message});

      return job;
    }
  }
}
