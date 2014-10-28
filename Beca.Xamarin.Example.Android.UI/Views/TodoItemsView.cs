using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Beca.Xamarin.Example.Core.ViewModels;

namespace Beca.Xamarin.Example.Android.UI.Views
{
	[Activity(Label = "ToDo Items", MainLauncher = true)]
	public class TodoItemsView : MvxActivity
	{
		public new TodoItemsViewModel ViewModel
		{
			get { return (TodoItemsViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected override void OnViewModelSet()
		{
			base.OnViewModelSet();
			SetContentView(Resource.Layout.View_TodoItems);
		}
	}
}