using System.Linq;
using cicd.domain.context.trigger.entity;
using cicd.infrastructure;
using System;
using System.Collections.Generic;
using cicd.infrastructure.dtos;
using godaddy.domains.cicd.Models;
using Newtonsoft.Json;

namespace cicdDomain
{
  public class testInfrastructure
  {
    public static string random
    {
      get { return Guid.NewGuid().ToString().Split('-').Last(); }
    }
    public static Job FailedJob()
    {
      return getJob(false);
    }
    public static Job getJob(bool successful = true)
    {
      List<string> messages = new List<string>();
      if (!successful)
      {
        messages.Add("Error");
      }
      return new Job
      {
        Executions = new List<Execution> { 
                        new Execution(successful, DateTime.Now, messages)
                    }
      };
    }

      public static RequestPayload getRequestPayload(RequestTrigger triggerType = RequestTrigger.Push)
      {
          return getRequestPayload(testInfrastructure.GitHubPushContent, triggerType);
      }
    public static RequestPayload getRequestPayload(string content, RequestTrigger triggerType = RequestTrigger.Push)
    {
        return new RequestPayload(triggerType, content);
    }
    public static pushactivity GetOtherBranchActivity
    {
        get
        {
            return new pushactivity
            {
                repository = new SourceControlRepository { url = "http://test.foo", master_branch = "master" },
                base_ref = null
            };
        }
    }
    public static pushactivity GetMasterBranchActivity
    {
        get
        {
          return new pushactivity
          {
              repository = new SourceControlRepository {url = "http://test.foo", master_branch = "master"},
              base_ref = "ref/heads/master"
          };
        }
      }

      public static string APIToken
      {
          get { return "4d5b313a7b53ad08387849bd8bb6f9999e1ced6b"; }
      }

      public static string IntegratedRepoUrl
      {
          get { return "https://github.secureserver.net/lukiri/CI.git"; }
      }
      public static string IntegratedRepoUrlSSH
      {
          get { return "git@github.secureserver.net:lukiri/CI.git"; }
      }

      public static string RepoUriUrl
      {
          get { return "https://github.test.net/user/app.git"; }
      }
      public static string RepoSshUrl
      {
          get { return "git@github.test.net:user/app.git"; }
      }

    #region Push Content

