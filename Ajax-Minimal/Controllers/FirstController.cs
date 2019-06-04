using Ajax_Minimal.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Ajax_Minimal.Controllers
{
	public class FirstController : Controller
	{
		private static Random rnd = new Random();

		// GET: First
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult display(string ip, string port, int time = 0)
		{
			InfoModel.Instance.ip = ip;
			InfoModel.Instance.port = port;
			InfoModel.Instance.time = time;
			InfoModel.Instance.isEmpty = false;
			InfoModel.Instance.locationQueue = new Queue<Location>();
			//if is either 0 or display a recorded file
			if (time == 0 && port != null)
			{
				Session["time"] = int.Parse(port);
			}
			else
			{
				Session["time"] = time;
			}
			return View();
		}

		[HttpGet]
		public ActionResult save(string ip, string port, int speed, int duration, string file)
		{
			InfoModel.Instance.ip = ip;
			InfoModel.Instance.port = port.ToString();
			InfoModel.Instance.speed = speed;
			InfoModel.Instance.duration = duration;
			InfoModel.Instance.fileName = file;
			InfoModel.Instance.locationQueue = new Queue<Location>();
			Session["speed"] = speed;
			Session["duration"] = duration;
			//
			string path = "~/SaveFiles/" + file + ".txt";
			if (System.IO.File.Exists(Server.MapPath(path)))
			{
				System.IO.File.Delete(Server.MapPath(path));
			}
			return View();
		}


		[HttpPost]
		public string GetLocation()
		{
			try
			{
				var loc = InfoModel.Instance.LoadLocation();
				return ToXml(loc);
			}
			catch (Exception e)
			{
				//Debug.Print("Exited with ERROR");
				return null;
			}
		}

		[HttpPost]
		public string SaveLocation()
		{
			try
			{
				var loc = InfoModel.Instance.LoadLocation();
				SaveXml(InfoModel.Instance.fileName, loc);
				return ToXml(loc);
			}
			catch (Exception e)
			{
				Debug.Print("Exited with ERROR");
				return null;
			}
		}

		private void SaveXml(string fileName, Location l)
		{
			if (fileName != null)
			{
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(Server.MapPath("~/SaveFiles/" + fileName + ".txt"), true))
				{
					file.WriteLine(l.Lon + " " + l.Lat + " " + l.Throttle + " " + l.Rudder);
				}
			}
		}
		private string ToXml(Location loc)
		{
			//Initiate XML stuff
			StringBuilder sb = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings();
			XmlWriter writer = XmlWriter.Create(sb, settings);

			writer.WriteStartDocument();
			writer.WriteStartElement("FlightLocation");

			loc.ToXml(writer);

			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Flush();
			//
			return sb.ToString();
		}
	}
}