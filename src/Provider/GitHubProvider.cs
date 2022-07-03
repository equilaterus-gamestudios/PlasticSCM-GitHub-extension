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
		private int _timeout;
		private bool _linux;

		public GitHubProvider(IssueTrackerConfiguration configuration, IGitHubHttpHelper httpHelper)
		{
			_configuration = configuration;
			_httpHelper = httpHelper;

			// Set global config
			_timeout = configuration.GetTimeout(_log);
			_linux = configuration.GetLinux(_log);
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
			if (!_httpHelper.TryCallApi(url, _configuration.GetAuthToken(), out var apiResponse, _timeout, _linux))
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
			if (!_httpHelper.TryCallApi(url, _configuration.GetAuthToken(), out var apiResponse, _timeout, _linux))			
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
			var body = $"{{\"body\": \"{ message }\"}}";
			return _httpHelper.TryCallApi(url, _configuration.GetAuthToken(), out _, _timeout, _linux, "POST", body);
		}

		public void TestConnection(IssueTrackerConfiguration byPassConfiguration)
		{
			var url = byPassConfiguration.GetApiUrlForTasks(null);
			_httpHelper.CallApi(url, byPassConfiguration.GetAuthToken(), _timeout, _linux);
		}

		private void LogException(Exception e)
		{
			_log.Error($"Exception deserializing: { e.Message}. { e.StackTrace}.");
		}
	}
}
