using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using log4net;
using Newtonsoft.Json;
using Equilaterus.GitHubExtension.Common;
using Codice.Client.IssueTracker;
using Equilaterus.GitHubExtension.Provider;

namespace Equilaterus.GitHubExtension
{
    public class GitHubExtension : IPlasticIssueTrackerExtension
    {
        private readonly IssueTrackerConfiguration _configuration;
		private readonly GitHubProvider _provider;

		internal GitHubExtension(IssueTrackerConfiguration configuration)
        {
			_configuration = configuration;
			_provider = new GitHubProvider(configuration, new GitHubHttpHelper());
		}

        public void Connect()
        {
            // No action needed
        }

        public void Disconnect()
        {
            // No action needed
        }

        public string GetExtensionName()
        {
            return "Github Issues Extension";
        }

        public List<PlasticTask> GetPendingTasks(string assignee)
        {
            if (string.IsNullOrEmpty(assignee))
                return new List<PlasticTask>();

            return _provider.FindTasks(assignee);
        }

        public List<PlasticTask> GetPendingTasks()
        {
            return _provider.FindTasks(null);
        }

        public PlasticTask GetTaskForBranch(string fullBranchName)
        {
            return _provider.FindSingleTaskById(
				fullBranchName.MapTaskIdFromFullBranchName(_configuration.GetBranchPrefix())
			);
        }

        public Dictionary<string, PlasticTask> GetTasksForBranches(List<string> fullBranchNames)
        {
            var result = new Dictionary<string, PlasticTask>();
            foreach(var fullBranchName in fullBranchNames)
            {
                var taskId = fullBranchName.MapTaskIdFromFullBranchName(_configuration.GetBranchPrefix());
                result.Add(fullBranchName, _provider.FindSingleTaskById(taskId));
            }
            return result;
        }

        public List<PlasticTask> LoadTasks(List<string> taskIds)
        {
            var result = new List<PlasticTask>();

            foreach (var taskId in taskIds)
            {
                PlasticTask loadedTask = _provider.FindSingleTaskById(taskId);
                if (loadedTask == null)
                    continue;
                result.Add(loadedTask);
            }

            return result;
        }

        public void LogCheckinResult(PlasticChangeset changeset, List<PlasticTask> tasks)
        {
            // Not supported
        }

        public void MarkTaskAsOpen(string taskId, string assignee)
        {
            // Not supported
        }

        public void OpenTaskExternally(string taskId)
        {
            Process.Start(_configuration.GetUrlForTask(taskId));
        }

        public bool TestConnection(IssueTrackerConfiguration configuration)
        {
			// TODO: do a real test connection
            return true;
        }

        public void UpdateLinkedTasksToChangeset(PlasticChangeset changeset, List<string> tasks)
        {
            // Not supported
        }
    }
}
