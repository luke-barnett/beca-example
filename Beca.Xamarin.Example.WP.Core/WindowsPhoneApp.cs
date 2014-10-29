using Beca.Xamarin.Example.Core.Services;
using Beca.Xamarin.Example.WP.Core.Services;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.PictureChooser;

namespace Beca.Xamarin.Example.WP.Core
{
	public class WindowsPhoneApp : Beca.Xamarin.Example.Core.App
	{
		public WindowsPhoneApp()
			: base()
		{
			Mvx.RegisterType<IPathsService, WindowsPhonePathsServices>();
			Mvx.RegisterType<IMvxPictureChooserTask, MvxPictureChooserTask>();
		}
	}
}