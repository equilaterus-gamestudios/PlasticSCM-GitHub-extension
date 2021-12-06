using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Text;
using Equilaterus.GitHubExtension.Common;

namespace Equilaterus.GitHubExtension.Provider
{
	public class GitHubHttpHelper : IGitHubHttpHelper
	{
		static readonly ILog _log = LogManager.GetLogger("githubextension");

		public string CallApi(string targetUrl, string token, string method = "GET", string body = null)
		{
			var request = (HttpWebRequest)WebRequest.Create(targetUrl);
			request.Method = method;
			request.UserAgent = Globals.API_USER_AGENT;
			request.Accept = Globals.API_ACCEPT;
			if (!string.IsNullOrEmpty(token))
			{
				request.Headers.Add("Authorization", $"token { token }");
			}

			if (!string.IsNullOrEmpty(body))
			{
				var bodyData = Encoding.Default.GetBytes(body);
				request.ContentLength = bodyData.Length;

				using (var requestBody = request.GetRequestStream())
				{
					requestBody.Write(bodyData, 0, bodyData.Length);
				}
			}
			
			using (var resp = (HttpWebResponse)request.GetResponse())
			using (var reader = new StreamReader(resp.GetResponseStream()))
			{
				return reader.ReadToEnd();
			}		
		}

		public bool TryCallApi(string targetUrl, string token, out string apiResponse, string method = "GET", string body = null)
		{
			try
			{
				apiResponse = CallApi(targetUrl, token, method, body);
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
