namespace Equilaterus.GitHubExtension.Provider
{
	public interface IGitHubHttpHelper
	{
		string CallApi(string targetUrl, string token, int timeout, bool linux, string method = "GET", string body = null);

		bool TryCallApi(string targetUrl, string token, out string apiResponse, int timeout, bool linux, 
			string method="GET", string body=null);
	}
}