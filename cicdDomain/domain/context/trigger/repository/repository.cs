using System.Collections.Generic;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.factory;

namespace cicd.domain.context.trigger.repository
{
    public class JobRepository : IJobRepo
    {
        private Dictionary<string, Job> jobs = new Dictionary<string, Job>() 
        { 
           {"https-github-com-louisukiri-paper-angel-push", new Job(){
           Name="louisukiri-paperAngel"
           ,Uri="http://4ab16704.ngrok.io/"
           ,Path="job/Test"
           //,vcUrl = "https://github.com/louisukiri/paper-angel/"
           ,Parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("GitUrl","https://github.com/louisukiri/paper-angel.git"),
                  new KeyValuePair<string,string>("FriendlyName","louisukiri-paperAngel"),
                  new KeyValuePair<string,string>("BranchName",string.Empty),
                  new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           },
           {"http-louisjenkins-dc1-corp-gd-8080-branch", new Job(){
           Name="louisukiri-ci"
           ,Uri="http://louisjenkins.dc1.corp.gd:8080/"
           ,Path="job/CI-Api"
           //,vcUrl = ""
           ,Parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("GitUrl","https://github.com/louisukiri/paper-angel.git"),
                  new KeyValuePair<string,string>("FriendlyName","louisukiri-ci"),
                  new KeyValuePair<string,string>("BranchName",string.Empty),
                  new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           },
           {"http-louisjenkins-dc1-corp-gd-8080-push", new Job(){
           Name="louisukiri-ci"
           ,Uri="http://louisjenkins.dc1.corp.gd:8080/"
           ,Path="job/CI-Api"
           //,vcUrl = ""
           ,Parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("GitUrl","https://github.com/louisukiri/paper-angel.git"),
                  new KeyValuePair<string,string>("FriendlyName","louisukiri-ci"),
                  new KeyValuePair<string,string>("BranchName",string.Empty),
                  new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           },
           //https-github-secureserver-net-api-v3-repos-domainapplications-dcc5-pull
           {"https-github-secureserver-net-api-v3-repos-domainapplications-dcc5-pull", new Job(){
           Name="dcc5-ci"
           ,Uri="http://g1dwtfsbuild008.jomax.paholdings.com/"
           ,Path="job/DOM-SITES-DEV-BUILD"
           ,AuthToken = "tokenforApi"
           //,vcUrl = ""
           ,Parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("GitUrl","git@github.secureserver.net:DomainApplications/DCC5.git"),
                  new KeyValuePair<string,string>("FriendlyName","dcc5-ci"),
                  new KeyValuePair<string,string>("BranchName",string.Empty),
                  new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           },
           {"https-github-secureserver-net-domainapplications-dcc5-push", new Job(){
           Name="dcc5-ci"
           ,Uri="http://g1dwtfsbuild008.jomax.paholdings.com/"
           ,Path="job/DOM-SITES-DEV-BUILD"
           , AuthToken = "tokenforApi"
           //,vcUrl = ""
           ,Parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("GitUrl","git@github.secureserver.net:DomainApplications/DCC5.git"),
                  //new KeyValuePair<string,string>("FriendlyName","dcc5-ci"),
                  new KeyValuePair<string,string>("BranchName",string.Empty)
                  ,new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           }
           ,
           {"https-github-secureserver-net-domainapplications-dcc5-branch", new Job(){
           Name="dcc5-ci"
           ,Uri="http://g1dwtfsbuild008.jomax.paholdings.com/"
           ,Path="job/DOM-SITES-DEV-BUILD"
           , AuthToken = "tokenforApi"
           //,vcUrl = ""
           ,Parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("GitUrl","git@github.secureserver.net:DomainApplications/DCC5.git"),
                  //new KeyValuePair<string,string>("FriendlyName","dcc5-ci"),
                  new KeyValuePair<string,string>("BranchName",string.Empty)
                  ,new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           },
           {"https-github-secureserver-net-lukiri-ci-branch", new Job(){
           Name="louisukiri-ci"
           ,Uri="http://louisjenkins.dc1.corp.gd:8080/"
           ,Path="job/DOM-SITES-DEV-BUILD"
           //,vcUrl = "https://github.secureserver.net/lukiri/CI/"
           ,Parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("GitUrl",string.Empty),
                  new KeyValuePair<string,string>("FriendlyName","louisukiri-ci"),
                  new KeyValuePair<string,string>("BranchName",string.Empty),
                  new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           },
           {"https-github-secureserver-net-lukiri-ci-pull", new Job(){
           Name="louisukiri-ci"
           ,Uri="http://louisjenkins.dc1.corp.gd:8080/"
           ,Path="job/DOM-SITES-DEV-BUILD"
           //,vcUrl = "https://github.secureserver.net/lukiri/CI/"
           ,Parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("GitUrl",string.Empty),
                  new KeyValuePair<string,string>("FriendlyName","louisukiri-ci"),
                  new KeyValuePair<string,string>("BranchName",string.Empty),
                  new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           },
           {"https-github-secureserver-net-lukiri-ci-push", new Job(){
           Name="louisukiri-ci"
           ,Uri="http://louisjenkins.dc1.corp.gd:8080/"
           ,Path="job/DOM-SITES-DEV-BUILD"
           //,vcUrl = "https://github.secureserver.net/lukiri/CI/"
           ,Parameters=new List<KeyValuePair<string,string>>(){
                  //new KeyValuePair<string,string>("GitUrl",string.Empty),
                  //new KeyValuePair<string,string>("FriendlyName","louisukiri-ci"),
                  //new KeyValuePair<string,string>("BranchName",string.Empty),
                  //new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           }
        }
        ;
        public Job getJobBy(string id)
        {
          if (jobs.ContainsKey(id))
          {
            return jobs[id];
          }
          return JobFactory.FailedJob("Job Not Found");
        }
    }
}
