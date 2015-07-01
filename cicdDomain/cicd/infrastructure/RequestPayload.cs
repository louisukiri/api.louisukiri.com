using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using cicdDomain.cicd.domain.entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.RegularExpressions;

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
    public readonly SourceControlRepository repository;
    public readonly pushactivity Activity;

    public RequestPayload(RequestTrigger _trigger, string _payload)
    {
      if (string.IsNullOrWhiteSpace(_payload))
      {
        throw new ArgumentNullException("Payload");
      }
      Payload = _payload;
      Activity = JsonConvert.DeserializeObject<pushactivity>(Payload);
      if(_trigger != getTriggerTypeFromPayloadString())
      {
          throw new ArgumentException("bad request");
      }
      Trigger = _trigger;
    }
    public RequestTrigger getTriggerTypeFromPayloadString()
    {
      if (Activity.pusher   != null)
      {
        return  RequestTrigger.Push;
      }
      return RequestTrigger.Unknown;
    }
    public string requestActionId 
    {
        get { 
            return //Regex.Replace(
                Regex.Replace(Activity.repository.url, @"\W+","-")
                //,@"-+", "-")
                .ToLower()
                + "-"+ Trigger.ToString().ToLower()
                ;
        }
    }
    public SourceControlRepository getRepository()
    {
        return Activity.repository;
    }
  }
}
