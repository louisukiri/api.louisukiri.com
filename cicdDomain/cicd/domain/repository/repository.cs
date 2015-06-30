using cicdDomain.cicd.domain.abstracts;
using cicdDomain.cicd.domain.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
           ,parameters=new List<KeyValuePair<string,string>>(){
                  new KeyValuePair<string,string>("Git_Url","https://github.com/louisukiri/paper-angel.git")
                }
              }
           }
        };
        public Job getJobBy(string id)
        {
            throw new NotImplementedException();
        }
    }
}
