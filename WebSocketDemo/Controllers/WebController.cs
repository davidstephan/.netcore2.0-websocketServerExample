using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSocketServer.Classes;
using WebSocketServer.WebSockets;
using Newtonsoft.Json;

namespace WebSocketServer.Controllers
{
	[Produces("application/json")]
	[Route("web/api")]
	public class WebController : Controller
	{
		// constructor
		private EviWebSocketHandler1 clientWebSocketHandler { get; set; }

		public WebController(EviWebSocketHandler1 handler)
		{
			clientWebSocketHandler = handler;
		}

		// GET web/api
		[HttpGet]
		public ActionResult Get()
		{
			//returns JSON of all instances logged in
			return Content(GetAllEntities.GetTheList(), "application/json");
		}

		// POST web/api
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Message messageObject)
		{
			//Debug Code prints the POST message to Console
			Debug.WriteLine("");
			Debug.WriteLine("Debug: This message just arrived as HTTP POST from /web/api:");
			Debug.WriteLine(messageObject.PrintToJsonString());
			
			// the object generated from the POST message gets serialized into a string
			var messageString = JsonConvert.SerializeObject(messageObject);

			// send the message to addressed instance
			if (messageObject.rx_Instance != null)
			{
				await clientWebSocketHandler.SendMessageToInstance(messageObject.rx_Instance, messageString);
				Debug.WriteLine($"Sent Message to Instance: {messageObject.rx_Instance}");
			}

			// send the message to addressed source
			if (messageObject.rx_Source != null)
			{
				await clientWebSocketHandler.SendMessageToSource(messageObject.rx_Source, messageString);
				Debug.WriteLine($"Sent Message to Source: {messageObject.rx_Source}");
			}

			// send the message to addressed host
			if (messageObject.rx_Host != null)
			{
				await clientWebSocketHandler.SendMessageToHost(messageObject.rx_Host, messageString);
				Debug.WriteLine($"Sent Message to Host: {messageObject.rx_Host}");
			}
			
			//returns the POST message to sender
			return Json(messageObject);

		}
	}
}
