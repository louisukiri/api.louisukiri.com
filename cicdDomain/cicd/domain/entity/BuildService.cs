﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
using cicd.domain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.abstracts;
using System.Net.Http;
using System.Threading.Tasks;

namespace cicdDomain.cicd.domain.entity
{
  public class JenkinsBuildService: IBuildService
  {
    public string BaseAddress { get; set; }
    public JenkinsBuildService()
    {
    }

    public virtual HttpResponseMessage trigger(string name, string uri, string relativePath, List<KeyValuePair<string, string>> parameters=null)
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
        //job/CI-Api/buildWithParameters
        return client.PostAsync(relativePath, content).Result;

      }
    }

    public Job build(Job job)
    {
        HttpResponseMessage a = null;
        try
        {
          var hasEmptyGitUrl = job.parameters
            .Any(z => z.Key == "GitUrl"
              && string.IsNullOrWhiteSpace(z.Value));
          var hasEmptyBranchName = job.parameters
            .Any(z => z.Key == "BranchName"
              && string.IsNullOrWhiteSpace(z.Value));
          if (hasEmptyGitUrl)
          {
            var gitUrlPair = job.parameters
              .First(z => z.Key == "GitUrl"
                          && string.IsNullOrWhiteSpace(z.Value));
            job.parameters.Remove(gitUrlPair);
            job.parameters.Add(new KeyValuePair<string, string>("GitUrl", job.vcUrl));
          }
          if (hasEmptyBranchName)
          {
            var gitUrlPair = job.parameters
              .First(z => z.Key == "BranchName"
                          && string.IsNullOrWhiteSpace(z.Value));
            job.parameters.Remove(gitUrlPair);
            job.parameters.Add(new KeyValuePair<string, string>("BranchName", job.branch));
          }
            a = trigger(job.name, job.uri, job.path, job.parameters);
            job.AddRun(a.IsSuccessStatusCode, new List<string> { a.StatusCode.ToString() });
        }
        catch(Exception ex)
        {
            job.AddRun(false, new List<string> { ex.Message });
        }

        
        return job;
    }
  }
}
