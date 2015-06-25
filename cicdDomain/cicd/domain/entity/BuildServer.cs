using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using cicd.domain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.abstracts;
using System.Net.Http;
using System.Threading.Tasks;

namespace cicdDomain.cicd.domain.entity
{
  public class JenkinsBuildServer: IBuildServer, IBuildServerRest
  {
    public JenkinsBuildServer()
    {
      BuildServerRest = this;
    }
    public JenkinsBuildServer(IBuildServerRest buildServerRest)
    {
      BuildServerRest = buildServerRest;
    }
    public void buildJob(string name)
    {
      BuildServerRest.trigger(name);
    }
    public IBuildServerRest BuildServerRest { get; private set; }

    public HttpResponseMessage trigger(string name)
    {
      using (var client = new HttpClient())
      {
        client.BaseAddress = new Uri("http://louisjenkins.dc1.corp.gd:8080/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        var keyvalues = new List<KeyValuePair<string, string>>();
        keyvalues.Add(new KeyValuePair<string, string>("test", name));

        var content = new FormUrlEncodedContent(keyvalues);

        return client.PostAsync("job/CI-Api/buildWithParameters", content).Result;

      }
    }
  }
}
