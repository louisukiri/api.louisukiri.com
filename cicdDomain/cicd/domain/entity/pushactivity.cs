using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cicdDomain.cicd.infrastructure;
using System.Text.RegularExpressions;

namespace cicdDomain.cicd.domain.entity
{
    public class pushactivity
    {
        public string @ref { get; set; }
        public string before { get; set; }
        public string after { get; set; }
        public bool created { get; set; }
        public bool deleted { get; set; }
        public bool forced { get; set; }
        public string base_ref { get; set; }
        public string compare { get; set; }
        public VersionControlUser pusher { get; set; }
        public SourceControlRepository repository { get; set; }

        public RequestTrigger type { 
          get
          {
            if (pusher == null)
            {
              return RequestTrigger.Unknown;
            }
            if (created)
            {
              return RequestTrigger.Branch;
            }
            return RequestTrigger.Push;
          }
        }

        public string Id {
          get
          {
            return 
                Regex.Replace(repository.url, @"\W+", "-")
                .ToLower()
                + "-" + type.ToString().ToLower()
                ;
          }
        }

      private bool isValidBranch
      {
        get
        {
          string [] split = @ref.Split('/');
          return split.Count() == 3 && split[1]=="heads";
        }
      }
      public string Branch
      {
        get
        {
          if (!isValidBranch)
          {
            return string.Empty;
          }
          return @ref.Split('/').Last();
        }
      }
    }
}
