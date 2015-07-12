﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cicdDomain.cicd.domain.entity
{
    //TODO: Consider adding a constructor that sets defaults on master_branch. This will remove null checks from pushactivity class
    public class SourceControlRepository
    {
      public string id { get; set; }
      public string name { get; set; }
      public string full_name { get; set; }
      public string url { get; set; }
      public string clone_url { get; set; }
      public string master_branch { get; set; }
    }
}
