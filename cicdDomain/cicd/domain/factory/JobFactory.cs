using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cicdDomain.cicd.domain.entity;

namespace cicdDomain.cicd.domain.factory
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
