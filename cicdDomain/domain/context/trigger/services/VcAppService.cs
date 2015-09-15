using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.@class;
using cicd.domain.context.trigger.entity;
using cicd.domain.context.trigger.events;
using cicd.domain.context.trigger.factory;
using cicd.domain.context.trigger.repository;
using cicd.domain.context.trigger.services.domain;
using cicd.infrastructure;
using Newtonsoft.Json;
using Octokit;

namespace cicd.domain.context.trigger.services
{
    public class VcAppService : IVersionControlService
    {
        private IVcActionService _vcService;
        private VerControlServerRepo _repo;
        private IVcSettingsService _vcSettingsService;
        public IBot ComBot { get; private set; }
        public VcAppService(IBot comBot)
        {
            ComBot = comBot;
            var repo = new VerControlServerRepo();
            var vcSettingsService = new VcSettingsService(repo, ComBot);
            Init(vcSettingsService , repo, vcSettingsService );
        }
        public VcAppService(IVcActionService service, VerControlServerRepo repo, IVcSettingsService settingsService = null)
        {
            Init(service, repo, settingsService);
        }

        private void Init(IVcActionService service, VerControlServerRepo repo, IVcSettingsService settingsService)
        {
            if (service == null)
            {
                throw new ArgumentException("Version Control Service");
            }
            if (repo == null)
            {
                throw new ArgumentException("Version Control Server repository");
            }
            _vcService = service;
            _vcSettingsService = settingsService ?? service as IVcSettingsService;
            _repo = repo;
        }

        public virtual void SetPullRequestStatus(entity.Branch branch, NewCommitStatus status)
        {
            _vcService.SetPullRequest(branch, status);
        }
        //public virtual async Task UpdateSetting(entity.Branch branch, Settings settings)
        //{
        //    await _vcSettingsService.UpdateBranchSetting(branch, settings);
        //}
        public Settings GetSettings(entity.Branch branch)
        {
            var settings = _vcSettingsService.GetBranchSettings(branch) ??
                           _vcSettingsService.CreateSettingsFile(branch, _vcSettingsService.GetDefaultSettingsFor(branch));
            return settings;
        }
        public virtual void TestResult(TestdataPayload testdataPayload)
        {
            ComBot.Trigger(new TestResultsEvent(NUnitTestResult.CreateFrom(testdataPayload)));
            entity.Branch branch = entity.Branch.CreateFrom(testdataPayload);
            //setPullRequest(repo, branchName, commitStatus)
            _vcService.SetPullRequest(branch,
            CommitStateFactory.GetStateFrom(testdataPayload.Testdata));
        }
        public Settings GetDefaultSettings(entity.Branch branch)
        {
            if (branch == null)
            {
                return null;
            }
            var settings = JsonConvert.DeserializeObject<Settings>(_vcSettingsService.GetDefaultSettingsFor(branch));
            return settings;
        }
    }
}
