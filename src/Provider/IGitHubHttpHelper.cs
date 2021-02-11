namespace Equilaterus.GitHubExtension.Provider
{
	public interface IGitHubHttpHelper
	{
		string CallApi(string targetUri, string authorization);
	}
}