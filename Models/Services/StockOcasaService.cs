using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using IntegracionOcasaDtv;
using IntegracionOcasaDtv.Models.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Auth;
using Renci.SshNet;


public class StockOcasaService
{
	private readonly IConfiguration _configuration;

	public StockOcasaService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

    [StructLayout(LayoutKind.Sequential)]
    private struct NETRESOURCE
    {
        public int dwScope;
        public int dwType;
        public int dwDisplayType;
        public int dwUsage;
        public string lpLocalName;
        public string lpRemoteName;
        public string lpComment;
        public string lpProvider;
    }

    [DllImport("mpr.dll")]
    private static extern int WNetAddConnection2(ref NETRESOURCE netResource, string password, string username, int flags);

    [DllImport("mpr.dll")]
    private static extern int WNetCancelConnection2(string name, int flags, bool force);

	public void ProcessFiles()
	{
		ProcessFiles(_configuration["Int053_data"], _configuration["Int053_archive"], _configuration["Int053_stage"]);
		ProcessFiles(_configuration["Int053_data_uy"], _configuration["Int053_archive_uy"], _configuration["Int053_stage_uy"]);
	}
    public void ProcessFiles(string data, string archive, string stage)
    {
        string username = _configuration["User"];
        string password = _configuration["Pass"];

        NETRESOURCE netResource = new NETRESOURCE
        {
            dwType = 1, // RESOURCETYPE_DISK
            lpRemoteName = data
        };

        int _result = WNetAddConnection2(ref netResource, password, username, 0);

        try
        {
            if (_result != 0)
            {
                string[] files = Directory.GetFiles(data);
                foreach (string file in files)
                {
                    string _file = file.Remove(0, data.Length);
                    //using (var fileStream = new FileStream(file, FileMode.Open))
                    //{
                    //    client.UploadFile(fileStream, stage + "/" + _file);
                    //}
                    try
                    {
                        File.Move(file, archive + "/" + _file);
                    }
                    catch (Exception e)
                    {
                        File.Delete(archive + "/" + _file);
                        File.Move(file, archive + "/" + _file);
                        throw new Exception(e.Message + " - " + e.StackTrace);
                    }
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message + " - " + e.StackTrace);
            //MailHelper.SendMail($"{e}" + " StockOcasa");
        }

        SftpConfig config = new SftpConfig
        {
            Host = _configuration["SftpServerIp"],
            Port = int.Parse(_configuration["SftpServerPort"]),
            UserName = _configuration["SftpUser"],
            Password = _configuration["SftpPassword"]
        };

        using SftpClient client = new SftpClient(config.Host, config.Port, config.UserName, config.Password);
        client.Connect();

		//finally
		//{
		//	client.Disconnect();
		//}
	}
}
