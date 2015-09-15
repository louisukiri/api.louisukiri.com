using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using cicd.infrastructure.dtos;
using godaddy.domains.cicd.Models;
using Newtonsoft.Json;

namespace api.louisukiri.com.Extensions
{
    public class GenericEventParameterBinding : HttpParameterBinding
    {
        public GenericEventParameterBinding(HttpParameterDescriptor parameter)
            : base(parameter)
        {
            
        }
        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            string payloadString = actionContext.GetRequestBody();
            try
            {
                var Event = JsonConvert.DeserializeObject<GenericApiEvent>(payloadString);
                return actionContext.SetActionParameter(Descriptor.ParameterName, Event);
            }
            catch (Exception)
            {
                return actionContext.SetActionParameter<GenericApiEvent>(Descriptor.ParameterName, null);
            }
        }
    }

    public class GenericEventParameterAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter.ParameterType == typeof (GenericApiEvent))
            {
                return new GenericEventParameterBinding(parameter);
            }
            return parameter.BindAsError("Wrong parameterype");
        }
    }
}