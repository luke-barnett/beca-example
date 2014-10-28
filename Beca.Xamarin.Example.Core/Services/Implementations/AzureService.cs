using Beca.Xamarin.Example.Core.Models;
using System;
using System.Collections.Generic;

namespace Beca.Xamarin.Example.Core.Services.Implementations
{
	public class AzureService : IAzureService
	{
		List<ToDoItem> _items = new List<ToDoItem>() { new ToDoItem { Text = "Xamarin", Complete = true } };

		public void AddTodoItem(string text)
		{
			_items.Add(new ToDoItem { Text = text, Complete = false });
		}

		public IEnumerable<ToDoItem> GetItems()
		{
			return _items;
		}
	}
}