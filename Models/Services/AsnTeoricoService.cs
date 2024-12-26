using IntegracionOcasaDtv.Models.Config;
using IntegracionOcasaDtv.Models.DAO;
using IntegracionOcasaDtv.Models.DBEntities;
using IntegracionOcasaDtv.Models.Entities;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Xml;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using IntegracionOcasaDtv.Controllers;

namespace IntegracionOcasaDtv.Models.Services
{
    public class AsnTeoricoService
    {
        private readonly ILogger<AsnTeoricoController> _logger;
        private readonly AsnTeoricoDAO _asnTeoricoDao;
        private readonly IConfiguration _configuration;
        
        public AsnTeoricoService(IntegracionDtvContext context, IConfiguration configuration, ILogger<AsnTeoricoController> logger)
        {
            _asnTeoricoDao = new AsnTeoricoDAO(context);
            _configuration = configuration;
            _logger = logger;
        }

        public void SaveData(AsnTheoretical asnTheoretical)
        {
            DtvAsn toDbTeorico = new DtvAsn();
            toDbTeorico.Clave = asnTheoretical.Message.ID.ToString();
            toDbTeorico.FechaSys = DateTime.Now;
            toDbTeorico.Usuario = "IntegOcasaDtv";
            toDbTeorico.DescCorta = asnTheoretical.Integration.Code;
            toDbTeorico.DescLarga = asnTheoretical.Integration.Operation;
            toDbTeorico.Estado = "0";

            toDbTeorico.IdMensaje = asnTheoretical.Message.ID;
            toDbTeorico.FechaMensaje = asnTheoretical.Message.DateTime;
            toDbTeorico.IdIntegracion = asnTheoretical.Integration.Code;
            toDbTeorico.IntegProceso = asnTheoretical.Integration.Process;
            toDbTeorico.IntegOperacion = asnTheoretical.Integration.Operation;
            toDbTeorico.IdPais = asnTheoretical.Country;
            toDbTeorico.NroOc = asnTheoretical.PurchaseOrder;
            toDbTeorico.FechaOc = asnTheoretical.PurchaseOrderDateTime;
            toDbTeorico.IdTransaccion = asnTheoretical.TransactionId;
            toDbTeorico.Cantlineas = asnTheoretical.LinesQty.ToString();
            toDbTeorico.Archivo = asnTheoretical.FileName;


            foreach (var line in asnTheoretical.Lines)
            {
                DtvAsnProduct ToDbLine = new DtvAsnProduct();
                ToDbLine.Clave = asnTheoretical.Message.ID.ToString();
                ToDbLine.FechaSys = DateTime.Now;
                ToDbLine.Usuario = "IntegOcasaDtv";
                ToDbLine.DescCorta = asnTheoretical.Integration.Code;
                ToDbLine.DescLarga = asnTheoretical.Integration.Operation;
                ToDbLine.Estado = "0";

                ToDbLine.IdMensaje = toDbTeorico.IdMensaje;
                ToDbLine.IdPallet = line.PalletId;
                ToDbLine.IdProducto = line.Product;
                ToDbLine.IdProductPaired = line.PairedProduct;
                ToDbLine.CantProducto = line.Quantity;
                //ToDbLine.IdAsnProduct = long.Parse(line.Product);

                if (line.Serials != null)
                {
                    foreach (var product in line.Serials)
                    {
                        DtvAsnSeries ToDbLineProducto = new DtvAsnSeries();
                        ToDbLineProducto.Clave = asnTheoretical.Message.ID.ToString();
                        ToDbLineProducto.FechaSys = DateTime.Now;
                        ToDbLineProducto.Usuario = "IntegOcasaDtv";
                        ToDbLineProducto.DescCorta = asnTheoretical.Integration.Code;
                        ToDbLineProducto.DescLarga = asnTheoretical.Integration.Operation;
                        ToDbLineProducto.Estado = "0";

                        ToDbLineProducto.IdMensaje = toDbTeorico.IdMensaje;
                        ToDbLineProducto.IdPallet = line.PalletId;
                        ToDbLineProducto.IdProducto = line.Product;
                        ToDbLineProducto.NroSerie = product.Serial;
                        ToDbLineProducto.NroSeriePaired = product.PairedSerial;
                        ToDbLineProducto.NroMacAddress = product.McSerial;
                        ToDbLineProducto.Reetiquetar = product.Relabel;
                        ToDbLine.DtvAsnSeries.Add(ToDbLineProducto);
                    }
                }
                toDbTeorico.DtvAsnProducts.Add(ToDbLine);
            }

            _asnTeoricoDao.Add(toDbTeorico);
        }

        public void ProcessFiles()
        {
            ProcessFiles(_configuration["Int009_data"], _configuration["Int009_archive"], _configuration["Int009_error"], _configuration["Int009_stage"]);
            ProcessFiles(_configuration["Int009_data_uy"], _configuration["Int009_archive_uy"], _configuration["Int009_error_uy"], _configuration["Int009_stage_uy"]);
        }
        public void ProcessFiles(string data, string archive, string error, string stage)
        {
            _logger.Log(LogLevel.Warning, "Pre-conexión");
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
                _logger.Log(LogLevel.Warning, "Post-conexión");

                var res = client.ListDirectory(data);
                List<string> Files = new List<string>();

                Files = res.Select(x => x.FullName).ToList();

                for (int i = 0; i < Files.Count(); i++)
                {
                    if (res.ElementAt(i).Name != "." && res.ElementAt(i).Name != "..")
                    {
                        client.RenameFile(Files.ElementAt(i), stage + Path.GetFileName(Files.ElementAt(i)));
                        //si ya existe un archivo con el mismo nombre genera una excepción 

                        MemoryStream memoryStream = new MemoryStream();
                        client.DownloadFile(stage + Path.GetFileName(Files.ElementAt(i)), memoryStream);
                        memoryStream.Position = 0;

                        StreamReader Reader = new StreamReader(memoryStream);
                        string text = Reader.ReadToEnd();

                        AsnTheoretical asnTeorico = JsonConvert.DeserializeObject<AsnTheoretical>(text);
                        asnTeorico.FileName = res.ElementAt(i).Name;

                        JObject order = JObject.FromObject(asnTeorico);

                        IntegracionDtvContext context = new IntegracionDtvContext();
                        Utils.Utils.ProcessLog(order, true, false, true, context);

                        string[] values = new string[] { 
                            TypeDocumentConfig.int009, 
                            TypeDocumentConfig.INT009, 
                            TypeDocumentConfig.PO, 
                            TypeDocumentConfig.TMOL 
                        };
                        bool result = Utils.Utils.Validation(order, client, values);

                        memoryStream.Dispose();

                        if (result)
                        {
                            SaveData(asnTeorico);
                            client.RenameFile(stage + Path.GetFileName(Files.ElementAt(i)), archive + Path.GetFileName(Files.ElementAt(i)));
                            //Utils.Utils.ProcessLog(order,false, true, true, context);
                        }
                        else
                        {
                            client.RenameFile(stage + Path.GetFileName(Files.ElementAt(i)), error + Path.GetFileName(Files.ElementAt(i)));
                            Utils.Utils.ProcessLog(order, false, false, true, context);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                client.Disconnect();
            }
        }
    }
}
            