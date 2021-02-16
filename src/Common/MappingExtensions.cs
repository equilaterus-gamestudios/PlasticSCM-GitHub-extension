using Codice.Client.IssueTracker;
using Equilaterus.GitHubExtension.Provider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.GitHubExtension.Common
{
	public static class MappingExtensions
	{
		//
		// GitHub mappers
		//

		public static PlasticTask MapToPlasticTask(this GitHubIssue issue)
		{
			if (issue == null)
				return null;

			return new PlasticTask()
			{
				Id = issue.Number.ToString(),
				Owner = issue.Assignee?.Login.ToString(),
				Title = issue.Title,
				Description = issue.Body,
				Status = "working" // TODO: Review possible values
			};
		}

		public static List<PlasticTask> MapToPlasticTasks(this GitHubIssue[] issues)
			=> issues
				.Select(issue => issue.MapToPlasticTask())
				.ToList();

		public static GitHubIssue DeserializeToGithubIssueSingle(this string apiResponse)
			=> JsonConvert.DeserializeObject<GitHubIssue>(apiResponse);

		public static GitHubIssue[] DeserializeToGithubIssues(this string apiResponse)
			=> JsonConvert.DeserializeObject<GitHubIssue[]>(apiResponse);

		//
		// Task naming mappings
		//

		public static string MapShortBranchName(this string fullBranchName)
		{
			int lastSeparatorIndex = fullBranchName.LastIndexOf('/');

			if (lastSeparatorIndex < 0)
				return fullBranchName;

			if (lastSeparatorIndex == fullBranchName.Length - 1)
				return string.Empty;

			return fullBranchName.Substring(lastSeparatorIndex + 1);
		}

		public static string MapTaskId(this string branchName, string prefix)
		{
			if (string.IsNullOrEmpty(prefix))
				return branchName;

			if (!branchName.StartsWith(branchName) || branchName == prefix)
				return string.Empty;

			return branchName.Substring(prefix.Length);
		}

		public static string MapTaskIdFromFullBranchName(this string fullBranchName, string prefix)
		{
			var branchName = fullBranchName.MapShortBranchName();
			return branchName.MapTaskId(prefix);
		}


		//
		// Misc
		//

		public static string MapToCleanAssignee(this string assignee)
		{
			return assignee.Contains("@") 
				? assignee.Substring(0, assignee.IndexOf("@"))
				: assignee;
		}
	}
}
