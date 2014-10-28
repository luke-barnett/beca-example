using Beca.Xamarin.Example.Core.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beca.Xamarin.Example.Core.Services.Implementations
{
	public class AzureService : IAzureService, IDisposable
	{
		readonly IPathsService _pathsService;
		readonly MobileServiceSQLiteStore _store;
		MobileServiceClient _client;

		public AzureService(IPathsService pathsService)
		{
			_pathsService = pathsService;

			_store = new MobileServiceSQLiteStore(_pathsService.SQLitePathString);
			_store.DefineTable<ToDoItem>();
			
		}

		public Task Initialize()
		{
			_client = new MobileServiceClient("https://beca-example.azure-mobile.net/", "hMiyMQKrwAUSEYmiZDBDltutlLZyFh60");

			return _client.SyncContext.InitializeAsync(_store);
		}

		public async Task AddTodoItem(string text)
		{
			await _client.GetSyncTable<ToDoItem>().InsertAsync(new ToDoItem { Text = text, Complete = false });
			await _client.SyncContext.PushAsync();
		}

		public async Task<IEnumerable<Models.ToDoItem>> GetItems()
		{
			await _client.GetSyncTable<ToDoItem>().PullAsync();
			return await _client.GetSyncTable<ToDoItem>().OrderBy(item => item.Complete).ThenBy(item => item.Text).ToListAsync();
		}

		public void Dispose()
		{
			_client.Dispose();
			_store.Dispose();
		}
	}
}