using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebSocketServer.Classes;

namespace WebSocketServer.WebSockets
{
	public class EviWebSocketHandler1 : WebSocketHandler
	{
		public EviWebSocketHandler1(WebSocketConnectionManager webSocketConnectionManager) : base(
			webSocketConnectionManager)
		{

		}

		// receives messages and registers the instance, host and source to the socketId
		public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
		{


			// convert incoming Bytes to string
			string message = Encoding.UTF8.GetString(buffer, 0, result.Count).Trim();

			//validate Json
			if (Validator.IsValidJson(message))
			{
				//serialize JSON String to C# Object
				var messageObject = new Message();
				JsonConvert.PopulateObject(message, messageObject);

				//debug message
				Debug.WriteLine("");
				Debug.WriteLine("Debug: This message just arrived from WebSocket api:");
				Debug.WriteLine(messageObject.PrintToJsonString());
				//messageObject.PrintToConsole();

				// if the message is a login
				if (messageObject.Event == ValueWord.LOGIN)
				{
					Debug.WriteLine("This is a Login Event");
					if (messageObject.tx_Source != null)
					{
						Registered.Entities.First(e => e.SocketId == WebSocketConnectionManager.GetId(socket)).Source
							= messageObject.tx_Source;
						Debug.WriteLine("Source Name Logged in successful");
					}

					if (messageObject.tx_Host != null)
					{
						Registered.Entities.First(e => e.SocketId == WebSocketConnectionManager.GetId(socket)).Host
							= messageObject.tx_Host;
						Debug.WriteLine("Host Name Logged in successful");
					}

					if (messageObject.tx_Instance != null)
					{
						Registered.Entities.First(e => e.SocketId == WebSocketConnectionManager.GetId(socket)).Instance
							= messageObject.tx_Instance;
						Debug.WriteLine("Instance Name Logged in successful");
					}

					await SendMessageAsync(WebSocketConnectionManager.GetId(socket),
						Registered.Entities.Find(e => e.SocketId == WebSocketConnectionManager.GetId(socket))
							.PrintToJsonString());
				}

				// if the message is a change
				if (messageObject.Event == ValueWord.CHANGE)
				{
					Debug.WriteLine("This is a Change Event");
					if (messageObject.rx_Host != null)
					{
						await SendMessageToHost(messageObject.rx_Host, message);
						Debug.WriteLine($"Sent Message to Host: {messageObject.rx_Host}");
					}

					if (messageObject.rx_Instance != null)
					{
						await SendMessageToInstance(messageObject.rx_Instance, message);
						Debug.WriteLine($"Sent Message to Instance: {messageObject.rx_Instance}");
					}

					if (messageObject.rx_Source != null)
					{
						await SendMessageToSource(messageObject.rx_Source, message);
						Debug.WriteLine($"Sent Message to Source: {messageObject.rx_Source}");
					}
				}

				if (messageObject.Event == ValueWord.READ)
				{
					Debug.WriteLine("This is a Read Event");
					if (messageObject.rx_Host != null)
					{
						await SendMessageToHost(messageObject.rx_Host, message);
						Debug.WriteLine($"Sent Message to Host: {messageObject.rx_Host}");
					}

					if (messageObject.rx_Instance != null)
					{
						await SendMessageToInstance(messageObject.rx_Instance, message);
						Debug.WriteLine($"Sent Message to Instance: {messageObject.rx_Instance}");
					}

					if (messageObject.rx_Source != null)
					{
						await SendMessageToSource(messageObject.rx_Source, message);
						Debug.WriteLine($"Sent Message to Source: {messageObject.rx_Source}");
					}
				}

				if (messageObject.Event == ValueWord.UPDATE)
				{
					Debug.WriteLine("This is an Update Event");
					await SendMessageAsync(WebSocketConnectionManager.GetId(socket), GetAllEntities.GetTheList());
				}
			}
			else
			{
				SendMessageAsync(WebSocketConnectionManager.GetId(socket), "Invalid JSON");
			}
		}

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
				if (message != null)
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