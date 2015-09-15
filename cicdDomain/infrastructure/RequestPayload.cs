using System;
using cicd.domain.context.trigger.entity;
using Newtonsoft.Json;

namespace cicd.infrastructure
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
        Branch = Branch.CreateFrom(Activity);
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

    public Branch Branch { get; set; }
  }
}
