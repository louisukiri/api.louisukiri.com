using System.Diagnostics.CodeAnalysis;
using cicd.domain.context.trigger.abstracts;

namespace cicd.infrastructure.dtos
{
  [ExcludeFromCodeCoverage]
  public class FailedRequest : IJobRunResult
  {
    public string message { get; set; }
    public bool Failed { get{ return true; }}
  }
  [ExcludeFromCodeCoverage]
  public class SuccessfulRequest : IJobRunResult
  {
      public string message { get; set; }
      public bool Failed { get { return false; } }
  }
}
