using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

public class IpAddressHelper
{
	public static string HostName()
	{
		string result = "";
		try
		{
			result = Dns.GetHostName();
		}
		catch (Exception)
		{
		}
		return result;
	}

	public static string IpAddress()
	{
		string myIP = "";
		try
		{
			IPHostEntry host = Dns.GetHostEntry(HostName());
			myIP = host.AddressList.FirstOrDefault((IPAddress ip) => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
		}
		catch (Exception)
		{
		}
		return myIP;
	}

	public static string HostNameIpAddressToString()
	{
		return "- Server Name:" + HostName() + " Ip:(" + IpAddress() + ")";
	}
}
