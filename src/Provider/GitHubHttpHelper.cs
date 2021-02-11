using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.GitHubExtension.Provider
{
	public class GitHubHttpHelper : IGitHubHttpHelper
	{
		static readonly ILog _log = LogManager.GetLogger("githubextension");

		public string CallApi(string targetUri, string authorization)
		{
			var request = (HttpWebRequest)WebRequest.Create(targetUri);
			request.Method = "GET";
			request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
			request.Accept = "application/vnd.github.v3+json";
			if (!string.IsNullOrEmpty(authorization))
			{
				request.Headers.Add("Authorization", authorization);
			}

			try
			{
				using (var resp = (HttpWebResponse)request.GetResponse())
				using (var reader = new StreamReader(resp.GetResponseStream()))
				{
					return reader.ReadToEnd();
				}
			}
			catch (Exception e)
			{
				_log.ErrorFormat(
					"Unable to perform request on URI {0}: {1}", targetUri, e.Message);
				_log.DebugFormat(
					"Stack trace:{0}{1}", Environment.NewLine, e.StackTrace);
				return string.Empty;
			}
		}
	}
}
