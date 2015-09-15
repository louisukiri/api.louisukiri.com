using System;
using System.Text.RegularExpressions;

namespace cicd.domain.context.trigger.entity
{
    public class VerControlServer
    {
        public VerControlServer(string id, string token)
        {
            string owner;
            Id = id;
            if (!id.EndsWith(".git"))
            {
                throw new ArgumentException("Id");
            }
            //moved the following from inside the GitUri Getter so that you dont
            //run regex everytime GitUri is needed
              var regex = new Regex(@"\w+@([\w\.]+):([\w\.]+)/([\w\.]+)\.git");
              if (regex.IsMatch(Id))
              {
                  var m = regex.Match(Id);
                  AppName = m.Groups[3].Value;
                  HostName = "https://" + m.Groups[1].Value;
                  owner = m.Groups[2].Value;
                  _normalizedIdString = string.Format("https://{0}/{1}/{2}.git", m.Groups[1], m.Groups[2], m.Groups[3]);
              }
              else
              {
                  regex = new Regex(@"(http[s])://([\w\.]+)/([\w\.]+)/([\w\.]+)\.git");
                  if (!regex.IsMatch(Id))
                  {
                      throw new UriFormatException();
                  }
                  Match m = regex.Match(Id);

                  AppName = m.Groups[4].Value;
                  owner = m.Groups[3].Value;
                  HostName = m.Groups[1].Value + "://" + m.Groups[2].Value;
                  _idStringToSsh = string.Format("git@{0}:{1}/{2}.git", m.Groups[2].Value, owner, AppName);
              }
            Auth = new VersionControlAuth(owner, token);
        }
        public string Id { get;
            private set;
        }
        public VersionControlAuth Auth { get; private set; }
        //this is only set if the originally provided string is SSH
        private readonly string _normalizedIdString;
        public string GitFullUriString
        {
            get
            {
                return _normalizedIdString??Id;
            }
        }

        public string HostName { get; private set; }

        public string AppName { get; private set; }

        private readonly string _idStringToSsh;
        public string GitSshUrlString {
            get
            {
                return _idStringToSsh??Id;
            }
        }
    }
    public struct VersionControlAuth
    {
        public string Owner { get; private set; }
        public string Token { get; private set; }

        public VersionControlAuth(string owner, string token)
        : this()
        {
            Owner = owner;
            Token = token;
        }
    }
}
