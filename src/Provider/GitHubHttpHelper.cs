using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Threading.Tasks;

namespace Equilaterus.GitHubExtension.Provider
{
	public class GitHubHttpHelper : IGitHubHttpHelper
	{
		static readonly ILog _log = LogManager.GetLogger("githubextension");

		public string CallApi(string targetUrl, string token)
		{
			var request = (HttpWebRequest)WebRequest.Create(targetUrl);
			request.Method = "GET";
			request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
			request.Accept = "application/vnd.github.v3+json";
			if (!string.IsNullOrEmpty(token))
			{
				request.Headers.Add("Authorization", $"token { token }");
			}

			
			using (var resp = (HttpWebResponse)request.GetResponse())
			using (var reader = new StreamReader(resp.GetResponseStream()))
			{
				return reader.ReadToEnd();
			}		
		}

		public bool TryCallApi(string targetUrl, string token, out string apiResponse)
		{
			try
			{
				apiResponse = CallApi(targetUrl, token);
				return true;
			}
			catch (Exception e)
			{
				LogException(e, targetUrl);
				apiResponse = string.Empty;
				return false;
			}
		}

		private void LogException(Exception e, string targetUrl)
		{
			_log.ErrorFormat(
				"Unable to perform request on URI {0}: {1}", targetUrl, e.Message);
			_log.DebugFormat(
				"Stack trace:{0}{1}", Environment.NewLine, e.StackTrace);
		}
	}
}
