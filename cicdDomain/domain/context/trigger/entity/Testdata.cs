using System.Collections.Generic;
using System.Linq;

namespace cicd.domain.context.trigger.entity
{
    public class Testdata
    {
        public IList<string>[] data { get; set; }
        public IDictionary<string, string> info { get; set; }

        public bool HasErrors {
            get
            {
                return data.Any(z => int.Parse(z[0]) > 0);
            }
        }

        public string GitUrl {
            get { return info.ContainsKey("GitUrl") ? info["GitUrl"] : string.Empty; }
        }

        public string SourceControlBranch {
            get { return info.ContainsKey("SourceControlBranch") ? info["SourceControlBranch"] : string.Empty; }
        }
    }
}
