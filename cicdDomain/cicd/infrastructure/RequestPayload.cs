using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using cicdDomain.cicd.domain.entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace cicdDomain.cicd.infrastructure
{
  public enum RequestTrigger
  {
    Pull = 0
    , Branch = 1,
    Push = 2,
    Unknown = -1
  }
  public class RequestPayload
  {
    public readonly RequestTrigger Trigger;
    public readonly string Payload;

    public RequestPayload(RequestTrigger _trigger, string _payload)
    {
      if (string.IsNullOrWhiteSpace(_payload))
      {
        throw new ArgumentNullException("Payload");
      }
      Trigger = _trigger;
      Payload = _payload;
    }
    public RequestTrigger getTriggerTypeFromPayloadString()
    {
      var VersionControlUser = JsonConvert.DeserializeObject<pushactivity>(Payload);
      if (VersionControlUser.pusher   != null)
      {
        return  RequestTrigger.Push;
      }
      return RequestTrigger.Unknown;
    }
  }
}
