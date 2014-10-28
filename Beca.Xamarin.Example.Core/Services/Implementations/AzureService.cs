using Beca.Xamarin.Example.Core.Models;
using System;
using System.Collections.Generic;

namespace Beca.Xamarin.Example.Core.Services.Implementations
{
	public class AzureService : IAzureService
	{
		List<ToDoItem> _items = new List<ToDoItem>() { new ToDoItem { Item = "Xamarin", Details = "Develop" } };

		public void AddTodoItem(string item, string details)
		{
			_items.Add(new ToDoItem { Item = item, Details = details });
		}

		public IEnumerable<ToDoItem> GetItems()
		{
			return _items;
		}
	}
}