using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Query.SemanticAst;
using WebSocketServer.Models;
using WebSocketServer.WebSockets;

//using WebSocketDemo.Models;

namespace WebSocketDemo.WebSockets
{   /*---------------------------------------*/
	//
	//	ClientWebSocketHandler derives from WebSocketHandler
	//
	/*---------------------------------------*/
	public class ClientWebSocketHandler : WebSocketHandler
	{
		public ClientWebSocketHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
		{
			
		}

		public override async Task OnConnected(WebSocket socket)
		{
			await base.OnConnected(socket);
			var socketId = WebSocketConnectionManager.GetId(socket);
			RegisteredSockets.Client.Add(socketId);
			RegisteredSockets.Client.ForEach(i => Debug.WriteLine("{0}\t", i));
			await SendMessageToAllAsync($"CLIENT {socketId} is now connected");
		}

		
		public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
		{
			JsonMessage jsonMessage = new JsonMessage();
			bool invalidJson = false;
			var socketid = WebSocketConnectionManager.GetId(socket);
			Debug.WriteLine(WebSocketConnectionManager.GetId(socket));
			string message = Encoding.UTF8.GetString(buffer, 0, result.Count).Trim();

			//here we deserialize the message to check the UID from where its been sent
			string deserialized = message;
			if (deserialized.StartsWith("{") && deserialized.EndsWith("}"))
			{
				deserialized = deserialized.Replace("{", "").Replace("}","").Trim();
				string[] keyValuePairs = deserialized.Split(",");
				foreach (string keyValuePairRaw in keyValuePairs)
				{
					string[] keyValuePair = keyValuePairRaw.Split(":");
					if (keyValuePair.Count() == 2)
					{
						string key = keyValuePair[0].Trim().Replace("\"", "");
						var value = keyValuePair[1].Trim().Replace("\"", "");

						if (key == "contrast")
						{
							jsonMessage.Contrast = value;
						}

						if (key == "event")
						{
							jsonMessage.Event = value;
						}

						if (key == "uid")
						{
							jsonMessage.Uid = value;
						}
					}


				}

			}
			else
			{
				invalidJson = true;
				Debug.WriteLine("THIS WAS NOT A JSON MESSAGE");
			}
			//if the message has been sent from evi, its a status update for the clients
			if (jsonMessage.Event == "login")
			{
				await SendMessageToAllAsync(message);
			}

			//if the message has been sent from a client, its a parameter to change in evi
			if (jsonMessage.Event == "change")
			{
				//send message to all EviSockets
				foreach (string element in RegisteredSockets.Client)
				{
					await SendMessageAsync(element, message);
				}
				
			}
			//if the message has been sent from a client, its a parameter to change in evi
			if (jsonMessage.Event == "ack")
			{
				
			}

			// echo message to all
			await SendMessageToAllAsync("HALLO ECHO");

			Debug.WriteLine("CLIENTHANDLER AKTIV");
			Debug.WriteLine("CLIENTHANDLER AKTIV");
			Debug.WriteLine("CLIENTHANDLER AKTIV");
			Debug.WriteLine("CLIENTHANDLER AKTIV");
			Debug.WriteLine("CLIENTHANDLER AKTIV");


			// debug messages
			//			Debug.WriteLine("original message = " + message);
			//			Debug.WriteLine("jsonMessage.Contrast = " + jsonMessage.Contrast);
			//			Debug.WriteLine("jsonMessage.Uid = " + jsonMessage.Uid);
						Debug.WriteLine("jsonMessage.Event = " + jsonMessage.Event);
			//			Debug.WriteLine(WebSocketConnectionManager.GetAll().ToString());
		}
	}
}
