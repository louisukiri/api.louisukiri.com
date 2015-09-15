using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cicd.infrastructure;

namespace cicd.domain.context.trigger.entity
{
    public enum BranchLevel
    {
        FeatureBranch=1
        ,VersionBranch=2
        ,Master=3
        ,Unknown
    }
    public class Branch
    {
        public string Name { get; protected set; }
        public string Parent { get; protected set; }
        public VerControlServer Server { get; protected set; }
        public BranchLevel Level { get; protected set; }
        protected Branch()
        {
            
        }
        public static Branch CreateFrom(pushactivity payload)
        {
            if (payload == null)
            {
                return null;
            }
            var branch = new Branch
            {
                Name = payload.Branch
                ,Parent = payload.BaseBranch
                ,Server = new VerControlServer(payload.repository.clone_url, AppSettings.AuthToken)
                ,Level = BranchLevel.Unknown
                ,IsVersionBranch = (payload.BaseBranch == payload.repository.master_branch)
            };

            return branch;
        }
        public static Branch CreateFrom(TestdataPayload payload)
        {
            if (payload == null)
            {
                return null;
            }
            var branch = new Branch
            {
                Name = payload.Testdata.SourceControlBranch
                ,Server = new VerControlServer(payload.Testdata.GitUrl, AppSettings.AuthToken)
                ,Level=BranchLevel.Unknown
                ,IsVersionBranch = false
            };
            return branch;
        }
        public bool IsVersionBranch { get; protected set; }
    }
}
