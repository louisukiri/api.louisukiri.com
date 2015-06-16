using cicdDomain.cicd.domain.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace api.louisukiri.com.Controllers
{
    public class WelcomeController : ApiController
    {
        // GET: api/Welcome
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Welcome/5
        public string Get(int id)
        {
            //File.AppendAllText(HostingEnvironment.MapPath("~/call.txt"), DateTime.Now.ToString());
            using (StreamWriter strw = File.AppendText(HostingEnvironment.MapPath("~/call.txt")))
            { 
                strw.WriteLineAsync(DateTime.Now.ToString()); 
            }
            return "value";
        }

        // POST: api/Welcome
        public pushactivity Post([FromBody]pushactivity value)
        {
            
            //File.AppendAllText(HostingEnvironment.MapPath("~/call.txt"), DateTime.Now.ToString());
            
            //using (StreamWriter strw = File.AppendText(HostingEnvironment.MapPath("~/call.txt")))
            //{
            //    strw.WriteLineAsync(Request.Content.ToString());
            //    strw.WriteLineAsync(DateTime.Now.ToString() + " doing this");
            //}
            string me = Request.Content.ReadAsStringAsync().Result;
            return value;
        }

        // PUT: api/Welcome/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Welcome/5
        public void Delete(int id)
        {
        }
    }
}
