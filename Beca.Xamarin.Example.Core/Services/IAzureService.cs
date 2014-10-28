﻿using Beca.Xamarin.Example.Core.Models;
using System.Collections.Generic;

namespace Beca.Xamarin.Example.Core.Services
{
	public interface IAzureService
	{
		void AddTodoItem(string item, string details);

		IEnumerable<ToDoItem> GetItems();
	}
}