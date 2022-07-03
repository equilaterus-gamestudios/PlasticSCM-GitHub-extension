using Codice.Client.IssueTracker;
using log4net;
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

		public static string GetPlasticWebURL(this IssueTrackerConfiguration configuration)
			=> configuration.GetValue(Globals.PLASTIC_WEBUI_URL);

		public static int GetTimeout(this IssueTrackerConfiguration configuration, ILog log)
		{
			var timeoutStr = configuration.GetValue(Globals.TIMEOUT);

			int timeout;
			if (int.TryParse(timeoutStr, out timeout))
			{
				return timeout;
			}
			else 
			{
				log.ErrorFormat("Configuration error: 'Timeout: {0}' is not an integer. Using default value (100).", timeoutStr);
				return 100;
			}
		}

		public static bool GetLinux(this IssueTrackerConfiguration configuration, ILog log)
		{
			var linuxStr = configuration.GetValue(Globals.LINUX);

			bool linux;
			if (bool.TryParse(linuxStr, out linux))
			{
				return linux;
			}
			else
			{
				log.ErrorFormat("Configuration error: 'Linux: {0}' is not a boolean. Using default value (false).", linux);
				return false;
			}
		}

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

		public static string GetApiUrlForTaskCommenting(this IssueTrackerConfiguration configuration, string taskId)
			=> $"{ configuration.GetApiUrl() }/issues/{ taskId }/comments";

		public static string GetApiUrlForSingleTask(this IssueTrackerConfiguration configuration, string taskId)
			=> $"{ configuration.GetApiUrl() }/issues/{ taskId }";
		
		public static string GetApiUrl(this IssueTrackerConfiguration configuration)
			=> $"{ Globals.GITHUB_API }{ configuration.GetProjectOwner() }/{ configuration.GetProjectName() }";

				
		// Browsing Urls

		public static string GetUrlForTask(this IssueTrackerConfiguration configuration, string taskId)
			=> $"{ configuration.GetUrl() }/issues/{ taskId }";

		public static string GetUrl(this IssueTrackerConfiguration configuration)
			=> $"{ Globals.GITHUB_URL }{ configuration.GetProjectOwner() }/{ configuration.GetProjectName() }";

		public static string GetPlasticChangesetURL(this IssueTrackerConfiguration configuration, PlasticChangeset changeset)
		{
			var customUrl = configuration.GetPlasticWebURL()?.Trim();			
			if (!string.IsNullOrEmpty(customUrl))
				return $"{ configuration.GetPlasticWebURL().Trim() }{ changeset.Id }";

			return string.Empty;
		}

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
