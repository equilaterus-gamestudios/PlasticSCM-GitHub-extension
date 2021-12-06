namespace Equilaterus.GitHubExtension.Provider
{
	public interface IGitHubHttpHelper
	{
		string CallApi(string targetUrl, string token, string method = "GET", string body = null);

		bool TryCallApi(string targetUrl, string token, out string apiResponse, string method="GET", string body=null);
	}
}