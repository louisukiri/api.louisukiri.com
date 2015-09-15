using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.events;
using cicd.infrastructure;

namespace cicd.domain.context.trigger.services.domain
{
  public class JenkinsBuildService: IBuildService
  {
    public string BaseAddress { get; set; }
    public IBot CommBot { get; private set; }
    public JenkinsBuildService(IBot commBot)
    {
        CommBot = commBot;
    }
    public virtual HttpResponseMessage trigger(string name, string uri, string relativePath, List<KeyValuePair<string, string>> parameters=null, string authToken="")
    {

      using (var client = new HttpClient())
      {
        //"http://louisjenkins.dc1.corp.gd:8080/"
        client.BaseAddress = new Uri(uri);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        FormUrlEncodedContent content=null;
        if(parameters.Count > 0)
        {
            content = new FormUrlEncodedContent(parameters);
            relativePath += "/buildWithParameters";
         }
        else
        {
            relativePath += "/build";
        }
        if (!string.IsNullOrWhiteSpace(authToken))
        {
              relativePath += "?token=" + authToken;
        }
        //job/CI-Api/buildWithParameters
        return client.PostAsync(relativePath, content).Result;

      }
    }
    public Job Build(Job job, Branch branch)
    {
        try
        {
            SetJobParameters(job, branch);
            CommBot.Trigger(new BuildEvent(job, branch));
            HttpResponseMessage a = trigger(job.Name, job.Uri, job.Path, job.Parameters, job.AuthToken);
            job.AddRun(a.IsSuccessStatusCode, new List<string> { a.StatusCode.ToString() });
        }
        catch (Exception ex)
        {
            job.AddRun(false, new List<string> { ex.Message });
        }
        return job;
    }
    private void SetJobParameters(Job job, Branch branch)
    {
        job.Parameters.SetIfEmtpy("GITURL", branch.Server.GitFullUriString);
        job.Parameters.SetIfEmtpy("GITBRANCH", branch.Name);
        job.Parameters.SetIfEmtpy("ENVIRONMENT", branch.Level.ToString());
        job.EscapeParameters();
    }
  }
    public static class ListKeyValueExtension
    {
        public static void SetIfEmtpy(this List<KeyValuePair<string, string>> pairs, string key, string value)
        {
            var gitUrlPair = pairs
                .Where(z => z.Key == key
                            && string.IsNullOrWhiteSpace(z.Value));
            var keyValuePairs = gitUrlPair as KeyValuePair<string, string>[] ?? gitUrlPair.ToArray();
            if (gitUrlPair != null && !keyValuePairs.Any())
            {
                return;
            }
            pairs.Remove(keyValuePairs.First());
            pairs.Add(new KeyValuePair<string, string>(key, value));
        }
    }
    public static class extendJobs
    {
        public static void EscapeParameters(this Job job)
        {
            List<KeyValuePair<string, string>> newPairs = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < job.Parameters.Count; i++)
            {
                var a = job.Parameters[i];
                if (a.Value.Contains(@"\"))
                {
                    string k = a.Key;
                    string v = a.Value;
                    StringBuilder strB = new StringBuilder();
                    foreach (char c in v)
                    {
                        strB.Append(c);
                        if (c == '\\')
                        {
                            strB.Append(c);
                            strB.Append(c);
                            strB.Append(c);
                        }
                    }
                    //job.Parameters.Remove(a);
                    newPairs.Add(new KeyValuePair<string, string>(k, strB.ToString()));
                }
            }
            newPairs.ForEach(z =>
            {
                var oldkvp = job.Parameters.First(y => y.Key == z.Key);
                job.Parameters.Remove(oldkvp);
                job.Parameters.Add(z);
            });
        }
    }
}
