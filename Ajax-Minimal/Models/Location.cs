using System.Xml;

namespace Ajax_Minimal.Models
{
	public class Location
	{
		public float Lon { get; set; }
		public float Lat { get; set; }

		public Location(int lon, int lat)
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