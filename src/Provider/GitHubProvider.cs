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
		static readonly ILog _log = LogManager.GetLogger("githubextension");
		private readonly IssueTrackerConfiguration _configuration;
		private readonly IGitHubHttpHelper _httpHelper;


		public GitHubProvider(IssueTrackerConfiguration configuration, IGitHubHttpHelper httpHelper)
		{
			_configuration = configuration;
			_httpHelper = httpHelper;
		}

		public PlasticTask FindSingleTaskById(string taskId)
		{
			// Validate params
			if (string.IsNullOrEmpty(taskId))
			{
				return null;
			}

			// Call API
			var url = _configuration.GetApiUrlForSingleTask(taskId);
			if (!_httpHelper.TryCallApi(url, _configuration.GetAuthToken(), out var apiResponse))
			{
				return null;
			}

			// Process response
			try
			{
				return apiResponse.DeserializeToGithubIssueSingle().MapToPlasticTask();
			}
			catch (Exception e)
			{
				LogException(e);
				return null;
			}
		}

		public List<PlasticTask> FindTasks(string assignee)
		{
			// Call API
			var url = _configuration.GetApiUrlForTasks(assignee);	
			if (!_httpHelper.TryCallApi(url, _configuration.GetAuthToken(), out var apiResponse))			
			{
				return new List<PlasticTask>();
			}

			// Process response
			try
			{
				return apiResponse.DeserializeToGithubIssues().MapToPlasticTasks();
			}
			catch (Exception e)
			{
				LogException(e);
				return null;
			}
		}

		public bool AddTaskComment(string taskId, string message)
		{
			// Call API
			var url = _configuration.GetApiUrlForTaskCommenting(taskId);
			return _httpHelper.TryCallApi(url, _configuration.GetAuthToken(), out _, "POST", $"{{\"body\": \"{ message }\"}}");
		}

		public void TestConnection(IssueTrackerConfiguration byPassConfiguration)
		{
			var url = byPassConfiguration.GetApiUrlForTasks(null);
			_httpHelper.CallApi(url, byPassConfiguration.GetAuthToken());
		}

		private void LogException(Exception e)
		{
			_log.Error($"Exception deserializing: { e.Message}. { e.StackTrace}.");
		}
	}
}
