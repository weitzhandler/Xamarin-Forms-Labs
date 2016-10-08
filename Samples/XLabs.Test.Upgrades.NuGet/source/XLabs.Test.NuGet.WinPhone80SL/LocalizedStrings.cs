using XLabs.Test.NuGet.WinPhone80SL.Resources;

namespace XLabs.Test.NuGet.WinPhone80SL
{
	/// <summary>
	/// Provides access to string resources.
	/// </summary>
	public class LocalizedStrings
	{
		private static AppResources _localizedResources = new AppResources();

		public AppResources LocalizedResources { get { return _localizedResources; } }
	}
}