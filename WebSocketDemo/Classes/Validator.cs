using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebSocketServer.Classes
{
	// the Json Validator gives true if a json string is valid
	public class Validator
	{
		public static bool IsValidJson(string strInput)
		{
			strInput = strInput.Trim();
			if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
			    (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
			{
				try
				{
					var obj = JToken.Parse(strInput);
					return true;
				}
				catch (JsonReaderException jex)
				{
					//Exception in parsing json
					Console.WriteLine(jex.Message);
					return false;
				}
				catch (Exception ex) //some other exception
				{
					Console.WriteLine(ex.ToString());
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}
}
