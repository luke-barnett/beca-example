using Newtonsoft.Json;
namespace Beca.Xamarin.Example.Core.Models
{
	public class ToDoItem
	{
		public string id { get; set; }

		[JsonProperty(PropertyName = "text")]
		public string Text { get; set; }

		[JsonProperty(PropertyName = "complete")]
		public bool Complete { get; set; }
	}
}