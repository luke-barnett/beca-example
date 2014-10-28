using Beca.Xamarin.Example.Core.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beca.Xamarin.Example.Core.Services.Implementations
{
	public class AzureService : IAzureService
	{
		public async Task AddTodoItem(string text)
		{
			using (var client = new MobileServiceClient("https://beca-example.azure-mobile.net/", "hMiyMQKrwAUSEYmiZDBDltutlLZyFh60"))
			{
				await client.GetTable<ToDoItem>().InsertAsync(new ToDoItem { Text = text, Complete = false });
			}
			
		}

		public async Task<IEnumerable<Models.ToDoItem>> GetItems()
		{
			try
			{
				using (var client = new MobileServiceClient("https://beca-example.azure-mobile.net/", "hMiyMQKrwAUSEYmiZDBDltutlLZyFh60"))
				{
					return await client.GetTable<ToDoItem>().OrderBy(item => item.Complete).ThenBy(item => item.Text).ToListAsync();
				}
			}
			catch(Exception e)
			{
				return new List<ToDoItem>();
			}
			
			
		}
	}
}
