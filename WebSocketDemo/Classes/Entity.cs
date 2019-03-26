using System.Diagnostics;
using Newtonsoft.Json;

namespace WebSocketServer.Classes
{
	// Entity is a connected socket
	public class Entity
	{
		public string Source { get; set; }

		public string Host { get; set; }

		public string Instance { get; set; }

		public string SocketId { get; set; }

		public void PrintToConsole()
		{
			Debug.WriteLine("");
			Debug.WriteLine("This is the Entity:");
			Debug.WriteLine($"Source: {this.Source}");
			Debug.WriteLine($"Host: {this.Host}");
			Debug.WriteLine($"Instance: {this.Instance}");
			Debug.WriteLine($"SocketId: {this.SocketId}");
			Debug.WriteLine("");
		}

		public string PrintToJsonString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}