using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using cicdDomain.cicd.infrastructure;

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
      if (actionContext.Request.Content == null)
      {
        return SetActionParameter(actionContext, null);
      }
      string payloadString = actionContext.Request.Content
        .ReadAsStringAsync()
        .Result
        ;
      if (!actionContext.Request.Headers.Contains("X-GitHub-Event")
        || string.IsNullOrWhiteSpace(payloadString)
        )
      {
        return SetActionParameter(actionContext, null);
      }
      RequestTrigger triggerType = GetTriggerTypeFromHeader(actionContext);
      
      RequestPayload payload;
      try
      {
          payload = new RequestPayload(triggerType, payloadString);
      }
      catch (Exception ex)
      {
        return SetActionParameter(actionContext, null);
      }
      return SetActionParameter(actionContext, payload);
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
    private Task SetActionParameter(HttpActionContext actionContext, RequestPayload payload)
    {
      actionContext.ActionArguments[Descriptor.ParameterName] = payload;
      //this is required so this method can exit at the point of invocation

      var tsc = new TaskCompletionSource<object>();
      tsc.SetResult(null);

      return tsc.Task;
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