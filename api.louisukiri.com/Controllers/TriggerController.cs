using System.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using cicd.domain.context.trigger.entity.bot;
using cicd.domain.context.trigger.services;
using cicd.infrastructure;

namespace api.louisukiri.com.Controllers
{
    public class TriggersController : ApiController
    {
        [Route("api/v1/test"), HttpPost]
        public string test()
        {
            var req = this.Request;
            return "ok jim";
        }
        [Route("api/v1/testdata"), HttpPost]
        public HttpResponseMessage testdata(TestdataPayload data)
        {
            if (data == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            var req = this.Request;
            VcAppService service = new VcAppService(new SlackBot());
            service.TestResult(data);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        [Route("api/v1/push"), HttpPost]
        public HttpResponseMessage push(RequestPayload value)
        {
          CICDService service = new CICDService();
          var result = service.run(value);

          return !result.Failed? new HttpResponseMessage(HttpStatusCode.OK) 
            : new HttpResponseMessage(HttpStatusCode.BadRequest){Content = new StringContent(result.message)} ;
        }

    }
}
