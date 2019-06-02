using System;
using System.Text;

using System.Net;      //required
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace Ajax_Minimal.Models
{
	public class Data
	{
		private TcpListener server;
		private TcpClient client;
		private BinaryReader reader;
		public bool IsRunning { get; set; } = false;
		public bool IsConnected { get; set; } = false;
		//the singelton implemented from AppSettingsModel
		#region Singleton
		private static Data m_Instance = null;
		public static Data Instance
		{
			get
			{
				if (m_Instance == null)
				{
					m_Instance = new Data();
				}
				return m_Instance;
			}
		}
		#endregion
		public void Connect(string ip, int port)
		{
			IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
			server = new TcpListener(ep);
			//check if running
			if (!IsRunning)
			{
				server.Start();
			}
			IsRunning = true;
		}
		public string[] ReadData()
		{
			//check if can connect
			while (!IsConnected)
			{
				this.client = server.AcceptTcpClient();
				reader = new BinaryReader(client.GetStream());
				IsConnected = true;
				//Debug.WriteLine("Connected");
			}
			//loop to read till first \n
			string dataRead = "";
			char currentChar;
			while ((currentChar = reader.ReadChar()) != '\n')
			{
				dataRead += currentChar;
			}
			//only need Lon and Lat
			string[] temp = dataRead.Split(',');
			string[] res = { temp[0], temp[1] };
			return res;
		}
		public void Disconnect()
		{
			//If was exited
			if (client != null && client.Connected)
			{
				client.Close();
			}

			//If running
			if (IsRunning)
			{
				server.Stop();
			}
			IsRunning = false;
			IsConnected = false;
			m_Instance = null;
		}
	}
}

