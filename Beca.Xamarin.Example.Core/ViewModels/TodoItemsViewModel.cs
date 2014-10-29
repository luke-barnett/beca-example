using Beca.Xamarin.Example.Core.Models;
using Beca.Xamarin.Example.Core.Services;
using Cirrious.MvvmCross.Plugins.Location;
using Cirrious.MvvmCross.Plugins.PictureChooser;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Beca.Xamarin.Example.Core.ViewModels
{
	public class TodoItemsViewModel : MvxViewModel
	{
		readonly IAzureService _azureService;
		readonly IMvxPictureChooserTask _pictureChooserTask;
		readonly IMvxLocationWatcher _locationWatcher;
		List<ToDoItem> _toDoItems;
		string _text, _location;
		byte[] _image;

		public TodoItemsViewModel(IAzureService azureService, IMvxPictureChooserTask pictureChooserTask, IMvxLocationWatcher locationWatcher)
		{
			_azureService = azureService;
			_pictureChooserTask = pictureChooserTask;
			_locationWatcher = locationWatcher;
			_toDoItems = new List<ToDoItem>();
		}

		public string Location
		{
			get { return _location; }
			set
			{
				_location = value;
				RaisePropertyChanged(() => Location);
			}
		}

		public string Text
		{
			get { return _text; }
			set
			{
				_text = value;
				RaisePropertyChanged(() => Text);
			}
		}

		public List<ToDoItem> ToDoItems
		{
			get
			{
				return _toDoItems;
			}

			set
			{
				_toDoItems = value;
				RaisePropertyChanged(() => ToDoItems);
			}
		}

		public override void Start()
		{
			_locationWatcher.Start(new MvxLocationOptions { TimeBetweenUpdates = TimeSpan.FromSeconds(10) }, HandleLocation, (error) =>
			{
				var x = 1;
			});

			Task.Run(async () =>
				{
					await _azureService.Initialize();
					ToDoItems = (await _azureService.GetItems()).ToList();
				});

			base.Start();
		}

		public IMvxCommand AddItem
		{
			get
			{
				return new MvxCommand(async () =>
				{
					await _azureService.AddTodoItem(Text);
					ToDoItems = (await _azureService.GetItems()).ToList();
				});
			}
		}

		public byte[] Image
		{
			get
			{
				return _image;
			}
			set
			{
				_image = value;
				RaisePropertyChanged(() => Image);
			}
		}

		public IMvxCommand TakePicture
		{
			get
			{
				return new MvxCommand(() =>
				{
					_pictureChooserTask.TakePicture(400, 95,
					(stream) =>
					{
						using (var memoryStream = new MemoryStream())
						{
							stream.CopyTo(memoryStream);
							Image = memoryStream.ToArray();
						}
					},
					() => { });
				});
			}
		}

		void HandleLocation(MvxGeoLocation location)
		{
			Location = string.Format("{0} {1} at {2}", location.Coordinates.Latitude, location.Coordinates.Longitude, location.Timestamp);
		}
	}
}