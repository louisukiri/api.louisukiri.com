using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace cicdDomain.cicd.domain.abstracts
{
  public interface IBuildServerRest
  {
    HttpResponseMessage trigger(string p);
  }
}
