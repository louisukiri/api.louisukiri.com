using System.Net.Http;

namespace cicd.domain.context.trigger.abstracts
{
  public interface IBuildServerRest
  {
    HttpResponseMessage trigger(string p);
  }
}
