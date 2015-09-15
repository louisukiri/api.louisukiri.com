using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.factory;
using cicd.domain.context.trigger.repository;
using Newtonsoft.Json;
using Octokit;
using Branch = cicd.domain.context.trigger.entity.Branch;

namespace cicd.domain.context.trigger.services.domain
{
    public class VcSettingsService : IVcActionService, IVcSettingsService
    {
        private IVersionControlServerRepo Repo = null;
        public IBot CommBot { get; private set; }
        public VcSettingsService()
            :this(new VerControlServerRepo())
        { }
        public VcSettingsService(IVersionControlServerRepo repo)
            : this(repo, null)
        {
        }
        public VcSettingsService(IVersionControlServerRepo repo, IBot commBot)
        {
            Repo = repo;
            CommBot = commBot;
        }

        public async Task UpdateBranchSetting(entity.Branch branch, Settings settings)
        {
            string Sha = string.Empty;
            var server = branch.Server;

            var settingsString = await GetSettingsFile(branch, (RepositoryContent content) =>
                    {
                        Sha = content.Sha;
                    }
                   )
                .ConfigureAwait(false)
                ;
            var settingsFileExists = settingsString!=null;
            
            var serializedSettings = JsonConvert.SerializeObject(settings, Formatting.Indented);
            if (settingsFileExists && !string.IsNullOrWhiteSpace(Sha))
            {
                await Repo.Update(server.HostName, server.Auth.Token, Sha, branch.Name, "branch-settings.json",
                serializedSettings, server.AppName, server.Auth.Owner, "Updated Settings"
                ).ConfigureAwait(false)
                ;
            }
            else if(!settingsFileExists)
            {
                CreateSettingsFile(branch, serializedSettings);
            }
        }

