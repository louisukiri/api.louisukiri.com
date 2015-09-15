using System.Threading.Tasks;
using cicd.domain.context.trigger.entity;
using Octokit;
using System.Collections.Generic;

namespace cicd.domain.context.trigger.abstracts
{
    public interface IVersionControlService
    {
        void SetPullRequestStatus(entity.Branch branch, NewCommitStatus status);
        Settings GetSettings(entity.Branch branch);
        Settings GetDefaultSettings(entity.Branch branch);
    }
}