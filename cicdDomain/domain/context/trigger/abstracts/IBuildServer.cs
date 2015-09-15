namespace cicd.domain.context.trigger.abstracts
{
    public interface IBuildServer
    {
      string BaseAddress { get; set; }
      void buildJob(string name);
    }
}
