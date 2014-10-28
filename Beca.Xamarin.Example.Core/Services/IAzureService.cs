using Beca.Xamarin.Example.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beca.Xamarin.Example.Core.Services
{
	public interface IAzureService
	{
		Task AddTodoItem(string text);

		Task<IEnumerable<ToDoItem>> GetItems();
	}
}