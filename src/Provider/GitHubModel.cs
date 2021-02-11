using Newtonsoft.Json;

namespace Equilaterus.GitHubExtension.Provider
{
    public class GitHubIssue
	{
        [JsonProperty("number")]
        public int Number { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("assignee")]
        public GitHubIssueAssignee Assignee { get; set; }
    }

    public class GitHubIssueAssignee
	{
        [JsonProperty("login")]
        public string Login { get; set; }
    }
}
