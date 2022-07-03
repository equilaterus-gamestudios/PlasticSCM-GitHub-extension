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
using Newtonsoft.Json;
using GitHubExtension.Common;

namespace Equilaterus.GitHubExtension.Provider
{
	public class GitHubHttpHelper : IGitHubHttpHelper
	{
		static readonly ILog _log = LogManager.GetLogger("githubextension");

		public HttpWebRequest GetApiRequest(string targetUrl, string token, string method = "GET")
		{
			var request = (HttpWebRequest)WebRequest.Create(targetUrl);
			request.Method = method;
			request.UserAgent = Globals.API_USER_AGENT;
			request.Accept = Globals.API_ACCEPT;
			if (!string.IsNullOrEmpty(token))
			{
				request.Headers.Add("Authorization", $"token { token }");
			}
			
			return request;
		}

		public string ExecuteHttpWebRequest(HttpWebRequest request, string body, int timeout)
		{
			// Body should be written right here, because it can thrown a WebException on Mono-Linux
			if (!string.IsNullOrEmpty(body))
			{
				var bodyData = Encoding.Default.GetBytes(body);
				request.ContentLength = bodyData.Length;
				request.Timeout = timeout * 1000; // secs to ms

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

		public string CallApi(string targetUrl, string token, int timeout, bool linux, string method = "GET", string body = null)
		{
			// Create basic request data
			var request = GetApiRequest(targetUrl, token, method);

			// Use Linux compatibility mode?
			if (!linux)
			{
				// Default runtime may have problems with SSL certificates on Linux
				return ExecuteHttpWebRequest(request, body, timeout);
			}
			else
			{
				// Curl requests don't have that kind of problems on Linux
				_log.DebugFormat("Using curl (may fail beforehand on any OS != linux)...");
				return BashUtils.ExecuteAsCurl(request, body, timeout, _log);
			}
		}

		public bool TryCallApi(string targetUrl, string token, out string apiResponse, int timeout, bool linux,
			string method = "GET", string body = null)
		{
			try
			{
				apiResponse = CallApi(targetUrl, token, timeout, linux, method, body);
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
			_log.DebugFormat(
				"Exception data:{0}{1}", Environment.NewLine, JsonConvert.SerializeObject(e)
			);
		}
	}
}
