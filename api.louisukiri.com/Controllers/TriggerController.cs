using System.Web;
using cicdDomain.cicd.domain.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using cicdDomain.cicd.domain.service;
using cicdDomain.cicd.infrastructure;

namespace api.louisukiri.com.Controllers
{
    public class TriggersController : ApiController
    {
        [Route("api/v1/test"), HttpGet]
        public string test()
        {
            return "ok jim";
        }
        [Route("api/v1/push"), HttpPost]
        public HttpResponseMessage push(RequestPayload value)
        {
          CICDService service = new CICDService();
          var result = service.run(value);

            
          return !result.Failed? new HttpResponseMessage(HttpStatusCode.OK) 
            : new HttpResponseMessage(HttpStatusCode.NotFound) ;
        }

    }
}
