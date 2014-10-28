using Beca.Xamarin.Example.Core.Models;
using Beca.Xamarin.Example.Core.Services;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beca.Xamarin.Example.Core.ViewModels
{
	public class TodoItemsViewModel : MvxViewModel
	{
		readonly IAzureService _azureService;
		List<ToDoItem> _toDoItems;
		string _item, _details;


		public TodoItemsViewModel(IAzureService azureService)
		{
			_azureService = azureService;
			_toDoItems = new List<ToDoItem>();
		}

		public string Item
		{
			get { return _item; }
			set
			{
				_item = value;
				RaisePropertyChanged(() => Item);
			}
		}

		public string Details
		{
			get { return _details; }
			set
			{
				_details = value;
				RaisePropertyChanged(() => Details);
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
			ToDoItems = _azureService.GetItems().ToList();
			base.Start();
		}

		public IMvxCommand AddItem
		{
			get
			{
				return new MvxCommand(() =>
				{
					_azureService.AddTodoItem(Item, Details);
					ToDoItems = _azureService.GetItems().ToList();
				});
			}
		}
	}
}
