using System;
using System.Threading.Tasks;
using cicd.domain.context.trigger.entity;
using Octokit;

namespace cicd.domain.context.trigger.abstracts
{
    public interface IVersionControlServerRepo
    {
        VerControlServer GetRepoBy(string id);
        Task<string> FileContent(string uri, string token, string owner, string appName, string branch, string fileNameWithPath,Action<RepositoryContent> CallBack=null);

        Task Insert(string uri, string token, string branch, string fileNameWithPath, string content, string appName,
            string owner, string description);
        Task<string> Update(string uri, string token, string sha, string branch, string fileNameWithPath, string content, string appName,
            string owner, string description);
    }
}
