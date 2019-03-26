using System;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Query.Internal;
using WebSocketServer.Classes;

namespace WebSocketServer.WebSockets
{

	/*---------------------------------------*/
	//
	//	WebSocketHandler handles connect, disconnect,
	//	sent, received events
	//
	/*---------------------------------------*/

	public abstract class WebSocketHandler
	{
		// The Handler creates an Instance of WebSocketConnectionManager to handle Connections
		public WebSocketConnectionManager WebSocketConnectionManager { get; set; }

		public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager)
		{
			WebSocketConnectionManager = webSocketConnectionManager;
		}

		// Is called if a new socket connects
		public virtual async Task OnConnected(WebSocket socket)
		{
			// Add Socket to WebSocketConnectionManager
			WebSocketConnectionManager.AddSocket(socket);
			// Add entity to list of registered entities
			Entity newEntity = new Entity {SocketId = WebSocketConnectionManager.GetId(socket)};
			Registered.Entities.Add(newEntity);
			// debug connect info messages
			Debug.WriteLine("");
			Debug.WriteLine("New Socket Connected:");
			Debug.WriteLine($"{WebSocketConnectionManager.GetId(socket)}");
			Debug.WriteLine($"Number of registered Registered Entities: {Registered.Entities.Count}");
			Debug.WriteLine("");
			// Welcome Message for new Socket
			await SendMessageAsync(WebSocketConnectionManager.GetId(socket), "Welcome to the Control Manager!");
			await SendMessageAsync(WebSocketConnectionManager.GetId(socket), $"Your SocketId: {WebSocketConnectionManager.GetId(socket)}");
			await SendMessageAsync(WebSocketConnectionManager.GetId(socket), "You are now registered to the Control Manager.");
			await SendMessageAsync(WebSocketConnectionManager.GetId(socket), "With your first incoming message Control Manager will update your Instance, Host and Source Ids.");
			await SendMessageAsync(WebSocketConnectionManager.GetId(socket), "");
			await SendMessageAsync(WebSocketConnectionManager.GetId(socket), $"Number of registered Registered Entities: {Registered.Entities.Count}");
			await SendMessageAsync(WebSocketConnectionManager.GetId(socket), "Now you get the List of all registered entities");
			await SendMessageAsync(WebSocketConnectionManager.GetId(socket), GetAllEntities.GetTheList());
			await SendMessageAsync(WebSocketConnectionManager.GetId(socket), "");
		}

		// Is called if a socket send a close message 
		public virtual async Task OnDisconnected(WebSocket socket)
		{
			// Remove Entity from Registered Entities
			Registered.Entities.RemoveAll(a => a.SocketId == WebSocketConnectionManager.GetId(socket));

			// debug disconnect info messages
			Debug.WriteLine("");
			Debug.WriteLine("Socket Disconnected:");
			Debug.WriteLine($"{WebSocketConnectionManager.GetId(socket)}");
			Debug.WriteLine($"Number of registered Registered Entities: {Registered.Entities.Count}");
			Debug.WriteLine("");

			// socket disconnect info messages
			await SendMessageToAllAsync("Socket Disconnected:");
			await SendMessageToAllAsync($"{WebSocketConnectionManager.GetId(socket)}");
			await SendMessageToAllAsync($"Number of registered Registered Entities: {Registered.Entities.Count}");
			await SendMessageToAllAsync(GetAllEntities.GetTheList());

			// socket gets removed
			await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket));

		}

		public async Task SendMessageAsync(WebSocket socket, string message)
		{
			if (socket.State != WebSocketState.Open)
				return;

			await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
																  offset: 0,
																  count: message.Length),
								   messageType: WebSocketMessageType.Text,
								   endOfMessage: true,
								   cancellationToken: CancellationToken.None);
		}

		// Sends a Message String to a specific socketId
		public async Task SendMessageAsync(string socketId, string message)
		{
			try
			{
				await SendMessageAsync(WebSocketConnectionManager.GetSocketById(socketId), message);
			}
			catch (Exception)
			{
				//Debug.WriteLine($"DS_Message to Socket {socketId} could not be sent");
			}

		}

		// Sends a Message to all sockets
		public async Task SendMessageToAllAsync(string message)
		{
			foreach (var pair in WebSocketConnectionManager.GetAll())
			{
				if (pair.Value.State == WebSocketState.Open)
					await SendMessageAsync(pair.Value, message);
			}
		}

		// Receives Message
		public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
	}
}
