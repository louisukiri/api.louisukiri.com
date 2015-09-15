namespace cicd.domain.context.trigger.entity
{
    public class GitHubPullRequest
    {
        public RefHead head { get; set; }
        public RefHead @base { get; set; }
    }
    public class RefHead
    {
        public string @ref { get; set; }
        public string sha { get; set; }
        public string label { get; set; }
        public VersionControlUser user { get; set; }
    }
}
