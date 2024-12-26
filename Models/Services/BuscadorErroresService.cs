using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IntegracionOcasaDtv;
using IntegracionOcasaDtv.Models.Config;
using IntegracionOcasaDtv.Models.DAO;
using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using Renci.SshNet.Sftp;

public class BuscadorErroresService
{
	private readonly IConfiguration _configuration;
	private readonly DTVMoniErrorDAO _dtvMoni;
	private string[] folders;
	private bool existFile;
	private decimal idInteg;

	public BuscadorErroresService(IntegracionDtvContext context, IConfiguration configuration)
	{
		_dtvMoni = new DTVMoniErrorDAO(context);
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
	}

	public void SaveFiles()
	{
		string[] array = folders;
		foreach (string item in array)
		{
			++idInteg;
			FillDataBase(item, "AR");
		}
		idInteg = default(decimal);
		string[] array2 = folders;
		foreach (string item2 in array2)
		{
			++idInteg;
			FillDataBase(item2, "UY");
		}
	}
	public void FillDataBase(string pathError, string idPais)
	{
		SftpConfig config = new SftpConfig
		{
			Host = _configuration["SftpServerIp"],
			Port = int.Parse(_configuration["SftpServerPort"]),
			UserName = _configuration["SftpUser"],
			Password = _configuration["SftpPassword"]
		};
		using SftpClient client = new SftpClient(config.Host, config.Port, config.UserName, config.Password);
		try
		{
			client.Connect();
			IEnumerable<SftpFile> res = ((idPais == "AR") ? client.ListDirectory(_configuration[pathError]).Where(x=>x.LastWriteTime >= DateTime.Today.AddDays(-1)) : client.ListDirectory(_configuration[pathError + "_" + idPais]).Where(x => x.LastWriteTime >= DateTime.Today.AddDays(-1)));
			List<string> Files = new List<string>();
			List<DateTime> Dates = new List<DateTime>();
			string[] data = new string[2];
			Files = res.Select((SftpFile x) => x.FullName).ToList();
			Dates = res.Select((SftpFile x) => x.LastWriteTime).ToList();
			List<DtvMoniError> _dtvMoniError = new List<DtvMoniError>();
			for (int i = 0; i < Files.Count(); i++)
			{
				string dateFile = Dates[i].ToString("dd/MM/yyyy");
				if (res.ElementAt(i).Name != "." && res.ElementAt(i).Name != "..")
				{
					string fileName = Path.GetFileName(Files.ElementAt(i));
					existFile = _dtvMoni.FindFile(fileName);
					bool idDocValido = false;
					string idDocumento = "";
					try
					{
						idDocumento = fileName.Split("_")[2].ToString();
					}
					catch (Exception)
					{
					}
					decimal _idDoc = 0.0m;
					try
					{
						_idDoc = Convert.ToDecimal(idDocumento);
						idDocValido = true;
					}
					catch (Exception)
					{
						idDocValido = false;
					}
					if (!existFile && dateFile == DateTime.Today.ToString("dd/MM/yyyy") && idDocValido)
					{
						DtvMoniError dtvMoniError = new DtvMoniError();
						dtvMoniError.FechaSys = DateTime.Now;
						dtvMoniError.FechaVcia = DateTime.Now;
						dtvMoniError.Usuario = "IntegOcasaDtv";
						dtvMoniError.DescCorta = "MonitorDTV";
						dtvMoniError.DescLarga = "MonitorDTV";
						dtvMoniError.Estado = "0";
						dtvMoniError.IdDocumento = _idDoc;
						dtvMoniError.NombreArchivo = fileName;
						dtvMoniError.IdIntegracion = idInteg;
						dtvMoniError.IdPais = idPais;
						dtvMoniError.IdEstado = 1m;
						dtvMoniError.FechaError = dateFile;
						dtvMoniError.IdResponsable = 1m;
						dtvMoniError.FechaCierre = "";
						dtvMoniError.Error = "";
						dtvMoniError.Observacion = "";
						_dtvMoniError.Add(dtvMoniError);
					}
				}
			}
			_dtvMoni.Add(_dtvMoniError);
		}
		catch (Exception ex)
		{
			MailHelper.SendMail("BuscadorErrores " + $"{ex}");
		}
		finally
		{
			client.Disconnect();
		}
	}
}
