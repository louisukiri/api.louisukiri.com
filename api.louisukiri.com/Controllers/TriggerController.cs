using cicdDomain.cicd.domain.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using cicdDomain.cicd.infrastructure;

namespace api.louisukiri.com.Controllers
{
    public class TriggersController : ApiController
    {
        [HttpGet]
        public string test()
        {
            return "ok jim";
        }
        [Route("api/v1/push"), HttpPost]
        public RequestPayload push(RequestPayload value)
        {
            string me = Request.Content.ReadAsStringAsync().Result;
            return value;
        }

    }
}
