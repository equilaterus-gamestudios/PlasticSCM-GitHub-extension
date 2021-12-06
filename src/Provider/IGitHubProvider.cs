using Codice.Client.IssueTracker;
using System.Collections.Generic;

namespace Equilaterus.GitHubExtension.Provider
{
	public interface IGitHubProvider
	{
		PlasticTask FindSingleTaskById(string taskId);

		List<PlasticTask> FindTasks(string assignee);

		bool AddTaskComment(string taskId, string message);

		void TestConnection(IssueTrackerConfiguration byPassConfiguration);
	}
}