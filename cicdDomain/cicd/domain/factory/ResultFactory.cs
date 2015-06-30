using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.entity;
using cicdDomain.cicd.infrastructure.dtos;

namespace cicdDomain.cicd.domain.factory
{
  public static class ResultFactory
  {
    public static IDomainResult FailResult(string message)
    {
      return new FailedRequest{ message = message};
    }

    public static IDomainResult getJobResult(Job job)
    {
      string message = string.Empty;
      job.Executions.ToList().ForEach(z => message += z);
      IDomainResult result = job.SuccesffullyRan? 
          new SuccessfulRequest{message = message}
          : FailResult(message)
          ;

      return result;
    }
  }
}
