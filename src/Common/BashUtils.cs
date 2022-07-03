using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GitHubExtension.Common
{
	static class BashUtils
	{
		public static string ExecuteCommand(string command, ILog log)
		{
			var proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "/bin/bash",
					Arguments = $"-c \"{ command.Replace("\"", "\\\"") }\"",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};

			proc.Start();
			var result = proc.StandardOutput.ReadToEnd();
			if (log != null)
			{
				try
				{
					log.DebugFormat("CommandError: {0}", proc.StandardError.ReadToEnd());
				}
				catch (Exception )
				{ 
					// Do nothing
				}
			}

			return result;
		}

		public static string ExecuteAsCurl(HttpWebRequest request, string body, int timeout, ILog log = null)
		{
			StringBuilder headers = new StringBuilder();
			foreach (string header in request.Headers.Keys)
			{
				headers.Append($" --header '{header}: {request.Headers[header]}'");
			}

			var dataRaw = string.IsNullOrEmpty(body)
				? string.Empty
				: $"--data-raw '{ body }'";

			var curl = $"curl --connect-timeout { timeout } --request { request.Method } '{ request.RequestUri }' { headers } { dataRaw }";
			
			return ExecuteCommand(curl, log);
		}
	}
}
