using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using cicd.infrastructure;

namespace api.louisukiri.com.Extensions
{
  public class PayloadParameterBinding : HttpParameterBinding
  {
    public PayloadParameterBinding(HttpParameterDescriptor parameter)
      : base(parameter)
    {
    }

    public override Task ExecuteBindingAsync(System.Web.Http.Metadata.ModelMetadataProvider metadataProvider, HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
    {
      string paramName = Descriptor.ParameterName;
      string payloadString = actionContext.GetRequestBody();
      if (string.IsNullOrWhiteSpace(payloadString)
          || (!actionContext.Request.Headers.Contains("X-GitHub-Event")
                    || string.IsNullOrWhiteSpace(payloadString))
          )
      {
        return actionContext.SetActionParameter<RequestPayload>(paramName, null);
      }
      RequestTrigger triggerType = GetTriggerTypeFromHeader(actionContext);
      
      RequestPayload payload;
      try
      {
          payload = new RequestPayload(triggerType, payloadString);
      }
      catch (Exception)
      {
          return actionContext.SetActionParameter<RequestPayload>(paramName, null);
      }
      return actionContext.SetActionParameter<RequestPayload>(paramName, payload);
    }
    private RequestTrigger GetTriggerTypeFromHeader(HttpActionContext actionContext)
    {
      string keyvalue = actionContext.Request.Headers
        .First(z => z.Key == "X-GitHub-Event")
        .Value.First();
      switch (keyvalue)
      {
        case "push":
          return RequestTrigger.Push;
        case "create":
          return RequestTrigger.Branch;
        case "pull_request":
          return RequestTrigger.Pull;
      }
      return RequestTrigger.Unknown;
    }
  }
  public class PayloadParameterAttribute : ParameterBindingAttribute
  {
    public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
    {
      if (parameter.ParameterType == typeof (RequestPayload))
      {
        return new PayloadParameterBinding(parameter);
      }
      return parameter.BindAsError("Wrong parameter type");
    }
  }
}