      public static pushactivity GitHubPushActivity
      {
          get { return JsonConvert.DeserializeObject<pushactivity>(GitHubPushContent); }
      }
    public static string GitHubPushContent = @"
{
  'ref': 'refs/heads/master',
  'before': 'f7055ca74d42451d6a2862426f36e7f415cb591d',
  'after': '9d6efe51930113ff08ab59ed2ae49315cb3a0a69',
  'created': false,
  'deleted': false,
  'forced': false,
  'base_ref': null,
  'compare': 'https://github.secureserver.net/lukiri/CI/compare/f7055ca74d42...9d6efe519301',
  'commits': [
    {
      'id': '9d6efe51930113ff08ab59ed2ae49315cb3a0a69',
      'distinct': true,
      'message': 'test git call\n\ntest get call',
      'timestamp': '2015-06-30T17:16:46-05:00',
      'url': 'https://github.secureserver.net/lukiri/CI/commit/9d6efe51930113ff08ab59ed2ae49315cb3a0a69',
      'author': {
        'name': 'Louis Ukiri',
        'email': 'lukiri@godaddy.com',
        'username': 'lukiri'
      },
      'committer': {
        'name': 'Louis Ukiri',
        'email': 'lukiri@godaddy.com',
        'username': 'lukiri'
      },
      'added': [

      ],
      'removed': [

      ],
      'modified': [
        'testDoc.html'
      ]
    }
  ],
  'head_commit': {
    'id': '9d6efe51930113ff08ab59ed2ae49315cb3a0a69',
    'distinct': true,
    'message': 'test git call\n\ntest get call',
    'timestamp': '2015-06-30T17:16:46-05:00',
    'url': 'https://github.secureserver.net/lukiri/CI/commit/9d6efe51930113ff08ab59ed2ae49315cb3a0a69',
    'author': {
      'name': 'Louis Ukiri',
      'email': 'lukiri@godaddy.com',
      'username': 'lukiri'
    },
    'committer': {
      'name': 'Louis Ukiri',
      'email': 'lukiri@godaddy.com',
      'username': 'lukiri'
    },
    'added': [

    ],
    'removed': [

    ],
    'modified': [
      'testDoc.html'
    ]
  },
  'repository': {
    'id': 9603,
    'name': 'CI',
    'full_name': 'lukiri/CI',
    'owner': {
      'name': 'lukiri',
      'email': 'lukiri@godaddy.com'
    },
    'private': false,
    'html_url': 'https://github.secureserver.net/lukiri/CI',
    'description': '',
    'fork': false,
    'url': 'https://github.secureserver.net/lukiri/CI',
    'forks_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/forks',
    'keys_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/keys{/key_id}',
    'collaborators_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/collaborators{/collaborator}',
    'teams_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/teams',
    'hooks_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/hooks',
    'issue_events_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/issues/events{/number}',
    'events_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/events',
    'assignees_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/assignees{/user}',
    'branches_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/branches{/branch}',
    'tags_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/tags',
    'blobs_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/blobs{/sha}',
    'git_tags_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/tags{/sha}',
    'git_refs_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/refs{/sha}',
    'trees_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/trees{/sha}',
    'statuses_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/statuses/{sha}',
    'languages_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/languages',
    'stargazers_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/stargazers',
    'contributors_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/contributors',
    'subscribers_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/subscribers',
    'subscription_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/subscription',
    'commits_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/commits{/sha}',
    'git_commits_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/commits{/sha}',
    'comments_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/comments{/number}',
    'issue_comment_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/issues/comments{/number}',
    'contents_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/contents/{+path}',
    'compare_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/compare/{base}...{head}',
    'merges_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/merges',
    'archive_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/{archive_format}{/ref}',
    'downloads_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/downloads',
    'issues_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/issues{/number}',
    'pulls_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/pulls{/number}',
    'milestones_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/milestones{/number}',
    'notifications_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/notifications{?since,all,participating}',
    'labels_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/labels{/name}',
    'releases_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/releases{/id}',
    'created_at': 1430142416,
    'updated_at': '2015-06-22T21:17:40Z',
    'pushed_at': 1435702634,
    'git_url': 'git://github.secureserver.net/lukiri/CI.git',
    'ssh_url': 'git@github.secureserver.net:lukiri/CI.git',
    'clone_url': 'https://github.secureserver.net/lukiri/CI.git',
    'svn_url': 'https://github.secureserver.net/lukiri/CI',
    'homepage': null,
    'size': 105,
    'stargazers_count': 0,
    'watchers_count': 0,
    'language': 'HTML',
    'has_issues': true,
    'has_downloads': true,
    'has_wiki': true,
    'has_pages': false,
    'forks_count': 0,
    'mirror_url': null,
    'open_issues_count': 0,
    'forks': 0,
    'open_issues': 0,
    'watchers': 0,
    'default_branch': 'master',
    'stargazers': 0,
    'master_branch': 'master'
  },
  'pusher': {
    'name': 'lukiri',
    'email': 'lukiri@godaddy.com'
  },
  'sender': {
    'login': 'lukiri',
    'id': 1460,
    'avatar_url': 'https://github.secureserver.net/avatars/u/1460?',
    'gravatar_id': '',
    'url': 'https://github.secureserver.net/api/v3/users/lukiri',
    'html_url': 'https://github.secureserver.net/lukiri',
    'followers_url': 'https://github.secureserver.net/api/v3/users/lukiri/followers',
    'following_url': 'https://github.secureserver.net/api/v3/users/lukiri/following{/other_user}',
    'gists_url': 'https://github.secureserver.net/api/v3/users/lukiri/gists{/gist_id}',
    'starred_url': 'https://github.secureserver.net/api/v3/users/lukiri/starred{/owner}{/repo}',
    'subscriptions_url': 'https://github.secureserver.net/api/v3/users/lukiri/subscriptions',
    'organizations_url': 'https://github.secureserver.net/api/v3/users/lukiri/orgs',
    'repos_url': 'https://github.secureserver.net/api/v3/users/lukiri/repos',
    'events_url': 'https://github.secureserver.net/api/v3/users/lukiri/events{/privacy}',
    'received_events_url': 'https://github.secureserver.net/api/v3/users/lukiri/received_events',
    'type': 'User',
    'site_admin': false,
    'ldap_dn': 'CN=Louis Ukiri,OU=Iowa Developers,OU=Users & Groups,OU=GoDaddy,DC=dc1,DC=corp,DC=gd'
  }
}
";
//    public static string GitHubPushContent = @"
//{
//  'zen': 'Design for failure.',
//  'hook_id': 3263,
//  'hook': {
//    'url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/hooks/3263',
//    'test_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/hooks/3263/test',
//    'ping_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/hooks/3263/pings',
//    'id': 3263,
//    'name': 'web',
//    'active': true,
//    'events': [
//      '*'
//    ],
//    'config': {
//      'url': 'http://louisukiri.dc1.corp.gd/api/v1/test',
//      'content_type': 'json',
//      'insecure_ssl': '0',
//      'secret': ''
//    },
//    'last_response': {
//      'code': null,
//      'status': 'unused',
//      'message': null
//    },
//    'updated_at': '2015-06-30T20:36:04Z',
//    'created_at': '2015-06-30T20:36:04Z'
//  },
//  'repository': {
//    'id': 9603,
//    'name': 'CI',
//    'full_name': 'lukiri/CI',
//    'owner': {
//      'login': 'lukiri',
//      'id': 1460,
//      'avatar_url': 'https://github.secureserver.net/avatars/u/1460?',
//      'gravatar_id': '',
//      'url': 'https://github.secureserver.net/api/v3/users/lukiri',
//      'html_url': 'https://github.secureserver.net/lukiri',
//      'followers_url': 'https://github.secureserver.net/api/v3/users/lukiri/followers',
//      'following_url': 'https://github.secureserver.net/api/v3/users/lukiri/following{/other_user}',
//      'gists_url': 'https://github.secureserver.net/api/v3/users/lukiri/gists{/gist_id}',
//      'starred_url': 'https://github.secureserver.net/api/v3/users/lukiri/starred{/owner}{/repo}',
//      'subscriptions_url': 'https://github.secureserver.net/api/v3/users/lukiri/subscriptions',
//      'organizations_url': 'https://github.secureserver.net/api/v3/users/lukiri/orgs',
//      'repos_url': 'https://github.secureserver.net/api/v3/users/lukiri/repos',
//      'events_url': 'https://github.secureserver.net/api/v3/users/lukiri/events{/privacy}',
//      'received_events_url': 'https://github.secureserver.net/api/v3/users/lukiri/received_events',
//      'type': 'User',
//      'site_admin': false,
//      'ldap_dn': 'CN=Louis Ukiri,OU=Iowa Developers,OU=Users & Groups,OU=GoDaddy,DC=dc1,DC=corp,DC=gd'
//    },
//    'private': false,
//    'html_url': 'https://github.secureserver.net/lukiri/CI',
//    'description': '',
//    'fork': false,
//    'url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI',
//    'forks_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/forks',
//    'keys_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/keys{/key_id}',
//    'collaborators_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/collaborators{/collaborator}',
//    'teams_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/teams',
//    'hooks_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/hooks',
//    'issue_events_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/issues/events{/number}',
//    'events_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/events',
//    'assignees_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/assignees{/user}',
//    'branches_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/branches{/branch}',
//    'tags_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/tags',
//    'blobs_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/blobs{/sha}',
//    'git_tags_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/tags{/sha}',
//    'git_refs_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/refs{/sha}',
//    'trees_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/trees{/sha}',
//    'statuses_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/statuses/{sha}',
//    'languages_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/languages',
//    'stargazers_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/stargazers',
//    'contributors_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/contributors',
//    'subscribers_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/subscribers',
//    'subscription_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/subscription',
//    'commits_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/commits{/sha}',
//    'git_commits_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/commits{/sha}',
//    'comments_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/comments{/number}',
//    'issue_comment_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/issues/comments{/number}',
//    'contents_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/contents/{+path}',
//    'compare_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/compare/{base}...{head}',
//    'merges_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/merges',
//    'archive_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/{archive_format}{/ref}',
//    'downloads_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/downloads',
//    'issues_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/issues{/number}',
//    'pulls_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/pulls{/number}',
//    'milestones_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/milestones{/number}',
//    'notifications_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/notifications{?since,all,participating}',
//    'labels_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/labels{/name}',
//    'releases_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/releases{/id}',
//    'created_at': '2015-04-27T13:46:56Z',
//    'updated_at': '2015-06-22T21:17:40Z',
//    'pushed_at': '2015-06-22T21:17:38Z',
//    'git_url': 'git://github.secureserver.net/lukiri/CI.git',
//    'ssh_url': 'git@github.secureserver.net:lukiri/CI.git',
//    'clone_url': 'https://github.secureserver.net/lukiri/CI.git',
//    'svn_url': 'https://github.secureserver.net/lukiri/CI',
//    'homepage': null,
//    'size': 105,
//    'stargazers_count': 0,
//    'watchers_count': 0,
//    'language': 'HTML',
//    'has_issues': true,
//    'has_downloads': true,
//    'has_wiki': true,
//    'has_pages': false,
//    'forks_count': 0,
//    'mirror_url': null,
//    'open_issues_count': 0,
//    'forks': 0,
//    'open_issues': 0,
//    'watchers': 0,
//    'default_branch': 'master'
//  },
//  'sender': {
//    'login': 'lukiri',
//    'id': 1460,
//    'avatar_url': 'https://github.secureserver.net/avatars/u/1460?',
//    'gravatar_id': '',
//    'url': 'https://github.secureserver.net/api/v3/users/lukiri',
//    'html_url': 'https://github.secureserver.net/lukiri',
//    'followers_url': 'https://github.secureserver.net/api/v3/users/lukiri/followers',
//    'following_url': 'https://github.secureserver.net/api/v3/users/lukiri/following{/other_user}',
//    'gists_url': 'https://github.secureserver.net/api/v3/users/lukiri/gists{/gist_id}',
//    'starred_url': 'https://github.secureserver.net/api/v3/users/lukiri/starred{/owner}{/repo}',
//    'subscriptions_url': 'https://github.secureserver.net/api/v3/users/lukiri/subscriptions',
//    'organizations_url': 'https://github.secureserver.net/api/v3/users/lukiri/orgs',
//    'repos_url': 'https://github.secureserver.net/api/v3/users/lukiri/repos',
//    'events_url': 'https://github.secureserver.net/api/v3/users/lukiri/events{/privacy}',
//    'received_events_url': 'https://github.secureserver.net/api/v3/users/lukiri/received_events',
//    'type': 'User',
//    'site_admin': false,
//    'ldap_dn': 'CN=Louis Ukiri,OU=Iowa Developers,OU=Users & Groups,OU=GoDaddy,DC=dc1,DC=corp,DC=gd'
//  }
//}
//";
//    public static string GitHubPushContent = @"{
//  'ref': 'refs/heads/Add-Module-Events',
//  'before': '36c0ab88025995d0562d3fb016f126b75306c93f',
//  'after': '8dad01851588d09c7ac66c2a7c80801306081d48',
//  'created': false,
//  'deleted': false,
//  'forced': false,
//  'base_ref': null,
//  'compare': 'https://github.com/louisukiri/paper-angel/compare/36c0ab880259...8dad01851588',
//  'commits': [
//    {
//      'id': '8dad01851588d09c7ac66c2a7c80801306081d48',
//      'distinct': true,
//      'message': 'commit\n\ncommited',
//      'timestamp': '2015-06-09T23:22:37-05:00',
//      'url': 'https://github.com/louisukiri/paper-angel/commit/8dad01851588d09c7ac66c2a7c80801306081d48',
//      'author': {
//        'name': 'louisukiri',
//        'email': 'louisukiri@gmail.com',
//        'username': 'louisukiri'
//      },
//      'committer': {
//        'name': 'louisukiri',
//        'email': 'louisukiri@gmail.com',
//        'username': 'louisukiri'
//      },
//      'added': [
//        'DNN Platform/Syndication/obj/Debug/DesignTimeResolveAssemblyReferencesInput.cache'
//      ],
//      'removed': [
//
//      ],
//      'modified': [
//
//      ]
//    }
//  ],
//  'head_commit': {
//    'id': '8dad01851588d09c7ac66c2a7c80801306081d48',
//    'distinct': true,
//    'message': 'commit\n\ncommited',
//    'timestamp': '2015-06-09T23:22:37-05:00',
//    'url': 'https://github.com/louisukiri/paper-angel/commit/8dad01851588d09c7ac66c2a7c80801306081d48',
//    'author': {
//      'name': 'louisukiri',
//      'email': 'louisukiri@gmail.com',
//      'username': 'louisukiri'
//    },
//    'committer': {
//      'name': 'louisukiri',
//      'email': 'louisukiri@gmail.com',
//      'username': 'louisukiri'
//    },
//    'added': [
//      'DNN Platform/Syndication/obj/Debug/DesignTimeResolveAssemblyReferencesInput.cache'
//    ],
//    'removed': [
//
//    ],
//    'modified': [
//
//    ]
//  },
//  'repository': {
//    'id': 20598016,
//    'name': 'paper-angel',
//    'full_name': 'louisukiri/paper-angel',
//    'owner': {
//      'name': 'louisukiri',
//      'email': 'louisukiri@gmail.com'
//    },
//    'private': false,
//    'html_url': 'https://github.com/louisukiri/paper-angel',
//    'description': '',
//    'fork': false,
//    'url': 'https://github.com/louisukiri/paper-angel',
//    'forks_url': 'https://api.github.com/repos/louisukiri/paper-angel/forks',
//    'keys_url': 'https://api.github.com/repos/louisukiri/paper-angel/keys{/key_id}',
//    'collaborators_url': 'https://api.github.com/repos/louisukiri/paper-angel/collaborators{/collaborator}',
//    'teams_url': 'https://api.github.com/repos/louisukiri/paper-angel/teams',
//    'hooks_url': 'https://api.github.com/repos/louisukiri/paper-angel/hooks',
//    'issue_events_url': 'https://api.github.com/repos/louisukiri/paper-angel/issues/events{/number}',
//    'events_url': 'https://api.github.com/repos/louisukiri/paper-angel/events',
//    'assignees_url': 'https://api.github.com/repos/louisukiri/paper-angel/assignees{/user}',
//    'branches_url': 'https://api.github.com/repos/louisukiri/paper-angel/branches{/branch}',
//    'tags_url': 'https://api.github.com/repos/louisukiri/paper-angel/tags',
//    'blobs_url': 'https://api.github.com/repos/louisukiri/paper-angel/git/blobs{/sha}',
//    'git_tags_url': 'https://api.github.com/repos/louisukiri/paper-angel/git/tags{/sha}',
//    'git_refs_url': 'https://api.github.com/repos/louisukiri/paper-angel/git/refs{/sha}',
//    'trees_url': 'https://api.github.com/repos/louisukiri/paper-angel/git/trees{/sha}',
//    'statuses_url': 'https://api.github.com/repos/louisukiri/paper-angel/statuses/{sha}',
//    'languages_url': 'https://api.github.com/repos/louisukiri/paper-angel/languages',
//    'stargazers_url': 'https://api.github.com/repos/louisukiri/paper-angel/stargazers',
//    'contributors_url': 'https://api.github.com/repos/louisukiri/paper-angel/contributors',
//    'subscribers_url': 'https://api.github.com/repos/louisukiri/paper-angel/subscribers',
//    'subscription_url': 'https://api.github.com/repos/louisukiri/paper-angel/subscription',
//    'commits_url': 'https://api.github.com/repos/louisukiri/paper-angel/commits{/sha}',
//    'git_commits_url': 'https://api.github.com/repos/louisukiri/paper-angel/git/commits{/sha}',
//    'comments_url': 'https://api.github.com/repos/louisukiri/paper-angel/comments{/number}',
//    'issue_comment_url': 'https://api.github.com/repos/louisukiri/paper-angel/issues/comments{/number}',
//    'contents_url': 'https://api.github.com/repos/louisukiri/paper-angel/contents/{+path}',
//    'compare_url': 'https://api.github.com/repos/louisukiri/paper-angel/compare/{base}...{head}',
//    'merges_url': 'https://api.github.com/repos/louisukiri/paper-angel/merges',
//    'archive_url': 'https://api.github.com/repos/louisukiri/paper-angel/{archive_format}{/ref}',
//    'downloads_url': 'https://api.github.com/repos/louisukiri/paper-angel/downloads',
//    'issues_url': 'https://api.github.com/repos/louisukiri/paper-angel/issues{/number}',
//    'pulls_url': 'https://api.github.com/repos/louisukiri/paper-angel/pulls{/number}',
//    'milestones_url': 'https://api.github.com/repos/louisukiri/paper-angel/milestones{/number}',
//    'notifications_url': 'https://api.github.com/repos/louisukiri/paper-angel/notifications{?since,all,participating}',
//    'labels_url': 'https://api.github.com/repos/louisukiri/paper-angel/labels{/name}',
//    'releases_url': 'https://api.github.com/repos/louisukiri/paper-angel/releases{/id}',
//    'created_at': 1402160630,
//    'updated_at': '2014-06-07T18:11:10Z',
//    'pushed_at': 1433910187,
//    'git_url': 'git://github.com/louisukiri/paper-angel.git',
//    'ssh_url': 'git@github.com:louisukiri/paper-angel.git',
//    'clone_url': 'https://github.com/louisukiri/paper-angel.git',
//    'svn_url': 'https://github.com/louisukiri/paper-angel',
//    'homepage': null,
//    'size': 48752,
//    'stargazers_count': 0,
//    'watchers_count': 0,
//    'language': 'C#',
//    'has_issues': true,
//    'has_downloads': true,
//    'has_wiki': true,
//    'has_pages': false,
//    'forks_count': 0,
//    'mirror_url': null,
//    'open_issues_count': 0,
//    'forks': 0,
//    'open_issues': 0,
//    'watchers': 0,
//    'default_branch': 'master',
//    'stargazers': 0,
//    'master_branch': 'master'
//  },
//  'pusher': {
//    'name': 'louisukiri',
//    'email': 'louisukiri@gmail.com'
//  },
//  'sender': {
//    'login': 'louisukiri',
//    'id': 3752355,
//    'avatar_url': 'https://avatars.githubusercontent.com/u/3752355?v=3',
//    'gravatar_id': '',
//    'url': 'https://api.github.com/users/louisukiri',
//    'html_url': 'https://github.com/louisukiri',
//    'followers_url': 'https://api.github.com/users/louisukiri/followers',
//    'following_url': 'https://api.github.com/users/louisukiri/following{/other_user}',
//    'gists_url': 'https://api.github.com/users/louisukiri/gists{/gist_id}',
//    'starred_url': 'https://api.github.com/users/louisukiri/starred{/owner}{/repo}',
//    'subscriptions_url': 'https://api.github.com/users/louisukiri/subscriptions',
//    'organizations_url': 'https://api.github.com/users/louisukiri/orgs',
//    'repos_url': 'https://api.github.com/users/louisukiri/repos',
//    'events_url': 'https://api.github.com/users/louisukiri/events{/privacy}',
//    'received_events_url': 'https://api.github.com/users/louisukiri/received_events',
//    'type': 'User',
//    'site_admin': false
//  }
//}
//";

