namespace Equilaterus.GitHubExtension.Provider
{
	public interface IGitHubHttpHelper
	{
		string CallApi(string targetUrl, string token);

		bool TryCallApi(string targetUrl, string token, out string apiResponse);
	}
}