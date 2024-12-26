using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IntegracionOcasaDtv;
using IntegracionOcasaDtv.Models.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using Renci.SshNet.Sftp;

public class GuardarArchivoService
{
	private readonly IConfiguration _configuration;
	private readonly IWebHostEnvironment _hostingEnvironment;
	private string[] folders;
	private bool existFile;
	public GuardarArchivoService(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
	{
		_configuration = configuration;
		folders = new string[9]
		{
			TypeDocumentConfig.Int008_error,
			TypeDocumentConfig.Int009_error,
			TypeDocumentConfig.Int010a_error,
			TypeDocumentConfig.Int010b_error,
			TypeDocumentConfig.Int012_error,
			TypeDocumentConfig.Int015a_error,
			TypeDocumentConfig.Int015b_error,
			TypeDocumentConfig.Int015c_error,
			TypeDocumentConfig.Int038_error
		};
		_hostingEnvironment = hostingEnvironment;
	}

	public byte[] FindFile(string fileName)
	{
		string[] nombreCorto = fileName.Split('_');
		MemberInfo[] lst = typeof(TypeDocumentConfig).GetMembers();
		string nombreFiltro = (nombreCorto[0] + "_error").ToUpper();
		MemberInfo _nombreCorto = lst.Where((MemberInfo x) => x.Name.ToUpper().Contains(nombreFiltro)).FirstOrDefault();
		string dato = _nombreCorto.Name;
		return DownloadFile(dato, fileName);
	}

	private byte[] DownloadFile(string path, string fileName)
	{
		SftpConfig config = new SftpConfig
		{
			Host = _configuration["SftpServerIp"],
			Port = int.Parse(_configuration["SftpServerPort"]),
			UserName = _configuration["SftpUser"],
			Password = _configuration["SftpPassword"]
		};
		byte[] dato = null;
		using SftpClient client = new SftpClient(config.Host, config.Port, config.UserName, config.Password);
		try
		{
			client.Connect();
			IEnumerable<SftpFile> res = client.ListDirectory(_configuration[path]);
			SftpFile fileFound = res.Where((SftpFile x) => x.Name == fileName).FirstOrDefault();
			if (fileFound != null)
			{
				try
				{
					dato = client.ReadAllBytes(_configuration[path] + "//" + fileName);
					return dato;
				}
				catch (Exception ex2)
				{
					MailHelper.SendMail($"{ex2}" + " GuardarArchivoService");
				}
				return dato;
			}
			return DownloadFile(path + "_uy", fileName);
		}
		catch (Exception ex)
		{
			MailHelper.SendMail($"{ex}" + " GuardarArchivoService");
			return dato = null;
		}
		finally
		{
			client.Disconnect();
		}
	}
}
