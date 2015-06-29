using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cicdDomain.cicd.domain.abstracts
{
  public interface IDomainResult
  {
    string message { get; set; }
    bool Failed { get;}
  }
}
