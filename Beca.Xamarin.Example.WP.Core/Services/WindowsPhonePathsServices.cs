using Beca.Xamarin.Example.Core.Services;
using System.IO;

namespace Beca.Xamarin.Example.WP.Core.Services
{
	public class WindowsPhonePathsServices : IPathsService
	{
		public string SQLitePathString
		{
			get
			{
				return "application.db";
			}
		}
	}
}