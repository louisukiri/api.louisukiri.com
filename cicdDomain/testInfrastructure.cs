using cicdDomain.cicd.domain.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cicdDomain
{
  public class testInfrastructure
  {
    public static string random
    {
      get { return Guid.NewGuid().ToString(); }
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
    #region Push Content
    public static string GitHubPushContent = @"{
  'ref': 'refs/heads/Add-Module-Events',
  'before': '36c0ab88025995d0562d3fb016f126b75306c93f',
  'after': '8dad01851588d09c7ac66c2a7c80801306081d48',
  'created': false,
  'deleted': false,
  'forced': false,
  'base_ref': null,
  'compare': 'https://github.com/louisukiri/paper-angel/compare/36c0ab880259...8dad01851588',
  'commits': [
    {
      'id': '8dad01851588d09c7ac66c2a7c80801306081d48',
      'distinct': true,
      'message': 'commit\n\ncommited',
      'timestamp': '2015-06-09T23:22:37-05:00',
      'url': 'https://github.com/louisukiri/paper-angel/commit/8dad01851588d09c7ac66c2a7c80801306081d48',
      'author': {
        'name': 'louisukiri',
        'email': 'louisukiri@gmail.com',
        'username': 'louisukiri'
      },
      'committer': {
        'name': 'louisukiri',
        'email': 'louisukiri@gmail.com',
        'username': 'louisukiri'
      },
      'added': [
        'DNN Platform/Syndication/obj/Debug/DesignTimeResolveAssemblyReferencesInput.cache'
      ],
      'removed': [

      ],
      'modified': [

      ]
    }
  ],
  'head_commit': {
    'id': '8dad01851588d09c7ac66c2a7c80801306081d48',
    'distinct': true,
    'message': 'commit\n\ncommited',
    'timestamp': '2015-06-09T23:22:37-05:00',
    'url': 'https://github.com/louisukiri/paper-angel/commit/8dad01851588d09c7ac66c2a7c80801306081d48',
    'author': {
      'name': 'louisukiri',
      'email': 'louisukiri@gmail.com',
      'username': 'louisukiri'
    },
    'committer': {
      'name': 'louisukiri',
      'email': 'louisukiri@gmail.com',
      'username': 'louisukiri'
    },
    'added': [
      'DNN Platform/Syndication/obj/Debug/DesignTimeResolveAssemblyReferencesInput.cache'
    ],
    'removed': [

    ],
    'modified': [

    ]
  },
  'repository': {
    'id': 20598016,
    'name': 'paper-angel',
    'full_name': 'louisukiri/paper-angel',
    'owner': {
      'name': 'louisukiri',
      'email': 'louisukiri@gmail.com'
    },
    'private': false,
    'html_url': 'https://github.com/louisukiri/paper-angel',
    'description': '',
    'fork': false,
    'url': 'https://github.com/louisukiri/paper-angel',
    'forks_url': 'https://api.github.com/repos/louisukiri/paper-angel/forks',
    'keys_url': 'https://api.github.com/repos/louisukiri/paper-angel/keys{/key_id}',
    'collaborators_url': 'https://api.github.com/repos/louisukiri/paper-angel/collaborators{/collaborator}',
    'teams_url': 'https://api.github.com/repos/louisukiri/paper-angel/teams',
    'hooks_url': 'https://api.github.com/repos/louisukiri/paper-angel/hooks',
    'issue_events_url': 'https://api.github.com/repos/louisukiri/paper-angel/issues/events{/number}',
    'events_url': 'https://api.github.com/repos/louisukiri/paper-angel/events',
    'assignees_url': 'https://api.github.com/repos/louisukiri/paper-angel/assignees{/user}',
    'branches_url': 'https://api.github.com/repos/louisukiri/paper-angel/branches{/branch}',
    'tags_url': 'https://api.github.com/repos/louisukiri/paper-angel/tags',
    'blobs_url': 'https://api.github.com/repos/louisukiri/paper-angel/git/blobs{/sha}',
    'git_tags_url': 'https://api.github.com/repos/louisukiri/paper-angel/git/tags{/sha}',
    'git_refs_url': 'https://api.github.com/repos/louisukiri/paper-angel/git/refs{/sha}',
    'trees_url': 'https://api.github.com/repos/louisukiri/paper-angel/git/trees{/sha}',
    'statuses_url': 'https://api.github.com/repos/louisukiri/paper-angel/statuses/{sha}',
    'languages_url': 'https://api.github.com/repos/louisukiri/paper-angel/languages',
    'stargazers_url': 'https://api.github.com/repos/louisukiri/paper-angel/stargazers',
    'contributors_url': 'https://api.github.com/repos/louisukiri/paper-angel/contributors',
    'subscribers_url': 'https://api.github.com/repos/louisukiri/paper-angel/subscribers',
    'subscription_url': 'https://api.github.com/repos/louisukiri/paper-angel/subscription',
    'commits_url': 'https://api.github.com/repos/louisukiri/paper-angel/commits{/sha}',
    'git_commits_url': 'https://api.github.com/repos/louisukiri/paper-angel/git/commits{/sha}',
    'comments_url': 'https://api.github.com/repos/louisukiri/paper-angel/comments{/number}',
    'issue_comment_url': 'https://api.github.com/repos/louisukiri/paper-angel/issues/comments{/number}',
    'contents_url': 'https://api.github.com/repos/louisukiri/paper-angel/contents/{+path}',
    'compare_url': 'https://api.github.com/repos/louisukiri/paper-angel/compare/{base}...{head}',
    'merges_url': 'https://api.github.com/repos/louisukiri/paper-angel/merges',
    'archive_url': 'https://api.github.com/repos/louisukiri/paper-angel/{archive_format}{/ref}',
    'downloads_url': 'https://api.github.com/repos/louisukiri/paper-angel/downloads',
    'issues_url': 'https://api.github.com/repos/louisukiri/paper-angel/issues{/number}',
    'pulls_url': 'https://api.github.com/repos/louisukiri/paper-angel/pulls{/number}',
    'milestones_url': 'https://api.github.com/repos/louisukiri/paper-angel/milestones{/number}',
    'notifications_url': 'https://api.github.com/repos/louisukiri/paper-angel/notifications{?since,all,participating}',
    'labels_url': 'https://api.github.com/repos/louisukiri/paper-angel/labels{/name}',
    'releases_url': 'https://api.github.com/repos/louisukiri/paper-angel/releases{/id}',
    'created_at': 1402160630,
    'updated_at': '2014-06-07T18:11:10Z',
    'pushed_at': 1433910187,
    'git_url': 'git://github.com/louisukiri/paper-angel.git',
    'ssh_url': 'git@github.com:louisukiri/paper-angel.git',
    'clone_url': 'https://github.com/louisukiri/paper-angel.git',
    'svn_url': 'https://github.com/louisukiri/paper-angel',
    'homepage': null,
    'size': 48752,
    'stargazers_count': 0,
    'watchers_count': 0,
    'language': 'C#',
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
    'name': 'louisukiri',
    'email': 'louisukiri@gmail.com'
  },
  'sender': {
    'login': 'louisukiri',
    'id': 3752355,
    'avatar_url': 'https://avatars.githubusercontent.com/u/3752355?v=3',
    'gravatar_id': '',
    'url': 'https://api.github.com/users/louisukiri',
    'html_url': 'https://github.com/louisukiri',
    'followers_url': 'https://api.github.com/users/louisukiri/followers',
    'following_url': 'https://api.github.com/users/louisukiri/following{/other_user}',
    'gists_url': 'https://api.github.com/users/louisukiri/gists{/gist_id}',
    'starred_url': 'https://api.github.com/users/louisukiri/starred{/owner}{/repo}',
    'subscriptions_url': 'https://api.github.com/users/louisukiri/subscriptions',
    'organizations_url': 'https://api.github.com/users/louisukiri/orgs',
    'repos_url': 'https://api.github.com/users/louisukiri/repos',
    'events_url': 'https://api.github.com/users/louisukiri/events{/privacy}',
    'received_events_url': 'https://api.github.com/users/louisukiri/received_events',
    'type': 'User',
    'site_admin': false
  }
}
";
    #endregion
  }
}
