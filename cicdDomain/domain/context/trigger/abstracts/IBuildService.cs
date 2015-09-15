using cicd.domain.context.trigger.entity;

namespace cicd.domain.context.trigger.abstracts
{
    public interface IBuildService
    {
        Job Build(Job job, Branch branch);
    }
}