    #endregion
    #region Branch Content

    public static pushactivity GitCreatePushactivity
    {
        get { return JsonConvert.DeserializeObject<pushactivity>(GitHubBranchContent); }
    }
    public static string GitHubBranchContent = @"
{
  'ref': 'refs/heads/test',
  'before': 'f7055ca74d42451d6a2862426f36e7f415cb591d',
  'after': '9d6efe51930113ff08ab59ed2ae49315cb3a0a69',
  'created': true,
  'deleted': false,
  'forced': false,
  'base_ref': 'refs/heads/master',
  'compare': 'https://github.secureserver.net/lukiri/CI/compare/f7055ca74d42...9d6efe519301',
  'commits': [
    {
      'id': '9d6efe51930113ff08ab59ed2ae49315cb3a0a69',
      'distinct': true,
      'message': 'test git call\n\ntest get call',
      'timestamp': '2015-06-30T17:16:46-05:00',
      'url': 'https://github.secureserver.net/lukiri/CI/commit/9d6efe51930113ff08ab59ed2ae49315cb3a0a69',
      'author': {
        'name': 'Louis Ukiri',
        'email': 'lukiri@godaddy.com',
        'username': 'lukiri'
      },
      'committer': {
        'name': 'Louis Ukiri',
        'email': 'lukiri@godaddy.com',
        'username': 'lukiri'
      },
      'added': [

      ],
      'removed': [

      ],
      'modified': [
        'testDoc.html'
      ]
    }
  ],
  'head_commit': {
    'id': '9d6efe51930113ff08ab59ed2ae49315cb3a0a69',
    'distinct': true,
    'message': 'test git call\n\ntest get call',
    'timestamp': '2015-06-30T17:16:46-05:00',
    'url': 'https://github.secureserver.net/lukiri/CI/commit/9d6efe51930113ff08ab59ed2ae49315cb3a0a69',
    'author': {
      'name': 'Louis Ukiri',
      'email': 'lukiri@godaddy.com',
      'username': 'lukiri'
    },
    'committer': {
      'name': 'Louis Ukiri',
      'email': 'lukiri@godaddy.com',
      'username': 'lukiri'
    },
    'added': [

    ],
    'removed': [

    ],
    'modified': [
      'testDoc.html'
    ]
  },
  'repository': {
    'id': 9603,
    'name': 'CI',
    'full_name': 'lukiri/CI',
    'owner': {
      'name': 'lukiri',
      'email': 'lukiri@godaddy.com'
    },
    'private': false,
    'html_url': 'https://github.secureserver.net/lukiri/CI',
    'description': '',
    'fork': false,
    'url': 'https://github.secureserver.net/lukiri/CI',
    'forks_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/forks',
    'keys_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/keys{/key_id}',
    'collaborators_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/collaborators{/collaborator}',
    'teams_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/teams',
    'hooks_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/hooks',
    'issue_events_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/issues/events{/number}',
    'events_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/events',
    'assignees_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/assignees{/user}',
    'branches_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/branches{/branch}',
    'tags_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/tags',
    'blobs_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/blobs{/sha}',
    'git_tags_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/tags{/sha}',
    'git_refs_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/refs{/sha}',
    'trees_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/trees{/sha}',
    'statuses_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/statuses/{sha}',
    'languages_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/languages',
    'stargazers_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/stargazers',
    'contributors_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/contributors',
    'subscribers_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/subscribers',
    'subscription_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/subscription',
    'commits_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/commits{/sha}',
    'git_commits_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/git/commits{/sha}',
    'comments_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/comments{/number}',
    'issue_comment_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/issues/comments{/number}',
    'contents_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/contents/{+path}',
    'compare_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/compare/{base}...{head}',
    'merges_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/merges',
    'archive_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/{archive_format}{/ref}',
    'downloads_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/downloads',
    'issues_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/issues{/number}',
    'pulls_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/pulls{/number}',
    'milestones_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/milestones{/number}',
    'notifications_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/notifications{?since,all,participating}',
    'labels_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/labels{/name}',
    'releases_url': 'https://github.secureserver.net/api/v3/repos/lukiri/CI/releases{/id}',
    'created_at': 1430142416,
    'updated_at': '2015-06-22T21:17:40Z',
    'pushed_at': 1435702634,
    'git_url': 'git://github.secureserver.net/lukiri/CI.git',
    'ssh_url': 'git@github.secureserver.net:lukiri/CI.git',
    'clone_url': 'https://github.secureserver.net/lukiri/CI.git',
    'svn_url': 'https://github.secureserver.net/lukiri/CI',
    'homepage': null,
    'size': 105,
    'stargazers_count': 0,
    'watchers_count': 0,
    'language': 'HTML',
    'has_issues': true,
    'has_downloads': true,
    'has_wiki': true,
    'has_pages': false,
    'forks_count': 0,
    'mirror_url': null,
    'open_issues_count': 0,
    'forks': 0,
    'open_issues': 0,
    'watchers': 0,
    'default_branch': 'master',
    'stargazers': 0,
    'master_branch': 'master'
  },
  'pusher': {
    'name': 'lukiri',
    'email': 'lukiri@godaddy.com'
  },
  'sender': {
    'login': 'lukiri',
    'id': 1460,
    'avatar_url': 'https://github.secureserver.net/avatars/u/1460?',
    'gravatar_id': '',
    'url': 'https://github.secureserver.net/api/v3/users/lukiri',
    'html_url': 'https://github.secureserver.net/lukiri',
    'followers_url': 'https://github.secureserver.net/api/v3/users/lukiri/followers',
    'following_url': 'https://github.secureserver.net/api/v3/users/lukiri/following{/other_user}',
    'gists_url': 'https://github.secureserver.net/api/v3/users/lukiri/gists{/gist_id}',
    'starred_url': 'https://github.secureserver.net/api/v3/users/lukiri/starred{/owner}{/repo}',
    'subscriptions_url': 'https://github.secureserver.net/api/v3/users/lukiri/subscriptions',
    'organizations_url': 'https://github.secureserver.net/api/v3/users/lukiri/orgs',
    'repos_url': 'https://github.secureserver.net/api/v3/users/lukiri/repos',
    'events_url': 'https://github.secureserver.net/api/v3/users/lukiri/events{/privacy}',
    'received_events_url': 'https://github.secureserver.net/api/v3/users/lukiri/received_events',
    'type': 'User',
    'site_admin': false,
    'ldap_dn': 'CN=Louis Ukiri,OU=Iowa Developers,OU=Users & Groups,OU=GoDaddy,DC=dc1,DC=corp,DC=gd'
  }
}
";

