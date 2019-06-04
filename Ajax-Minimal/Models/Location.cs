using System.Xml;

namespace Ajax_Minimal.Models
{
	public class Location
	{
		public double Lon { get; set; }
		public double Lat { get; set; }

		public Location(double lon, double lat)
		{
			Lon = lon;
			Lat = lat;
		}
		public void ToXml(XmlWriter writer)
		{
			writer.WriteStartElement("Location");
			writer.WriteElementString("Lon", Lon.ToString());
			writer.WriteElementString("Lat", Lat.ToString());
			writer.WriteEndElement();
		}
	}
}