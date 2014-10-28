using Beca.Xamarin.Example.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beca.Xamarin.Example.Core.Services.Implementations
{
	public class MockedAzureService : IAzureService
	{
		List<ToDoItem> _items = new List<ToDoItem>() { new ToDoItem { Text = "Xamarin", Complete = true } };

		public async Task AddTodoItem(string text)
		{
			_items.Add(new ToDoItem { Text = text, Complete = false });
		}

		public async Task<IEnumerable<ToDoItem>> GetItems()
		{
			return _items;
		}

		public async Task Initialize()
		{
			
		}
	}
}