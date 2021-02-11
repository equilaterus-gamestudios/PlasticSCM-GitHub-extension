using Codice.Client.IssueTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.GitHubExtension.Common
{
	public static class IssueTrackerConfigurationExtensions
	{
		//
		// Configuration getters
		//

		public static string GetBranchPrefix(this IssueTrackerConfiguration configuration)	
			=> configuration.GetValue(Globals.BRANCH_PREFIX_KEY);
		
		public static string GetUserKey(this IssueTrackerConfiguration configuration)
			=> configuration.GetValue(Globals.USER_KEY);

		public static string GetProjectOwner(this IssueTrackerConfiguration configuration)
			=> configuration.GetValue(Globals.PROJECT_OWNER);

		public static string GetProjectName(this IssueTrackerConfiguration configuration)
			=> configuration.GetValue(Globals.PROJECT_NAME);

		public static string GetAuthToken(this IssueTrackerConfiguration configuration)
			=> configuration.GetValue(Globals.AUTH_TOKEN);


		//
		// URL Helpers
		//

		// API Urls

		public static string GetApiUrlForTasks(this IssueTrackerConfiguration configuration, string assignee)
		{
			string assigneeFilter = !string.IsNullOrEmpty(assignee)
				? $"assignee={ assignee.MapToCleanAssignee() }&"
				: string.Empty;

			return $"{ configuration.GetApiUrl() }/issues?{ assigneeFilter }state=open";
		}

		public static string GetApiUrlForSingleTask(this IssueTrackerConfiguration configuration, string taskId)
			=> $"{ configuration.GetApiUrl() }/issues/{ taskId }";
		
		public static string GetApiUrl(this IssueTrackerConfiguration configuration)
			=> $"{ Globals.GITHUB_API }{ configuration.GetProjectOwner() }/{ configuration.GetProjectName() }";

		
		// Browsing Urls

		public static string GetUrlForTask(this IssueTrackerConfiguration configuration, string taskId)
			=> $"{ configuration.GetUrl() }/issues/{ taskId }";

		public static string GetUrl(this IssueTrackerConfiguration configuration)
			=> $"{ Globals.GITHUB_URL }{ configuration.GetProjectOwner() }/{ configuration.GetProjectName() }";


		//
		// Misc
		//

		public static ExtensionWorkingMode GetWorkingMode(this IssueTrackerConfiguration configuration)
		{
			if (configuration == null)
				return ExtensionWorkingMode.TaskOnBranch;

			if (configuration.WorkingMode == ExtensionWorkingMode.None)
				return ExtensionWorkingMode.TaskOnBranch;

			return configuration.WorkingMode;
		}

		public static string GetValidParameterValue(
			this IssueTrackerConfiguration configuration, string paramName, string defaultValue)
		{
			var value = configuration?.GetValue(paramName);
			if (string.IsNullOrEmpty(value))
				return defaultValue;
			return value;
		}
	}
}