        public Settings CreateSettingsFile(entity.Branch branch, string NewContent,
            string changeComment = "Added Branch Settings")
        {
            return CreateSettingsFile(branch.Server, branch.Name, NewContent, changeComment);
        }
        public Settings  CreateSettingsFile(VerControlServer server, string branch, string NewContent, string ChangeComment="Added Branch Settings")
        {
            var settings = JsonConvert.DeserializeObject<Settings>(NewContent);

            Task.Run(() => Repo.Insert(server.HostName, server.Auth.Token, branch, "branch-settings.json",
                NewContent, server.AppName, server.Auth.Owner, ChangeComment
                ));

            return settings;
        }
        public bool SetPullRequest(Branch branch, NewCommitStatus newStatus)
        {
            var server = branch.Server;
            if(server == null)
            {
                return false;
            }
            try
            {
                DoPullRequest(server.HostName, server.AppName, server.Auth.Owner, branch.Name, newStatus, server.Auth.Token);
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public Settings GetBranchSettings(entity.Branch branch)
        {
            string sha = string.Empty;
            Action<RepositoryContent> getSha = (RepositoryContent repoContent) => {
                                                                                      if (repoContent != null)
                                                                                      {
                                                                                          sha = repoContent.Sha;
                                                                                      }
            };
            Task<string> t = Task.Run(() =>
                GetSettingsFile(branch, getSha)
            );
            var settingsString =  t.Result;
            if (String.IsNullOrWhiteSpace(settingsString))
            {
                return null;
            }
            try
            {
                var settingsObj = SettingsFactory.SettingsFromJson(settingsString);
                return SynchBranchSettings(branch, settingsObj , sha);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// This method implements rules regarding branches and their settings
        /// It makes sure that <see cref="branch"/> matches <see cref="results"/>.Branch
        /// </summary>
        /// <param name="branch">The current branch</param>
        /// <param name="result">The branch that the current branch-settings file is referencing</param>
        /// <param name="sha">The Sha for the current settings file</param>
        /// <returns></returns>
        private Settings SynchBranchSettings(Branch branch, Settings result, string sha)
        {
            if (result.Branch.Name == branch.Name)
            {
                {
                    return result;
                }
            }
            //getting to this point indicates that the current request
            //is a result of a branch action
            result.Branch.BaseBranch = result.Branch.Name;
            result.Branch.Name = branch.Name;
            result.Branch.Path = (result.Branch.Path ?? "") + "/" + branch.Name;
            Task<string> t2 = Task.Run(() =>
                UpdateConfigFileFor(branch, sha, JsonConvert.SerializeObject(result, Formatting.Indented)));
            var updatedString = t2.Result;
            if (string.IsNullOrWhiteSpace(updatedString))
            {
                throw new Exception("could not update Branch Settings");
            }
            return result;
        }

        private async Task<string> UpdateConfigFileFor(entity.Branch branch, string sha, string serializedSettings)
        {
            var server = branch.Server;
            string ret =
                    await
                        Repo.Update(branch.Server.HostName, server.Auth.Token, sha, branch.Name, "branch-settings.json",
                            serializedSettings, server.AppName, server.Auth.Owner, "Updated Settings"
                            ).ConfigureAwait(false);
            return ret;
        }
        public virtual async Task<string> GetSettingsFile(entity.Branch branch, Action<RepositoryContent> contentDelegate = null)
        {
            var server = branch.Server;
            var branchName = branch.Name;
            var settingsString = await Repo.FileContent(server.HostName, server.Auth.Token, server.Auth.Owner
                , server.AppName, branchName, "branch-settings.json"
                , contentDelegate
                ).ConfigureAwait(false)
                ;
            return settingsString;
        }

        public virtual CommitStatus DoPullRequest(string GitUri, string AppName, string user, string branch, NewCommitStatus status, string Token = "")
        {
            var github = new GitHubClient(new ProductHeaderValue(AppName), new Uri(GitUri));
            github.Credentials = new Credentials(Token);

            var b = github.Repository.Commits.Get(user, AppName, branch).Result;
            return github.Repository.CommitStatus.Create(user, AppName, b.Sha, status)
                .Result
                ;
        }
        private string GetBranchLevel(entity.Branch branch)
        {
            return branch.IsVersionBranch? "Version" : "other";
        }
        public string GetDefaultSettingsFor(Branch branch)
        {
            if (branch == null)
            {
                return string.Empty;
            }
            string Template = @"
{{
    'Branch':{{
                'Name':'{2}',
                'Path':'{3}',
                'Level':'{0}',
                'BaseBranch':'{1}'
             }}
    ,'Jobs':[
                {{
                    'Name':'DOM-SITES-DEV-BUILD'
                    ,'Uri':''
                    ,'Path':''
                    ,'Parameters':[
                                    {{'Key':'COMPUTERNAME','Value':''}},
                                    {{'Key':'GITURL','Value':'{4}'}},
                                    {{'Key':'GITBRANCH','Value':'{5}'}},
                                    {{'Key':'PROJECTDIRECTORY','Value':''}},
                                    {{'Key':'NUNITFILENAME','Value':''}},
                                    {{'Key':'OPENCOVERREPORT','Value':''}},
                                    {{'Key':'TESTPATHDIR','Value':''}},
                                    {{'Key':'REPORTPATHDIR','Value':''}},
                                    {{'Key':'SOLUTIONPATH','Value':''}},
                                    {{'Key':'SITENAME','Value':''}},
                                    {{'Key':'SITEPATH','Value':''}},
                                    {{'Key':'PROJECTNAME','Value':'{6}'}},
                                    {{'Key':'REPORTAPI','Value':''}},
                                    {{'Key':'DEPLOYFROM','Value':''}}
                                  ]
                    ,'AuthToken':''
                    ,'Level':'Unknown'
                    ,'Trigger':'Push'
                }}
            ]
}}";
            return string.Format(Template, GetBranchLevel(branch), branch.Parent, branch.Name, (branch.Parent??"")+ "/"+branch.Name,
                branch.Server.GitSshUrlString,branch.Name,branch.Name
                );
        }
    }
}
