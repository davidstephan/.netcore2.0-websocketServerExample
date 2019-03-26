using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebSocketServer.Globals
{
	public class Message
	{
		public string Event { get; set; }

		public string tx_Source { get; set; }

		public string rx_Source { get; set; }

		public string tx_Host { get; set; }

		public string rx_Host { get; set; }

		public string tx_Instance { get; set; }

		public string rx_Instance { get; set; }

		//public string SocketId { get; set; }

		//public double TargetBrightness { get; set; }

		//public double AutoBrightness { get; set; }

		//public double Contrast { get; set; }

		//public double Homogenization { get; set; }

		//public double BlackLimiter { get; set; }

		//public double Saturation { get; set; }

		//public bool SceneDetection { get; set; }

		public void PrintToConsole()
		{
			Debug.WriteLine("");
			Debug.WriteLine("This is the Message:");
			Debug.WriteLine($"Event: {this.Event}");/*
			Debug.WriteLine($"TX_Source: {this.TX_Source}");
			Debug.WriteLine($"RX_Source: {this.RX_Source}");
			Debug.WriteLine($"TX_Host: {this.TX_Host}");
			Debug.WriteLine($"RX_Host: {this.RX_Host}");
			Debug.WriteLine($"TX_Instance: {this.TX_Instance}");
			Debug.WriteLine($"RX_Instance: {this.RX_Instance}");
			Debug.WriteLine($"SocketId: {this.SocketId}");
			Debug.WriteLine($"TargetBrightness: {this.TargetBrightness}");
			Debug.WriteLine($"AutoBrightness: {this.AutoBrightness}");
			Debug.WriteLine($"Contrast: {this.Contrast}");
			Debug.WriteLine($"Homogenization: {this.Homogenization}");
			Debug.WriteLine($"BlackLimiter: {this.BlackLimiter}");
			Debug.WriteLine($"Saturation: {this.Saturation}");
			Debug.WriteLine($"SceneDetection: {this.SceneDetection}");*/
			Debug.WriteLine("");
		}

		public string PrintToJsonString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
