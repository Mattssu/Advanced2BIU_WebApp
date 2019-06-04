using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace Ajax_Minimal.Models
{
	public class InfoModel
	{
		private static InfoModel s_instace = null;

		public static InfoModel Instance
		{
			get
			{
				if (s_instace == null)
				{
					s_instace = new InfoModel();
				}
				return s_instace;
			}
		}
		// members
		public Location Location { get; private set; }
		public string ip { get; set; }
		public string port { get; set; }
		public string fileName { get; set; }
		public int time { get; set; }
		public int speed { get; set; }
		public int duration { get; set; }
		public bool isEmpty { get; set; }
		public Queue<Location> locationQueue;
		public const string SCENARIO_FILE = "~/SaveFiles/{0}.txt"; // The Path of the Secnario

		public Location LoadLocation()
		{
			Location result;
			bool flag = IPAddress.TryParse(ip, out IPAddress IP);
			if (flag)
			{
				result = LocationServer();
			}
			else
			{
				//because it is a file name and not an ip
				fileName = ip;
				//time = int.Parse(port);
				result = LocationFile();
			}
			return result;
		}

		public Location LocationServer()
		{
			TcpClient client = new TcpClient(ip, int.Parse(port));
			client.ReceiveTimeout = 5000;
			StreamReader reader = new StreamReader(client.GetStream(), Encoding.ASCII);
			StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.ASCII);
			// writing proccess
			string lonStr = "get /position/longitude-deg\r\n";
			string latStr = "get /position/latitude-deg\r\n";
			string rudStr = "get /controls/flight/rudder\r\n";
			string thrStr = "get /controls/engines/current-engine/throttle\r\n";
			//reading process
			writer.Write(lonStr);
			writer.Flush();
			string lon = reader.ReadLine().Split(new string[] { "'" }, StringSplitOptions.None)[1];
			writer.Write(latStr);
			writer.Flush();
			string lat = reader.ReadLine().Split(new string[] { "'" }, StringSplitOptions.None)[1];
			writer.Write(thrStr);
			writer.Flush();
			string throttle = reader.ReadLine().Split(new string[] { "'" }, StringSplitOptions.None)[1];
			writer.Write(rudStr);
			writer.Flush();
			string rudder = reader.ReadLine().Split(new string[] { "'" }, StringSplitOptions.None)[1];
			//close
			Location result = new Location(double.Parse(lon), double.Parse(lat),
				double.Parse(throttle), double.Parse(rudder));
			client.Close();
			locationQueue.Enqueue(result);
			return result;
		}

		public Location LocationFile()
		{
			string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
			try
			{
				if (locationQueue.Count() == 0 && isEmpty == false)
				{
					isEmpty = true;
					string[] lines = File.ReadAllLines(path);        // reading all the lines of the file
					foreach (string line in lines)
					{
						//line split into queue ,read from file
						var ls = line.Split(' ');
						locationQueue.Enqueue(new Location(double.Parse(ls[0]),
							double.Parse(ls[1]), double.Parse(ls[2]), double.Parse(ls[3])));
					}
				}
				return locationQueue.Dequeue();
			}
			catch (Exception e)
			{
				//Debug.Print("Exited with ERROR");
				return null;
			}

		}
	}

}
