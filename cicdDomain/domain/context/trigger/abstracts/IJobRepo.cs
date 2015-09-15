using cicd.domain.context.trigger.entity;

namespace cicd.domain.context.trigger.abstracts
{
    public interface IJobRepo
    {
        Job getJobBy(string id);
    }
}
