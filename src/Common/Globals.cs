using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.GitHubExtension.Common
{
	public static class Globals
	{
		//
		// Global configuration values
		//

		public const string BRANCH_PREFIX_KEY = "Branch prefix";
		public const string USER_KEY = "Username";
		public const string PROJECT_OWNER = "Project owner";
		public const string PROJECT_NAME = "Project name";
		public const string AUTH_TOKEN = "Authentication token";

		//
		// Default extension values
		//

		public const string DEFAULT_BRANCH_PREFIX_KEY = "task";
		public const string DEFAULT_USER_KEY = "githubusername";
		public const string DEFAULT_PROJECT_OWNER = "project-owner";
		public const string DEFAULT_PROJECT_NAME = "project-name";
		public const string DEFAULT_AUTH_TOKEN = "";

		//
		// Services URLs
		//

		public const string GITHUB_API = "https://api.github.com/repos/";
		public const string GITHUB_URL = "https://github.com/";
	}
}
