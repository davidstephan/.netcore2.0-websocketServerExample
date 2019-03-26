using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Template;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using WebSocketServer.Globals;
namespace WebSocketServer.WebSockets
{	/*---------------------------------------*/
	//
	//	EviWebSocketHandler derives from WebSocketHandler
	//
	/*---------------------------------------*/

	public class EviWebSocketHandler : WebSocketHandler
	{
		public EviWebSocketHandler(WebSocketConnectionManager webSocketConnectionManager) : base(
			webSocketConnectionManager)
		{

		}

		// receives messages and registers the instance, host and source to the socketId
		public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
		{
/*---------------------------------------*/
//
//	Here the message gets encoded an deserialized from JSON to a dictionary of key value pairs
//
/*---------------------------------------*/
			
			// convert incoming Bytes to string
			string message = Encoding.UTF8.GetString(buffer, 0, result.Count).Trim();


			//var messageDeserialized = new Dictionary<string, string>();
			//string testMessage = message.Trim();

			//TODO generate a token that validates the json. if message is not json, don't execute all the stuff below
			if (message.StartsWith("{") && message.EndsWith("}"))
			{
				// create a dictionary from deserialized JSON message
				var messageDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);

				//Print dictionary to console
				Debug.WriteLine("Debugging message folowing:");
				Debug.WriteLine("Printing the Dictionary generated from the JSON message from WebSocket Connection:");
				foreach (KeyValuePair<string, string> kvp in messageDictionary)
				{
					Debug.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
				}

				/*---------------------------------------*/
				//
				//	Here the database of registered entities gets updated based on the data of the senders message
				//
				/*---------------------------------------*/
				// the registered entity with the matching SocketId...
				if (messageDictionary.ContainsKey(KeyWord.TX_SOURCE))
				{
					Registered.Entities.First(a => a.SocketId == WebSocketConnectionManager.GetId(socket)).Source
						= messageDictionary.First(b => b.Key == KeyWord.TX_SOURCE).Value;
				}


				//// the registered entity with the matching SocketId...
				if (messageDictionary.ContainsKey(KeyWord.TX_HOST))
				{
					Registered.Entities.First(a => a.SocketId == WebSocketConnectionManager.GetId(socket)).Host
						= messageDictionary.First(b => b.Key == KeyWord.TX_HOST).Value;
				}


				//// the registered entity with the matching SocketId...
				if (messageDictionary.ContainsKey(KeyWord.TX_INSTANCE))
				{
					Registered.Entities.First(a => a.SocketId == WebSocketConnectionManager.GetId(socket)).Instance
						= messageDictionary.First(b => b.Key == KeyWord.TX_INSTANCE).Value;
				}

				///*---------------------------------------*/
				//
				//	just debug code
				//
				/*---------------------------------------*/
				if (Registered.Entities.Count > 0)
				{
					Debug.WriteLine("");
					Debug.WriteLine("Debugging message: Printing the registered Entities:");
					foreach (var element in Registered.Entities)
					{
						element.PrintToConsole();
					}
				}

				/*---------------------------------------*/
				//
				//	in case of the { "event" : "update" } send a broadcast to update all sockets
				//
				/*---------------------------------------*/

				//if the message contains the { "event" : "update" }
				foreach (KeyValuePair<string, string> kvp in messageDictionary)
				{
					if (kvp.Key == KeyWord.EVENT && kvp.Value == ValueWord.UPDATE)
					{
						await SendMessageAsync(WebSocketConnectionManager.GetId(socket), GetAllEntities.GetTheList());
					}
				}
				/*---------------------------------------*/
				//
				//	sends message to a specified instance
				//
				/*---------------------------------------*/
				if (messageDictionary.ContainsKey(KeyWord.RX_INSTANCE))
				{
					var instanceType = messageDictionary[KeyWord.RX_INSTANCE];
					if (instanceType != null)
					{
						await SendMessageToInstance(instanceType, message);
					}
				}

/*---------------------------------------*/
//
//	sends message to a specified host
//
/*---------------------------------------*/
				if (messageDictionary.ContainsKey(KeyWord.RX_HOST))
				{
					var hostType = messageDictionary[KeyWord.RX_HOST];
					if (hostType != null)
					{
						await SendMessageToHost(hostType, message);
					}
				}

/*---------------------------------------*/
//
//	sends message to a specified source
//
/*---------------------------------------*/
				if (messageDictionary.ContainsKey(KeyWord.RX_SOURCE))
				{
					var hostType = messageDictionary[KeyWord.RX_SOURCE];
					if (hostType != null)
					{
						await SendMessageToSource(hostType, message);
					}
				}
			}

			else
			{
				//await SendMessageToAllAsync($"INVALID JSON: {message}");
				await SendMessageAsync(WebSocketConnectionManager.GetId(socket), "Error: No valid JSON message!");
				Debug.WriteLine($"INVALID JSON: {message}");
			}


		}
/*---------------------------------------*/
//
//	methods to send messages
//
/*---------------------------------------*/

		//method to send message to a specific instance
		public async Task SendMessageToInstance(string instance, string message)
		{
			// check the registered entities for the given instance name
			var i = Registered.Entities.First(entity => entity.Instance == instance);
			if (message != null)
				if (i.SocketId != null)
					// send the message to the socket of the specified instance
					await SendMessageAsync(i.SocketId, message);
		}

		//method to send message to all matching hosts
		public async Task SendMessageToHost(string specifiedHostType, string message)
		{
			// make a list of all entities of the specified Host
			var specifiedEntities = Registered.Entities.FindAll(a => a.Host == specifiedHostType);
			// send the message to all SocketIds of this list
			foreach (var entity in specifiedEntities)
			{
				if(message != null)
					if (entity.SocketId != null)
						await SendMessageAsync(entity.SocketId, message);
			}
		}

		//method to send message to the specified source types
		public async Task SendMessageToSource(string specifiedSourceType, string message)
		{
			// make a list of all entities of the specified Source-type
			var specifiedEntities = Registered.Entities.FindAll(a => a.Source == specifiedSourceType);
			// send the message to all SocketIds of this list
			foreach (var entity in specifiedEntities)
			{
				if (message != null)
					if (entity.SocketId != null)
						await SendMessageAsync(entity.SocketId, message);
			}
		}

	}
}
