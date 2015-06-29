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
        throw new HttpResponseException(HttpStatusCode.BadRequest);
      }
      string payloadString = actionContext.Request.Content
        .ReadAsStringAsync()
        .Result
        ;
      RequestPayload payload = new RequestPayload(RequestTrigger.Branch, payloadString);
      if (string.IsNullOrWhiteSpace(payloadString))
      {
        throw new HttpResponseException(HttpStatusCode.BadRequest);
      }
      actionContext.ActionArguments[Descriptor.ParameterName] = payload;

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