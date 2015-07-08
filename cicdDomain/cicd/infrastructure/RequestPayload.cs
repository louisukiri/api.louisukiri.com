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
      _trigger = NormalizeRequestType(_trigger);
      if(_trigger != Activity.type)
      {
          throw new ArgumentException("bad request");
      }
      Trigger = _trigger;
    }
    /// <summary>
    /// fix for rules that create inconsistencies between domain rules
    /// and the received header
    /// </summary>
    /// <param name="_trigger"></param>
    /// <returns></returns>
    private RequestTrigger NormalizeRequestType(RequestTrigger _trigger)
    {
      if (Activity.type == RequestTrigger.Branch && _trigger == RequestTrigger.Push)
      {
        _trigger = RequestTrigger.Branch;
      }
      return _trigger;
    }
  }
}
