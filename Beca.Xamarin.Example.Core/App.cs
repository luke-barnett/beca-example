using Beca.Xamarin.Example.Core.Services;
using Beca.Xamarin.Example.Core.Services.Implementations;
using Beca.Xamarin.Example.Core.ViewModels;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace Beca.Xamarin.Example.Core
{
	public class App : MvxApplication
	{
		public App()
		{
			Mvx.RegisterType<IAzureService, AzureService>();
			Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<TodoItemsViewModel>());
		}
	}
}