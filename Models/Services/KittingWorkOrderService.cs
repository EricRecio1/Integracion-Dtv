using IntegracionOcasaDtv.Models.Config;
using IntegracionOcasaDtv.Models.DAO;
using IntegracionOcasaDtv.Models.DBEntities;
using IntegracionOcasaDtv.Models.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace IntegracionOcasaDtv.Models.Services
{
    public class KittingWorkOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly KittingWorkOrderDAO _kwoDao;

        public KittingWorkOrderService(IntegracionDtvContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _kwoDao = new KittingWorkOrderDAO(context);
        }

        public void SaveData(KittingWorkOrders order)
        {
            //reemplazar por el nombre de la clase base
            DtvKitOrder toDb = new DtvKitOrder();
            toDb.Clave = order.Message.ID.ToString();
            toDb.FechaSys = DateTime.Now;
            toDb.Usuario = "IntegOcasaDtv";
            toDb.DescCorta = order.Integration.Code;
            toDb.DescLarga = order.Integration.Operation;
            toDb.Estado = "0";

            toDb.IdMensaje = order.Message.ID;
            toDb.FechaMensaje = order.Message.DateTime;
            toDb.IdIntegracion = order.Integration.Code;
            toDb.IntegProceso = order.Integration.Process;
            toDb.IntegOperacion = order.Integration.Operation;
            toDb.IdPais = order.Country;
            toDb.TipoDocumento = order.DocumentType;
            toDb.IdDocumento = order.DocumentId;
            toDb.FecDocumento = order.DocumentDateTime;
            toDb.OrganizacionDes = order.ReceiverParty.Organization;
            toDb.SubinventariDes = order.ReceiverParty.SubInventory;
            toDb.LocalizadorDes = order.ReceiverParty.Tracker;
            toDb.Observaciones = order.OrderNote;
            toDb.CantItems = order.OrderLine.ItemLinesQuantity;
            toDb.Archivo = order.FileName;

            foreach (var producto in order.OrderLine.ItemLine)
            {
                DtvKitProduct toDbLine = new DtvKitProduct();
                toDbLine.Clave = order.Message.ID.ToString();
                toDbLine.FechaSys = DateTime.Now;
                toDbLine.Usuario = "IntegOcasaDtv";
                toDbLine.DescCorta = order.Integration.Code;
                toDbLine.DescLarga = order.Integration.Operation;
                toDbLine.Estado = "0";

                toDbLine.IdMensaje = order.Message.ID;
                toDbLine.CantProducto = producto.Quantity;
                toDbLine.IdProducto = producto.Item.Product.ID;
                toDbLine.TipoProducto = producto.Item.Type;
                toDbLine.OrganizacionOri = order.OrderLine.OriginParty.Organization;
                toDbLine.SubinventariOri = order.OrderLine.OriginParty.SubInventory;
                toDbLine.LocalizadorOri = order.OrderLine.OriginParty.Tracker;

                foreach(var sust in producto.Item.Sustitute)
                {
                    if (sust.ID.Length != 0)
                    {
                        DtvKitSusti toDbSus = new DtvKitSusti();
                        toDbSus.Clave = order.Message.ID.ToString();
                        toDbSus.FechaSys = DateTime.Now;
                        toDbSus.Usuario = "IntegOcasaDtv";
                        toDbSus.DescCorta = order.Integration.Code;
                        toDbSus.DescLarga = order.Integration.Operation;
                        toDbSus.Estado = "0";

                        toDbSus.IdMensaje = order.Message.ID;
                        toDbSus.IdSustituto = sust.ID;

                        toDbLine.DtvKitSustis.Add(toDbSus);
                    }
                    
                }

                toDb.DtvKitProducts.Add(toDbLine);
            }

            _kwoDao.Add(toDb);
        }


        public void ProcessFiles()
        {
            ProcessFiles(_configuration["Int008_data"], _configuration["Int008_archive"], _configuration["Int008_error"], _configuration["Int008_stage"]);
            ProcessFiles(_configuration["Int008_data_uy"], _configuration["Int008_archive_uy"], _configuration["Int008_error_uy"], _configuration["Int008_stage_uy"]);
        }

        public void ProcessFiles(string _data, string _archive, string _error, string _stage)
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

                //var res = client.ListDirectory(_configuration["Int008_data"]);
                var res = client.ListDirectory(_data);
                List<string> Files = new List<string>();

                Files = res.Select(x => x.FullName).ToList();

                for (int i = 0; i < Files.Count(); i++)
                {
                    if (res.ElementAt(i).Name != "." && res.ElementAt(i).Name != "..")
                    {

                        //client.RenameFile(Files.ElementAt(i), _configuration["Int008_stage"] + Path.GetFileName(Files.ElementAt(i)));
                        client.RenameFile(Files.ElementAt(i), _stage + Path.GetFileName(Files.ElementAt(i)));
                        //si ya existe un archivo con el mismo nombre genera una excepción 

                        MemoryStream memoryStream = new MemoryStream();
                        //client.DownloadFile(_configuration["Int008_stage"] + Path.GetFileName(Files.ElementAt(i)), memoryStream);
                        client.DownloadFile(_stage + Path.GetFileName(Files.ElementAt(i)), memoryStream);
                        memoryStream.Position = 0;

                        StreamReader Reader = new StreamReader(memoryStream);
                        string text = Reader.ReadToEnd();

                        //Para eliminar namespace y prefix del xml

                        XElement xdoc = XElement.Parse(text);
                        xdoc.Descendants().Attributes().Where(a => a.IsNamespaceDeclaration).Remove();
                        xdoc = Utils.Utils.RemoveAllNamespaces(xdoc);
                        string XmlNormalizado = xdoc.ToString();

                        XmlSerializer serializer = new XmlSerializer(typeof(KittingWorkOrders));
                        using (StringReader Reader2 = new StringReader(XmlNormalizado))
                        {
                            KittingWorkOrders Order = (KittingWorkOrders)serializer.Deserialize(Reader2);
                            Order.FileName = res.ElementAt(i).Name;
                            JObject order = JObject.FromObject(Order);

                            IntegracionDtvContext context = new IntegracionDtvContext();//para logs
                            Utils.Utils.ProcessLog(order, true, false, true, context);//para logs

                            string[] values = new string[] { 
                                TypeDocumentConfig.int008, 
                                TypeDocumentConfig.INT008, 
                                TypeDocumentConfig.WO
                            };
                            bool result = Utils.Utils.Validation(order, client, values);

                            if (result)
                            {
                                SaveData(Order);
                                client.RenameFile(_stage + Path.GetFileName(Files.ElementAt(i)), _archive + Path.GetFileName(Files.ElementAt(i)));
                                Utils.Utils.ProcessLog(order, false, true, true, context);//para logs
                            }
                            else
                            {
                                //client.RenameFile(_configuration["Int008_stage"] + Path.GetFileName(Files.ElementAt(i)), _configuration["Int008_error"] + Path.GetFileName(Files.ElementAt(i)));
                                client.RenameFile(_stage + Path.GetFileName(Files.ElementAt(i)), _error + Path.GetFileName(Files.ElementAt(i)));
                                Utils.Utils.ProcessLog(order, false, false, true, context);//para logs
                            }
                        }

                        memoryStream.Dispose();
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

