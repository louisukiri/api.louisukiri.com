namespace cicd.domain.context.trigger.abstracts
{
  public interface IJobRunResult
  {
    string message { get; set; }
    bool Failed { get;}
  }
}
