using Beca.Xamarin.Example.Android.Core.Services;
using Beca.Xamarin.Example.Core.Services;
using Cirrious.CrossCore;

namespace Beca.Xamarin.Example.Android.Core
{
	public class AndroidApp : Beca.Xamarin.Example.Core.App
	{
		public AndroidApp()
			: base()
		{
			Mvx.RegisterType<IPathsService, AndroidPathsService>();
		}
	}
}