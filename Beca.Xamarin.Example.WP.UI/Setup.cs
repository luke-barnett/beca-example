using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsCommon.Platform;
using Windows.UI.Xaml.Controls;

namespace Beca.Xamarin.Example.WP.UI
{
	public class Setup : MvxWindowsSetup
	{
		public Setup(Frame rootFrame)
			: base(rootFrame)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			return new Core.WindowsPhoneApp();
		}
	}
}