using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.entity;
using cicd.infrastructure;
using Octokit;

namespace cicd.domain.context.trigger.repository
{
    public class VerControlServerRepo : IVersionControlServerRepo
    {
        public Dictionary<string, VerControlServer> Servers = new Dictionary<string, VerControlServer>{
           {"git@github.secureserver.net:lukiri/CI.git", new VerControlServer("git@github.secureserver.net:lukiri/CI.git",AppSettings.AuthToken)}
           //,{"https://github.secureserver.net/DomainApplications/DCC5.git", new VerControlServer("https://github.secureserver.net/DomainApplications/DCC5.git","DomainApplications",testInfrastructure.APIToken)}}
           ,{"git@github.secureserver.net:DomainApplications/DCC5.git", new VerControlServer("https://github.secureserver.net/DomainApplications/DCC5.git",AppSettings.AuthToken)}
        };
        public virtual VerControlServer GetRepoBy(string id)
        {
            var HasRepo = Servers.Any(z => z.Key == id
                || z.Value.GitFullUriString == id
                );
            if(HasRepo)
            {
                return Servers.First(z => z.Key == id || z.Value.GitFullUriString == id).Value;
            }
            return null;
        }
        public virtual VerControlServer GetRepoBy(string repoUrl, string Auth)
        {
            try
            {
                return new VerControlServer(repoUrl, Auth);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public virtual IGitHubClient GitHubClient(string uri, string appName, string token)
        {
            var githubclient = new GitHubClient(new ProductHeaderValue(appName), new Uri(uri))
            {
                Credentials = new Credentials(token)
            };
            return githubclient;
        }
        public virtual async Task<string> FileContent(string uri, string token, string owner, string appName, string branch, string fileNameWithPath
            ,Action<RepositoryContent> CallBack=null
            )
        {
            try
            {
                var fObject = await getFileObject(uri, appName, owner, token, fileNameWithPath, branch)
                    .ConfigureAwait(false)
                    ;
                if (fObject == null)
                {
                    return null;
                }
                if (CallBack != null)
                {
                    CallBack(fObject);
                }
                return fObject.Content;
                //string url = fObject.DownloadUrl.ToString();
                //WebClient client = new WebClient();
                //return client.DownloadString(url);
            }
            catch (Exception)
            {
            }
            return string.Empty;
        }
        public async Task<string> Update(string uri, string token, string Sha, string branch, string fileNameWithPath, string content, string appName,
             string owner, string description)
        {
            if (string.IsNullOrWhiteSpace(branch))
            {
                throw new ArgumentException("branch");
            }
            if (string.IsNullOrWhiteSpace(fileNameWithPath))
            {
                throw new ArgumentException("file path");
            }
            if (string.IsNullOrWhiteSpace(Sha))
            {
                throw new ArgumentException("Sha");
            }
            var github = GitHubClient(uri, appName, token);
            try
            {
                var c = await github.Repository.Content.CreateFile(owner, appName, fileNameWithPath,
                    new UpdateFileRequest(description, content, Sha){ Branch = branch });
                    return content;
            }
            catch (Exception)
            {
                return string.Empty;
            }


        }
        public async Task Insert(string uri, string token, string branch, string fileNameWithPath, string content, string appName,
             string owner, string description)
        {
            if (string.IsNullOrWhiteSpace(branch))
            {
                throw new ArgumentException("branch");
            }
            if (string.IsNullOrWhiteSpace(fileNameWithPath))
            {
                throw new ArgumentException("file path");
            }
            var github = GitHubClient(uri, appName, token);

            var a = await github.Repository.Content.CreateFile(owner, appName, fileNameWithPath,
                    new CreateFileRequest(description, content) { Branch = branch });
        }
        private async Task<string> getFileDownloadUrl(string uri, string appName, string owner, string token, string filename, string branch)
        {
            var github = GitHubClient(uri, appName, token);

            var a = await GetFileReference(appName, owner, filename, branch, github)

                ;
            if (a.Count == 0)
            {
                return await Task.FromResult(string.Empty);
            }

            return await Task.FromResult(a.First().DownloadUrl.ToString());
        }
        private async Task<RepositoryContent> getFileObject(string uri, string appName, string owner, string token, string filename, string branch)
        {
            var github = GitHubClient(uri, appName, token);

            var a = await GetFileReference(appName, owner, filename, branch, github)
                .ConfigureAwait(false)
                ;
            if (a.Count == 0)
            {
                return null;
            }

            return a.First();
        }
        private async static Task<IReadOnlyList<RepositoryContent>> GetFileReference(string appName, string owner, string filename, string branch,
            IGitHubClient github)
        {
            try
            {
                string _filename = filename + (!string.IsNullOrWhiteSpace(branch) ? "?ref=" + branch : "");
                var a = await github.Repository.Content
                    .GetAllContents(owner, appName, _filename)
                    .ConfigureAwait(false)
                    ;
                return a;
            }
            catch (Exception)
            {
            }
            var b = new List<RepositoryContent>().AsReadOnly() as IReadOnlyList<RepositoryContent>;
            return b;
        }

        public string getFile(string repoUrl, string branchName, string filename)
        {
            throw new NotImplementedException();
        }
    }
}
