using cicd.domain.context.trigger.entity;
using Octokit;

namespace cicd.domain.context.trigger.factory
{
    public static class CommitStateFactory
    {

        public static NewCommitStatus GetStateFrom(Testdata input)
        {
            NewCommitStatus status = new NewCommitStatus
            {
                State = input.HasErrors? CommitState.Failure : CommitState.Success
            };
            return status;
        }
    }
}