    #endregion
    #region testdata content
    public static Testdata TestdataWithError
    {
        get
        {
            return new Testdata
            {
                data = new List<IList<string>>
                {
                    //this is the one error
                    new List<string>{"1", "0", "3"}
                    , new List<string>{"0", "0", "3"}
                    , new List<string>{"0", "0", "3"}
                }.ToArray()
            };
        }
    }
    public static Testdata Testdata
    {
        get
        {
            return new Testdata
            {
                data = new List<IList<string>>
                {
                    new List<string>{"0", "0", "1"}
                    , new List<string>{"0", "0", "4"}
                    , new List<string>{"0", "0", "3"}
                }.ToArray()
            };
        }
    }
    public static string TestdataContent
    {
        get
        {
            return @"
{
    'data':  [
                 [
                     '0',
                     '0',
                     '6',
                     'BusinessObjects.DB.Tests-nunitTestResults.xml'
                 ],
                 [
                     '0',
                     '0',
                     '64',
                     'BusinessObjects.Domains.Tests-nunitTestResults.xml'
                 ],
                 [
                     '0',
                     '0',
                     '11',
                     'DCCWeb.Tests-nunitTestResults.xml'
                 ]
             ],
    'info':  {
                 'BuildNumber':  '27',
                 'ProjectName':  'PleaseUpdate',
                 'SourceControlBranch':  'DOMWARGS-3452-1',
                 'DeployEnvironment':  'dev',
                 'JobName':  'DOM-Sites-DCC50-Dev-CD',
                 'GitUrl': 'git@github.secureserver.net:lukiri/CI.git'
             }
} 
";
        }
    }
    public static string TestdataContentWithError
    {
        get
        {
            return @"
{
    'data':  [
                 [
                     '6',
                     '1',
                     '5',
                     'BusinessObjects.DB.Tests-nunitTestResults.xml'
                 ],
                 [
                     '2',
                     '0',
                     '64',
                     'BusinessObjects.Domains.Tests-nunitTestResults.xml'
                 ],
                 [
                     '0',
                     '0',
                     '11',
                     'DCCWeb.Tests-nunitTestResults.xml'
                 ]
             ],
    'info':  {
                 'BuildNumber':  '27',
                 'ProjectName':  'PleaseUpdate',
                 'SourceControlBranch':  'DOMWARGS-3452-1',
                 'DeployEnvironment':  'dev',
                 'JobName':  'DOM-Sites-DCC50-Dev-CD',
                 'GitUrl': 'git@github.secureserver.net:lukiri/CI.git'
             }
} 
";
        }
    }
    #endregion
    #region Pull Content
      public static string GithubPullRequestContent
      {
          get { return @"
{
  'action': 'closed',
  'number': 235,
  'pull_request': {
    'url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/pulls/235',
    'id': 29761,
    'html_url': 'https://github.secureserver.net/DomainApplications/DCC5/pull/235',
    'diff_url': 'https://github.secureserver.net/DomainApplications/DCC5/pull/235.diff',
    'patch_url': 'https://github.secureserver.net/DomainApplications/DCC5/pull/235.patch',
    'issue_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/issues/235',
    'number': 235,
    'state': 'closed',
    'locked': false,
    'title': 'version-7 - Merge to master missed this change.',
    'user': {
      'login': 'jroling',
      'id': 804,
      'avatar_url': 'https://github.secureserver.net/avatars/u/804?',
      'gravatar_id': '',
      'url': 'https://github.secureserver.net/api/v3/users/jroling',
      'html_url': 'https://github.secureserver.net/jroling',
      'followers_url': 'https://github.secureserver.net/api/v3/users/jroling/followers',
      'following_url': 'https://github.secureserver.net/api/v3/users/jroling/following{/other_user}',
      'gists_url': 'https://github.secureserver.net/api/v3/users/jroling/gists{/gist_id}',
      'starred_url': 'https://github.secureserver.net/api/v3/users/jroling/starred{/owner}{/repo}',
      'subscriptions_url': 'https://github.secureserver.net/api/v3/users/jroling/subscriptions',
      'organizations_url': 'https://github.secureserver.net/api/v3/users/jroling/orgs',
      'repos_url': 'https://github.secureserver.net/api/v3/users/jroling/repos',
      'events_url': 'https://github.secureserver.net/api/v3/users/jroling/events{/privacy}',
      'received_events_url': 'https://github.secureserver.net/api/v3/users/jroling/received_events',
      'type': 'User',
      'site_admin': false,
      'ldap_dn': 'CN=John Roling,OU=Iowa Developers,OU=Users & Groups,OU=GoDaddy,DC=dc1,DC=corp,DC=gd'
    },
    'body': '',
    'created_at': '2015-07-13T22:13:12Z',
    'updated_at': '2015-07-13T22:15:50Z',
    'closed_at': '2015-07-13T22:15:50Z',
    'merged_at': '2015-07-13T22:15:50Z',
    'merge_commit_sha': null,
    'assignee': null,
    'milestone': null,
    'commits_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/pulls/235/commits',
    'review_comments_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/pulls/235/comments',
    'review_comment_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/pulls/comments{/number}',
    'comments_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/issues/235/comments',
    'statuses_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/statuses/118fbffb248d4dd98dc07a0297a5af4f81420edf',
    'head': {
      'label': 'DomainApplications:version-7',
      'ref': 'version-7',
      'sha': '118fbffb248d4dd98dc07a0297a5af4f81420edf',
      'user': {
        'login': 'DomainApplications',
        'id': 693,
        'avatar_url': 'https://github.secureserver.net/avatars/u/693?',
        'gravatar_id': '',
        'url': 'https://github.secureserver.net/api/v3/users/DomainApplications',
        'html_url': 'https://github.secureserver.net/DomainApplications',
        'followers_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/followers',
        'following_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/following{/other_user}',
        'gists_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/gists{/gist_id}',
        'starred_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/starred{/owner}{/repo}',
        'subscriptions_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/subscriptions',
        'organizations_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/orgs',
        'repos_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/repos',
        'events_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/events{/privacy}',
        'received_events_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/received_events',
        'type': 'Organization',
        'site_admin': false
      },
      'repo': {
        'id': 6208,
        'name': 'DCC5',
        'full_name': 'DomainApplications/DCC5',
        'owner': {
          'login': 'DomainApplications',
          'id': 693,
          'avatar_url': 'https://github.secureserver.net/avatars/u/693?',
          'gravatar_id': '',
          'url': 'https://github.secureserver.net/api/v3/users/DomainApplications',
          'html_url': 'https://github.secureserver.net/DomainApplications',
          'followers_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/followers',
          'following_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/following{/other_user}',
          'gists_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/gists{/gist_id}',
          'starred_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/starred{/owner}{/repo}',
          'subscriptions_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/subscriptions',
          'organizations_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/orgs',
          'repos_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/repos',
          'events_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/events{/privacy}',
          'received_events_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/received_events',
          'type': 'Organization',
          'site_admin': false
        },
        'private': false,
        'html_url': 'https://github.secureserver.net/DomainApplications/DCC5',
        'description': '',
        'fork': false,
        'url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5',
        'forks_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/forks',
        'keys_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/keys{/key_id}',
        'collaborators_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/collaborators{/collaborator}',
        'teams_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/teams',
        'hooks_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/hooks',
        'issue_events_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/issues/events{/number}',
        'events_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/events',
        'assignees_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/assignees{/user}',
        'branches_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/branches{/branch}',
        'tags_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/tags',
        'blobs_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/blobs{/sha}',
        'git_tags_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/tags{/sha}',
        'git_refs_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/refs{/sha}',
        'trees_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/trees{/sha}',
        'statuses_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/statuses/{sha}',
        'languages_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/languages',
        'stargazers_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/stargazers',
        'contributors_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/contributors',
        'subscribers_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/subscribers',
        'subscription_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/subscription',
        'commits_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/commits{/sha}',
        'git_commits_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/commits{/sha}',
        'comments_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/comments{/number}',
        'issue_comment_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/issues/comments{/number}',
        'contents_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/contents/{+path}',
        'compare_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/compare/{base}...{head}',
        'merges_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/merges',
        'archive_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/{archive_format}{/ref}',
        'downloads_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/downloads',
        'issues_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/issues{/number}',
        'pulls_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/pulls{/number}',
        'milestones_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/milestones{/number}',
        'notifications_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/notifications{?since,all,participating}',
        'labels_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/labels{/name}',
        'releases_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/releases{/id}',
        'created_at': '2014-09-05T14:33:12Z',
        'updated_at': '2015-07-13T22:15:50Z',
        'pushed_at': '2015-07-13T22:15:49Z',
        'git_url': 'git://github.secureserver.net/DomainApplications/DCC5.git',
        'ssh_url': 'git@github.secureserver.net:DomainApplications/DCC5.git',
        'clone_url': 'https://github.secureserver.net/DomainApplications/DCC5.git',
        'svn_url': 'https://github.secureserver.net/DomainApplications/DCC5',
        'homepage': null,
        'size': 7119,
        'stargazers_count': 1,
        'watchers_count': 1,
        'language': 'C#',
        'has_issues': true,
        'has_downloads': true,
        'has_wiki': true,
        'has_pages': false,
        'forks_count': 1,
        'mirror_url': null,
        'open_issues_count': 0,
        'forks': 1,
        'open_issues': 0,
        'watchers': 1,
        'default_branch': 'master'
      }
    },
    'base': {
      'label': 'DomainApplications:master',
      'ref': 'master',
      'sha': 'f1a2477830aa5e2ca1c70c1045e533c3e12a5de9',
      'user': {
        'login': 'DomainApplications',
        'id': 693,
        'avatar_url': 'https://github.secureserver.net/avatars/u/693?',
        'gravatar_id': '',
        'url': 'https://github.secureserver.net/api/v3/users/DomainApplications',
        'html_url': 'https://github.secureserver.net/DomainApplications',
        'followers_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/followers',
        'following_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/following{/other_user}',
        'gists_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/gists{/gist_id}',
        'starred_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/starred{/owner}{/repo}',
        'subscriptions_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/subscriptions',
        'organizations_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/orgs',
        'repos_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/repos',
        'events_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/events{/privacy}',
        'received_events_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/received_events',
        'type': 'Organization',
        'site_admin': false
      },
      'repo': {
        'id': 6208,
        'name': 'DCC5',
        'full_name': 'DomainApplications/DCC5',
        'owner': {
          'login': 'DomainApplications',
          'id': 693,
          'avatar_url': 'https://github.secureserver.net/avatars/u/693?',
          'gravatar_id': '',
          'url': 'https://github.secureserver.net/api/v3/users/DomainApplications',
          'html_url': 'https://github.secureserver.net/DomainApplications',
          'followers_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/followers',
          'following_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/following{/other_user}',
          'gists_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/gists{/gist_id}',
          'starred_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/starred{/owner}{/repo}',
          'subscriptions_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/subscriptions',
          'organizations_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/orgs',
          'repos_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/repos',
          'events_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/events{/privacy}',
          'received_events_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/received_events',
          'type': 'Organization',
          'site_admin': false
        },
        'private': false,
        'html_url': 'https://github.secureserver.net/DomainApplications/DCC5',
        'description': '',
        'fork': false,
        'url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5',
        'forks_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/forks',
        'keys_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/keys{/key_id}',
        'collaborators_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/collaborators{/collaborator}',
        'teams_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/teams',
        'hooks_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/hooks',
        'issue_events_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/issues/events{/number}',
        'events_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/events',
        'assignees_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/assignees{/user}',
        'branches_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/branches{/branch}',
        'tags_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/tags',
        'blobs_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/blobs{/sha}',
        'git_tags_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/tags{/sha}',
        'git_refs_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/refs{/sha}',
        'trees_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/trees{/sha}',
        'statuses_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/statuses/{sha}',
        'languages_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/languages',
        'stargazers_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/stargazers',
        'contributors_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/contributors',
        'subscribers_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/subscribers',
        'subscription_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/subscription',
        'commits_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/commits{/sha}',
        'git_commits_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/commits{/sha}',
        'comments_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/comments{/number}',
        'issue_comment_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/issues/comments{/number}',
        'contents_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/contents/{+path}',
        'compare_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/compare/{base}...{head}',
        'merges_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/merges',
        'archive_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/{archive_format}{/ref}',
        'downloads_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/downloads',
        'issues_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/issues{/number}',
        'pulls_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/pulls{/number}',
        'milestones_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/milestones{/number}',
        'notifications_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/notifications{?since,all,participating}',
        'labels_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/labels{/name}',
        'releases_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/releases{/id}',
        'created_at': '2014-09-05T14:33:12Z',
        'updated_at': '2015-07-13T22:15:50Z',
        'pushed_at': '2015-07-13T22:15:49Z',
        'git_url': 'git://github.secureserver.net/DomainApplications/DCC5.git',
        'ssh_url': 'git@github.secureserver.net:DomainApplications/DCC5.git',
        'clone_url': 'https://github.secureserver.net/DomainApplications/DCC5.git',
        'svn_url': 'https://github.secureserver.net/DomainApplications/DCC5',
        'homepage': null,
        'size': 7119,
        'stargazers_count': 1,
        'watchers_count': 1,
        'language': 'C#',
        'has_issues': true,
        'has_downloads': true,
        'has_wiki': true,
        'has_pages': false,
        'forks_count': 1,
        'mirror_url': null,
        'open_issues_count': 0,
        'forks': 1,
        'open_issues': 0,
        'watchers': 1,
        'default_branch': 'master'
      }
    },
    '_links': {
      'self': {
        'href': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/pulls/235'
      },
      'html': {
        'href': 'https://github.secureserver.net/DomainApplications/DCC5/pull/235'
      },
      'issue': {
        'href': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/issues/235'
      },
      'comments': {
        'href': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/issues/235/comments'
      },
      'review_comments': {
        'href': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/pulls/235/comments'
      },
      'review_comment': {
        'href': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/pulls/comments{/number}'
      },
      'commits': {
        'href': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/pulls/235/commits'
      },
      'statuses': {
        'href': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/statuses/118fbffb248d4dd98dc07a0297a5af4f81420edf'
      }
    },
    'merged': true,
    'mergeable': null,
    'mergeable_state': 'unknown',
    'merged_by': {
      'login': 'jroling',
      'id': 804,
      'avatar_url': 'https://github.secureserver.net/avatars/u/804?',
      'gravatar_id': '',
      'url': 'https://github.secureserver.net/api/v3/users/jroling',
      'html_url': 'https://github.secureserver.net/jroling',
      'followers_url': 'https://github.secureserver.net/api/v3/users/jroling/followers',
      'following_url': 'https://github.secureserver.net/api/v3/users/jroling/following{/other_user}',
      'gists_url': 'https://github.secureserver.net/api/v3/users/jroling/gists{/gist_id}',
      'starred_url': 'https://github.secureserver.net/api/v3/users/jroling/starred{/owner}{/repo}',
      'subscriptions_url': 'https://github.secureserver.net/api/v3/users/jroling/subscriptions',
      'organizations_url': 'https://github.secureserver.net/api/v3/users/jroling/orgs',
      'repos_url': 'https://github.secureserver.net/api/v3/users/jroling/repos',
      'events_url': 'https://github.secureserver.net/api/v3/users/jroling/events{/privacy}',
      'received_events_url': 'https://github.secureserver.net/api/v3/users/jroling/received_events',
      'type': 'User',
      'site_admin': false,
      'ldap_dn': 'CN=John Roling,OU=Iowa Developers,OU=Users & Groups,OU=GoDaddy,DC=dc1,DC=corp,DC=gd'
    },
    'comments': 0,
    'review_comments': 0,
    'commits': 1,
    'additions': 1,
    'deletions': 1,
    'changed_files': 1
  },
  'repository': {
    'id': 6208,
    'name': 'DCC5',
    'full_name': 'DomainApplications/DCC5',
    'owner': {
      'login': 'DomainApplications',
      'id': 693,
      'avatar_url': 'https://github.secureserver.net/avatars/u/693?',
      'gravatar_id': '',
      'url': 'https://github.secureserver.net/api/v3/users/DomainApplications',
      'html_url': 'https://github.secureserver.net/DomainApplications',
      'followers_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/followers',
      'following_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/following{/other_user}',
      'gists_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/gists{/gist_id}',
      'starred_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/starred{/owner}{/repo}',
      'subscriptions_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/subscriptions',
      'organizations_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/orgs',
      'repos_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/repos',
      'events_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/events{/privacy}',
      'received_events_url': 'https://github.secureserver.net/api/v3/users/DomainApplications/received_events',
      'type': 'Organization',
      'site_admin': false
    },
    'private': false,
    'html_url': 'https://github.secureserver.net/DomainApplications/DCC5',
    'description': '',
    'fork': false,
    'url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5',
    'forks_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/forks',
    'keys_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/keys{/key_id}',
    'collaborators_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/collaborators{/collaborator}',
    'teams_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/teams',
    'hooks_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/hooks',
    'issue_events_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/issues/events{/number}',
    'events_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/events',
    'assignees_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/assignees{/user}',
    'branches_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/branches{/branch}',
    'tags_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/tags',
    'blobs_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/blobs{/sha}',
    'git_tags_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/tags{/sha}',
    'git_refs_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/refs{/sha}',
    'trees_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/trees{/sha}',
    'statuses_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/statuses/{sha}',
    'languages_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/languages',
    'stargazers_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/stargazers',
    'contributors_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/contributors',
    'subscribers_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/subscribers',
    'subscription_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/subscription',
    'commits_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/commits{/sha}',
    'git_commits_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/git/commits{/sha}',
    'comments_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/comments{/number}',
    'issue_comment_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/issues/comments{/number}',
    'contents_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/contents/{+path}',
    'compare_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/compare/{base}...{head}',
    'merges_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/merges',
    'archive_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/{archive_format}{/ref}',
    'downloads_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/downloads',
    'issues_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/issues{/number}',
    'pulls_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/pulls{/number}',
    'milestones_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/milestones{/number}',
    'notifications_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/notifications{?since,all,participating}',
    'labels_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/labels{/name}',
    'releases_url': 'https://github.secureserver.net/api/v3/repos/DomainApplications/DCC5/releases{/id}',
    'created_at': '2014-09-05T14:33:12Z',
    'updated_at': '2015-07-13T22:15:50Z',
    'pushed_at': '2015-07-13T22:15:49Z',
    'git_url': 'git://github.secureserver.net/DomainApplications/DCC5.git',
    'ssh_url': 'git@github.secureserver.net:DomainApplications/DCC5.git',
    'clone_url': 'https://github.secureserver.net/DomainApplications/DCC5.git',
    'svn_url': 'https://github.secureserver.net/DomainApplications/DCC5',
    'homepage': null,
    'size': 7119,
    'stargazers_count': 1,
    'watchers_count': 1,
    'language': 'C#',
    'has_issues': true,
    'has_downloads': true,
    'has_wiki': true,
    'has_pages': false,
    'forks_count': 1,
    'mirror_url': null,
    'open_issues_count': 0,
    'forks': 1,
    'open_issues': 0,
    'watchers': 1,
    'default_branch': 'master'
  },
  'organization': {
    'login': 'DomainApplications',
    'id': 693,
    'url': 'https://github.secureserver.net/api/v3/orgs/DomainApplications',
    'repos_url': 'https://github.secureserver.net/api/v3/orgs/DomainApplications/repos',
    'events_url': 'https://github.secureserver.net/api/v3/orgs/DomainApplications/events',
    'members_url': 'https://github.secureserver.net/api/v3/orgs/DomainApplications/members{/member}',
    'public_members_url': 'https://github.secureserver.net/api/v3/orgs/DomainApplications/public_members{/member}',
    'avatar_url': 'https://github.secureserver.net/avatars/u/693?',
    'description': 'Group for Domain Applications repos.'
  },
  'sender': {
    'login': 'jroling',
    'id': 804,
    'avatar_url': 'https://github.secureserver.net/avatars/u/804?',
    'gravatar_id': '',
    'url': 'https://github.secureserver.net/api/v3/users/jroling',
    'html_url': 'https://github.secureserver.net/jroling',
    'followers_url': 'https://github.secureserver.net/api/v3/users/jroling/followers',
    'following_url': 'https://github.secureserver.net/api/v3/users/jroling/following{/other_user}',
    'gists_url': 'https://github.secureserver.net/api/v3/users/jroling/gists{/gist_id}',
    'starred_url': 'https://github.secureserver.net/api/v3/users/jroling/starred{/owner}{/repo}',
    'subscriptions_url': 'https://github.secureserver.net/api/v3/users/jroling/subscriptions',
    'organizations_url': 'https://github.secureserver.net/api/v3/users/jroling/orgs',
    'repos_url': 'https://github.secureserver.net/api/v3/users/jroling/repos',
    'events_url': 'https://github.secureserver.net/api/v3/users/jroling/events{/privacy}',
    'received_events_url': 'https://github.secureserver.net/api/v3/users/jroling/received_events',
    'type': 'User',
    'site_admin': false,
    'ldap_dn': 'CN=John Roling,OU=Iowa Developers,OU=Users & Groups,OU=GoDaddy,DC=dc1,DC=corp,DC=gd'
  }
}
"; }
      }
    #endregion
    #region TestdataPayload
      public static TestdataPayload TestDataValidPayload
      {
          get
          {
              return new TestdataPayload(TestdataContent);
          }
      }
      public static TestdataPayload TestDataValidErrorPayload
      {
          get
          {
              return new TestdataPayload(TestdataContentWithError);
          }
      }
    #endregion
    public static string BadPushPayload { get; set; }

    public static string TestRepoUrl_Invalid {
        get { return "https://github.test.net/user/app"; }
    }

    public static string StrValidSettingsJson {
        get
        {
            return
                JsonConvert.SerializeObject(StgValidSettings);
        }
    }

    public static Settings StgValidSettings {
        get
        {
            return new Settings
            {
                Branch = new BranchSetting {BaseBranch = "testbasebranch", Level = "testLevel"}
            };
        }
    }

    public static Branch BrnValidBranch
    {
        get { return Branch.CreateFrom(GitHubPushActivity); }
    }
    public static string BrnValidBranchString
    {
        get { return "{'Branch':{'Name':'master','Path':'master','Level':'test', 'BaseBranch':'test2 val'}}"; }
    }
    public static GitHubPullRequest GitHubPullRequest
    {
        get { return new GitHubPullRequest() {head = new RefHead() {@ref = "ref/heads/test"}}; }
    }
      #region TalkService

      public static GenericApiEvent GenericApiDeployEvent
      {
          get
          {
              return new GenericApiEvent
              {
                Name = random
                ,Type="Deploy"
                ,What=random
                ,Where = random
                ,Results = new Dictionary<string, string> { { random, random} }
              };
          }
      }

      #endregion
  }
  public static class pushactivityExtension
  {
      public static void SetBase(this pushactivity obj, string parent)
      {
          obj.base_ref = "ref/heads/" + parent;
      }
      public static void SetName(this pushactivity obj, string name)
      {
          obj.@ref = "ref/heads/" + name;
      }
      public static void SetRepoMasterBranch(this pushactivity obj, string parent)
      {
          obj.repository.master_branch = parent;
      }
  }
}
