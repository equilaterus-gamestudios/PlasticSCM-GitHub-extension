using Codice.Client.IssueTracker;
using Equilaterus.GitHubExtension.Common;
using System.Collections.Generic;

namespace Equilaterus.GitHubExtension
{
    public class SampleExtensionFactory : IPlasticIssueTrackerExtensionFactory
    {
        public IssueTrackerConfiguration GetConfiguration(IssueTrackerConfiguration storedConfiguration)
        {
			//
			// Extension parameters
			// 

			var parameters = new List<IssueTrackerConfigurationParameter>
			{
				new IssueTrackerConfigurationParameter
				{
					Name = Globals.USER_KEY,
					Value = storedConfiguration.GetValidParameterValue(Globals.USER_KEY, Globals.DEFAULT_USER_KEY),
					Type = IssueTrackerConfigurationParameterType.User,
					IsGlobal = false
				},
				new IssueTrackerConfigurationParameter
				{
					Name = Globals.BRANCH_PREFIX_KEY,
					Value = storedConfiguration.GetValidParameterValue(Globals.BRANCH_PREFIX_KEY, Globals.DEFAULT_BRANCH_PREFIX_KEY),
					Type = IssueTrackerConfigurationParameterType.BranchPrefix,
					IsGlobal = true
				},
				new IssueTrackerConfigurationParameter
				{
					Name = Globals.PROJECT_OWNER,
					Value = storedConfiguration.GetValidParameterValue(Globals.PROJECT_OWNER, Globals.DEFAULT_PROJECT_OWNER),
					Type = IssueTrackerConfigurationParameterType.Text,
					IsGlobal = false
				},
				new IssueTrackerConfigurationParameter
				{
					Name = Globals.PROJECT_NAME,
					Value = storedConfiguration.GetValidParameterValue(Globals.PROJECT_NAME, Globals.DEFAULT_PROJECT_NAME),
					Type = IssueTrackerConfigurationParameterType.Text,
					IsGlobal = false
				},
				new IssueTrackerConfigurationParameter
				{
					Name = Globals.AUTH_TOKEN,
					Value = storedConfiguration.GetValidParameterValue(Globals.AUTH_TOKEN, Globals.DEFAULT_AUTH_TOKEN),
					Type = IssueTrackerConfigurationParameterType.Text,
					IsGlobal = false
				}
			};

			//
			// Other params
			//

			var workingMode = storedConfiguration.GetWorkingMode();

            return new IssueTrackerConfiguration(workingMode, parameters);
        }

        public IPlasticIssueTrackerExtension GetIssueTrackerExtension(IssueTrackerConfiguration configuration)
        {
            return new GitHubExtension(configuration);
        }

        public string GetIssueTrackerName()
        {
            return "GitHub";
        }
    }
}
