using Beca.Xamarin.Example.Core.Services;
using System.IO;

namespace Beca.Xamarin.Example.Android.Core.Services
{
	public class AndroidPathsService : IPathsService
	{
		public string SQLitePathString
		{
			get
			{
				var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "application.db");

				if (!File.Exists(path))
				{
					File.Create(path).Dispose();
				}

				return path;
			}
		}
	}
}