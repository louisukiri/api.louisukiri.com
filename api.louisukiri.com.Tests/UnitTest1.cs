using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Octokit;
using TechTalk.SpecFlow;

namespace api.louisukiri.com.Tests
{
    [TestClass, Binding]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            run();
        }
         void run()
        { 
        
            var github = new GitHubClient(new ProductHeaderValue("MyAmazingApp"));
            var user =  github.User.Get("LouisUkiri");
            var _repo = github.Repository.Get("louisukiri", "paper-angel")
                .Result
                ;

            Octokit.PushEventPayload pEp = new PushEventPayload();
             
            Console.WriteLine(_repo.Url + " folks love the half ogre!");
        }
    }
}
