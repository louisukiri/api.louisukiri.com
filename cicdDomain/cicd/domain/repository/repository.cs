using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cicdDomain.cicd.domain.factory;

namespace cicdDomain.cicd.domain.repository
{
    public class JobRepository : IJobRepo
    {
        private Dictionary<string, Job> jobs = new Dictionary<string, Job>() 
        { 
           {"https-github-com-louisukiri-paper-angel-push", new Job(){
           name="louisukiri-paperAngel"
           ,uri="http://4ab16704.ngrok.io/"
           ,path="job/Test"
           //,vcUrl = "https://github.com/louisukiri/paper-angel/"
           ,parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("GitUrl","https://github.com/louisukiri/paper-angel.git"),
                  new KeyValuePair<string,string>("FriendlyName","louisukiri-paperAngel"),
                  new KeyValuePair<string,string>("BranchName",string.Empty),
                  new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           },
           {"http-louisjenkins-dc1-corp-gd-8080-branch", new Job(){
           name="louisukiri-ci"
           ,uri="http://louisjenkins.dc1.corp.gd:8080/"
           ,path="job/CI-Api"
           //,vcUrl = ""
           ,parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("GitUrl","https://github.com/louisukiri/paper-angel.git"),
                  new KeyValuePair<string,string>("FriendlyName","louisukiri-ci"),
                  new KeyValuePair<string,string>("BranchName",string.Empty),
                  new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           },
           {"http-louisjenkins-dc1-corp-gd-8080-push", new Job(){
           name="louisukiri-ci"
           ,uri="http://louisjenkins.dc1.corp.gd:8080/"
           ,path="job/CI-Api"
           //,vcUrl = ""
           ,parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("GitUrl","https://github.com/louisukiri/paper-angel.git"),
                  new KeyValuePair<string,string>("FriendlyName","louisukiri-ci"),
                  new KeyValuePair<string,string>("BranchName",string.Empty),
                  new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           },
           {"https-github-secureserver-net-domainapplications-dcc5-branch", new Job(){
           name="dcc5-ci"
           ,uri="http://g1dwtfsbuild008.jomax.paholdings.com/"
           ,path="job/DOM-Sites-DCC-5-Seed"
           ,authToken = "tokenforApi"
           //,vcUrl = ""
           ,parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("GitUrl","https://github.secureserver.net/DomainApplications/DCC5.git"),
                  new KeyValuePair<string,string>("FriendlyName","dcc5-ci"),
                  new KeyValuePair<string,string>("BranchName",string.Empty),
                  new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           },
           {"https-github-secureserver-net-domainapplications-dcc5-push", new Job(){
           name="dcc5-ci"
           ,uri="http://g1dwtfsbuild008.jomax.paholdings.com/"
           ,path="job/DOM-Sites-DCC-5-Seed"
           , authToken = "tokenforApi"
           //,vcUrl = ""
           ,parameters=new List<KeyValuePair<string,string>>(){
                }
              }
           },
           {"https-github-secureserver-net-lukiri-ci-branch", new Job(){
           name="louisukiri-ci"
           ,uri="http://louisjenkins.dc1.corp.gd:8080/"
           ,path="job/CI-Api"
           //,vcUrl = "https://github.secureserver.net/lukiri/CI/"
           ,parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("GitUrl",string.Empty),
                  new KeyValuePair<string,string>("FriendlyName","louisukiri-ci"),
                  new KeyValuePair<string,string>("BranchName",string.Empty),
                  new KeyValuePair<string,string>("Environment",string.Empty)
                }
              }
           },
           {"https-github-secureserver-net-lukiri-ci-push", new Job(){
           name="louisukiri-ci"
           ,uri="http://louisjenkins.dc1.corp.gd:8080/"
           ,path="job/CI-Api"
           //,vcUrl = "https://github.secureserver.net/lukiri/CI/"
           ,parameters=new List<KeyValuePair<string,string>>(){
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
