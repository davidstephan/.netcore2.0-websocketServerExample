using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketServer.WebSockets
{

	/*---------------------------------------*/
	//
	//	WebSocketConnectionManager holds the Socket IDs in _sockets.
	//	Deals with getting, adding and removing sockets.
	//
	/*---------------------------------------*/

	public class WebSocketConnectionManager
	{
		// ConcurrentDictionary<TKey,TValue> Class
		// Represents a thread-safe collection of key/value pairs that can be accessed by multiple threads concurrently.
		private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
		public WebSocket GetSocketById(string id)
		{
			return _sockets.FirstOrDefault(p => p.Key == id).Value;
		}

		// Get all Sockets
		public ConcurrentDictionary<string, WebSocket> GetAll()
		{
			return _sockets;
		}

		// Get Id of a Socket
		public string GetId(WebSocket socket)
		{
			return _sockets.FirstOrDefault(p => p.Value == socket).Key;
		}

		// Add a Socket. This Socket gets a string sId (Globally Unique Identifier)
		public void AddSocket(WebSocket socket)
		{
			string sId = CreateConnectionId();
			while (!_sockets.TryAdd(sId, socket))
			{
				sId = CreateConnectionId();
			}
			


		}

		// remove a Socket
		public async Task RemoveSocket(string id)
		{
			try
			{
				WebSocket socket;
				
				_sockets.TryRemove(id, out socket);
				

				await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);


			}
			catch (Exception)
			{
				Console.WriteLine("DS: Exception thrown from: public async Task RemoveSocket(string id)");
			}

		}
		
		// Gets called from AddSocket
		private string CreateConnectionId()
		{
			return Guid.NewGuid().ToString();
		}
	}
}
