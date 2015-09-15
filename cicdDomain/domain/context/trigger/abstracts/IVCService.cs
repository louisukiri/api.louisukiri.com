using System.Threading.Tasks;
using cicd.domain.context.trigger.entity;
using Octokit;
using System.Collections.Generic;

namespace cicd.domain.context.trigger.abstracts
{
    public interface IVcActionService
    {
        bool SetPullRequest(entity.Branch branch, NewCommitStatus newStatus);
    }
    public interface IVcSettingsService
    {
        Task UpdateBranchSetting(entity.Branch branch, Settings settings);
        Settings GetBranchSettings(entity.Branch branch);
        Settings CreateSettingsFile(VerControlServer server, string branch, string NewContent,
        string ChangeComment = "Added Branch Settings");
        Settings CreateSettingsFile(entity.Branch branch, string NewContent,
        string changeComment = "Added Branch Settings");
        string GetDefaultSettingsFor(entity.Branch branch);
    }
}
