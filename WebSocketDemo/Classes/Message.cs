using System.Diagnostics;
using Newtonsoft.Json;

namespace WebSocketServer.Classes
{
	// every parameter that can bet sent via HTTP POST
	public class Message
	{
		public string Event { get; set; }

		public string tx_Source { get; set; }

		public string rx_Source { get; set; }

		public string tx_Host { get; set; }

		public string rx_Host { get; set; }

		public string tx_Instance { get; set; }

		public string rx_Instance { get; set; }

		public string SocketId { get; set; }

		public double TargetBrightness { get; set; }

		public double AutoBrightness { get; set; }

		public double Contrast { get; set; }

		public double Homogenization { get; set; }

		public double BlackLimiter { get; set; }

		public double Saturation { get; set; }

		public bool SceneDetection { get; set; }

		public void PrintToConsole()
		{
			Debug.WriteLine("");
			Debug.WriteLine("This is the Message:");
			Debug.WriteLine($"Event: {this.Event}");
			Debug.WriteLine($"TX_Source: {this.tx_Source}");
			Debug.WriteLine($"RX_Source: {this.rx_Source}");
			Debug.WriteLine($"TX_Host: {this.tx_Host}");
			Debug.WriteLine($"RX_Host: {this.rx_Host}");
			Debug.WriteLine($"TX_Instance: {this.tx_Instance}");
			Debug.WriteLine($"RX_Instance: {this.rx_Instance}");
			Debug.WriteLine($"SocketId: {this.SocketId}");
			Debug.WriteLine($"TargetBrightness: {this.TargetBrightness}");
			Debug.WriteLine($"AutoBrightness: {this.AutoBrightness}");
			Debug.WriteLine($"Contrast: {this.Contrast}");
			Debug.WriteLine($"Homogenization: {this.Homogenization}");
			Debug.WriteLine($"BlackLimiter: {this.BlackLimiter}");
			Debug.WriteLine($"Saturation: {this.Saturation}");
			Debug.WriteLine($"SceneDetection: {this.SceneDetection}");
			Debug.WriteLine("");
		}

		public string PrintToJsonString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
