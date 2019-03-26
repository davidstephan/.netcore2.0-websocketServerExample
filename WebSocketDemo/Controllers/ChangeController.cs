using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSocketServer.Classes;
using WebSocketServer.WebSockets;
using Newtonsoft.Json;

namespace WebSocketServer.Controllers
{
	// just some tests in here. controller not used at the moment
	[Produces("application/json")]
	[Route("webapi/Change")]
	public class ChangeController : Controller
	{
		private EviWebSocketHandler1 clientWebSocketHandler { get; set; }

		public ChangeController(EviWebSocketHandler1 handler)
		{
			clientWebSocketHandler = handler;
		}

		// GET: webapi/Change
		[HttpGet]
		public ActionResult Get()
		{

			return Content(GetAllEntities.GetTheList(), "application/json");
		}


		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Entity entity)
		{
			Debug.WriteLine("");
			Debug.WriteLine("Debug: This just arrived as Post from /webapi/change:");
			Debug.WriteLine(entity);
			entity.PrintToConsole();
			await clientWebSocketHandler.SendMessageToAllAsync("Hello World");

			var message = JsonConvert.SerializeObject(entity);

			if (entity.Instance != null)
			{
				await clientWebSocketHandler.SendMessageToInstance(entity.Instance, message);

			}

			if (entity.Source != null)
			{
				await clientWebSocketHandler.SendMessageToSource(entity.Source, message);
			}

			if (entity.Host != null)
			{
				await clientWebSocketHandler.SendMessageToHost(entity.Host, message);
			}

			return Json(entity);

		}
	}

}