using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketServer.Globals;
using WebSocketServer.WebSockets;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace WebSocketServer.Globals
{
	public class PostHandler : WebSocketHandler
	{
		public PostHandler(WebSocketConnectionManager webSocketConnectionManager) : base(
			webSocketConnectionManager)
		{

		}
		public async void PostToInstance(/*string instanceType, string message*/)
		{
			await SendMessageToAllAsync("eeeeeeeendliche");
			//Debug.WriteLine("eeeeeeeeeeendliche!!!");
		}

		public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
		{
			throw new NotImplementedException();
		}
	}
}
