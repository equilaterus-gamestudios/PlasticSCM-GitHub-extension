using Codice.Client.IssueTracker;
using Equilaterus.GitHubExtension.Common;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Equilaterus.GitHubExtension.Provider
{
	public class GitHubProvider : IGitHubProvider
	{
		private readonly IssueTrackerConfiguration _configuration;
		private readonly IGitHubHttpHelper _httpHelper;


		public GitHubProvider(IssueTrackerConfiguration configuration, IGitHubHttpHelper httpHelper)
		{
			_configuration = configuration;
			_httpHelper = httpHelper;
		}

		public PlasticTask FindSingleTaskById(string taskId)
		{
			if (string.IsNullOrEmpty(taskId))
				return null;

			var url = _configuration.GetApiUrlForSingleTask(taskId);
			var apiResponse = _httpHelper.CallApi(url, $"token { _configuration.GetAuthToken() }");

			return JsonConvert.DeserializeObject<GitHubIssue>(apiResponse)
				.MapToPlasticTask();
		}

		public List<PlasticTask> FindTasks(string assignee)
		{
			var url = _configuration.GetApiUrlForTasks(assignee);
			var apiResponse = _httpHelper.CallApi(url, $"token { _configuration.GetAuthToken() }");

			return JsonConvert.DeserializeObject<GitHubIssue[]>(apiResponse)
				.Select(issue => issue.MapToPlasticTask())
				.ToList();
		}


	}
}
