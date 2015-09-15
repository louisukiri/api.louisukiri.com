using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cicd.domain.context.trigger.entity
{

    public class BranchSetting
    {
        public string Level { get; set; }
        public string BaseBranch { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
    public class Settings
    {
        public BranchSetting Branch { get; set; }
        public IList<Job> Jobs { get; set; }

        public Settings()
        {
            Jobs = new List<Job>();
        }
        public static bool operator ==(Settings a, Settings b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if ((object) a == null || (object) b == null)
            {
                return false;
            }
            return a.Branch.Level==b.Branch.Level
                && a.Branch.BaseBranch==b.Branch.BaseBranch
                ;
        }
        public static bool operator !=(Settings a, Settings b)
        {
            return !(a==b);
        }
        public override int GetHashCode()
        {
            return this.Branch.Level.GetHashCode() ^ this.Branch.BaseBranch.GetHashCode();
        }

        public bool Equals(Settings obj)
        {
            if (obj == null)
            {
                return false;
            }

            return this.Branch.Level == obj.Branch.Level
                && Branch.BaseBranch == obj.Branch.BaseBranch
                ;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var settings = obj as Settings;
            if (settings == null)
            {
                return false;
            }
            return this.Branch.Level == settings.Branch.Level
                && Branch.BaseBranch== settings.Branch.BaseBranch
                ;
        }
    }
}
