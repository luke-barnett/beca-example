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
		string _text;


		public TodoItemsViewModel(IAzureService azureService)
		{
			_azureService = azureService;
			_toDoItems = new List<ToDoItem>();
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
	}
}
