﻿using System;
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

    public Job build(Job job, pushactivity Activity)
    {
        try
        {
          var hasEmptyGitUrl = HasEmptyParameterValue(job, "GitUrl");
          var hasEmptyBranchName = HasEmptyParameterValue(job, "BranchName");
          var hasEmptyEnvironment = HasEmptyParameterValue(job, "Environment");
          if (hasEmptyGitUrl)
          {
            var gitUrlPair = job.parameters
              .First(z => z.Key == "GitUrl"
                          && string.IsNullOrWhiteSpace(z.Value));
            job.parameters.Remove(gitUrlPair);
            job.parameters.Add(new KeyValuePair<string, string>("GitUrl", Activity.repository.clone_url));
          }
          if (hasEmptyBranchName)
          {
            var gitUrlPair = job.parameters
              .First(z => z.Key == "BranchName"
                          && string.IsNullOrWhiteSpace(z.Value));
            job.parameters.Remove(gitUrlPair);
            job.parameters.Add(new KeyValuePair<string, string>("BranchName", Activity.Branch));
          }
          if (hasEmptyEnvironment)
          {
            var gitUrlPair = job.parameters
              .First(z => z.Key == "Environment"
                          && string.IsNullOrWhiteSpace(z.Value));
            job.parameters.Remove(gitUrlPair);
            job.parameters.Add(new KeyValuePair<string, string>("Environment", Activity.IsStagingBranch?"staging":"development"));
          }
          HttpResponseMessage a = trigger(job.name, job.uri, job.path, job.parameters);
          job.AddRun(a.IsSuccessStatusCode, new List<string> { a.StatusCode.ToString() });
        }
        catch(Exception ex)
        {
            job.AddRun(false, new List<string> { ex.Message });
        }

        
        return job;
    }

    private static bool HasEmptyParameterValue(Job job, string Key)
    {
      if (string.IsNullOrWhiteSpace(Key))
      {
        return false;
      }
      return job.parameters
        .Any(z => z.Key == Key
                  && string.IsNullOrWhiteSpace(z.Value));
    }
  }
}
