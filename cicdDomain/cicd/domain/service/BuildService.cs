using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.entity;

namespace cicdDomain.cicd.domain.service
{
  public class JenkinsBuildService: IBuildService
  {
    public string BaseAddress { get; set; }
    public JenkinsBuildService()
    {
    }

    public virtual HttpResponseMessage trigger(string name, string uri, string relativePath, List<KeyValuePair<string, string>> parameters=null)
    {

      using (var client = new HttpClient(new HttpClientHandler{ Credentials = new NetworkCredential(@"jomax\lukiri", "Vaneinstein4")}))
      {
        //"http://louisjenkins.dc1.corp.gd:8080/"
        client.BaseAddress = new Uri(uri);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        FormUrlEncodedContent content=null;
        if(parameters.Count > 0)
        {
            content = new FormUrlEncodedContent(parameters);
            relativePath += "/buildWithParameters?token=testToken";
         }
        else
        {
            relativePath += "/build";
        }
        
        //job/CI-Api/buildWithParameters
        return client.PostAsync(relativePath, content).Result;

      }
    }

    public Job buildSeed(Job job, pushactivity Activity)
    {
        try
        {
          SetJobParameters(job, Activity);
          HttpResponseMessage a = trigger(job.name, job.uri, job.path, job.parameters);
          job.AddRun(a.IsSuccessStatusCode, new List<string> { a.StatusCode.ToString() });
        }
        catch(Exception ex)
        {
            job.AddRun(false, new List<string> { ex.Message });
        }
        return job;
    }
    public Job buildPush(Job job, pushactivity activity)
    {
        try
        {
            SetJobParameters(job, activity);
            HttpResponseMessage a = trigger(job.name, job.uri, "job/"+GetBuildProjectName(job), job.parameters);
            job.AddRun(a.IsSuccessStatusCode, new List<string> { a.StatusCode.ToString() });
        }
        catch (Exception ex)
        {
            job.AddRun(false, new List<string> { ex.Message });
        }
        return job;
    }
    private void SetJobParameters(Job job, pushactivity Activity)
    {
        if (Activity.repository != null) job.parameters.SetIfEmtpy("GitUrl", Activity.repository.clone_url);
        job.parameters.SetIfEmtpy("BranchName", Activity.Branch??"");
        job.parameters.SetIfEmtpy("Environment", Activity.IsStagingBranch ? "staging" : "development");
        
        job.parameters.Add(new KeyValuePair<string, string>("JobName", GetBuildProjectName(job)));
    }
    public string GetBuildProjectName(Job testJob)
    {

        return "DOM-SITES-"+testJob.name??"NONAME";
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
}
