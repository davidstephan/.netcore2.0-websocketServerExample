using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebSocketServer.Globals
{
	
	public static class Registered
	{
		public static List<Entity> Entities = new List<Entity>();
	}

	public static class GetAllEntities
	{
		public static string GetTheList()
		{
			string output;
			if (Registered.Entities.Count > 0)
			{
				output = JsonConvert.SerializeObject(Registered.Entities);
			}
			else
			{
				output = "Error: no Entities logged into the list";
			}

			return output;
		}
	}
}