using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cicdDomain.cicd.domain.abstracts;

namespace cicdDomain.cicd.infrastructure.dtos
{
  public class DomainResult : IDomainResult
  {
    public string message { get; set; }


    public virtual bool Failed {
      get { return false; }
    }
  }
  public class FailedRequest : DomainResult
  {
    public override bool Failed { get{ return true; }}
  }

  public class SuccessfulRequest : DomainResult
  {
    
  }
}
