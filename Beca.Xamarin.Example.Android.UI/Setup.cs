using Android.Content;
using Beca.Xamarin.Example.Android.Core;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;


namespace Beca.Xamarin.Example.Android.UI
{
	public class Setup : MvxAndroidSetup
	{
		public Setup(Context applicationContext)
			: base(applicationContext)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			return new AndroidApp();
		}
	}
}