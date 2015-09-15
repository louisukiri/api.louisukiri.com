using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using cicd.infrastructure;

namespace api.louisukiri.com.Extensions
{
    public class TestdataParameterBinding : HttpParameterBinding
    {
        public TestdataParameterBinding(HttpParameterDescriptor parameter)
            : base(parameter)
        {
            
        }
        public override Task ExecuteBindingAsync(System.Web.Http.Metadata.ModelMetadataProvider metadataProvider, HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            string paramName = Descriptor.ParameterName;
            string requestBody = actionContext.GetRequestBody();
            if (string.IsNullOrWhiteSpace(requestBody))
            {
                return actionContext.SetActionParameter<TestdataPayload>(paramName, null);
            }
            TestdataPayload payload;
            try
            {
                payload = new TestdataPayload(requestBody);
            }
            catch (Exception)
            {
                return actionContext.SetActionParameter<TestdataPayload>(paramName, null);
            }

            return actionContext.SetActionParameter<TestdataPayload>(paramName, payload);
        }
    }

    public class TestdataParameterAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter.ParameterType == typeof (TestdataPayload))
            {
                return new TestdataParameterBinding(parameter);
            }
            return parameter.BindAsError("Wrong parameter type");
        }
    }
}