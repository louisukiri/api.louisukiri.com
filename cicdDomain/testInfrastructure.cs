using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cicdDomain
{
  public class testInfrastructure
  {
    public static string random
    {
      get { return Guid.NewGuid().ToString(); }
    }
  }
}
