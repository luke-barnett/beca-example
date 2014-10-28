using Beca.Xamarin.Example.Core.Services;
using Beca.Xamarin.Example.WP.Core.Services;
using Cirrious.CrossCore;

namespace Beca.Xamarin.Example.WP.Core
{
	public class WindowsPhoneApp : Beca.Xamarin.Example.Core.App
	{
		public WindowsPhoneApp()
			: base()
		{
			Mvx.RegisterType<IPathsService, WindowsPhonePathsServices>();
		}
	}
}