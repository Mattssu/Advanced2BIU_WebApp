using System.Xml;

namespace Ajax_Minimal.Models
{
	public class Location
	{
		public double Lon { get; set; }
		public double Lat { get; set; }
		public double Throttle { get; set; }
		public double Rudder { get; set; }

		public Location(double lon, double lat, double throttle, double rudder)
		{
			Lon = lon;
			Lat = lat;
			Throttle = throttle;
			Rudder = rudder;
		}
		public void ToXml(XmlWriter writer)
		{
			writer.WriteStartElement("Location");
			writer.WriteElementString("Lon", Lon.ToString());
			writer.WriteElementString("Lat", Lat.ToString());
			writer.WriteElementString("Throttle",Throttle.ToString());
			writer.WriteElementString("Rudder", Rudder.ToString());
			writer.WriteEndElement();
		}
	}
}