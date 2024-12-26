using IntegracionOcasaDtv.Models.Config;
using IntegracionOcasaDtv.Models.DAO;
using IntegracionOcasaDtv.Models.DBEntities;
using IntegracionOcasaDtv.Models.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace IntegracionOcasaDtv.Models.Services
{
    public class AsnReceptionService
    {
        private readonly IConfiguration _configuration;
        private readonly AsnReceptionDAO _asnReceptionDao;
        string filename = "";

        public AsnReceptionService(IntegracionDtvContext context, IConfiguration configuration)
        {
            _asnReceptionDao = new AsnReceptionDAO(context);
            _configuration = configuration;
        }

        public void GenerateReception()
        {
            try
            {
                DtvAsnRecep[] receptions = _asnReceptionDao.GetUnprocessed();

                foreach (DtvAsnRecep reception in receptions)
                {
                    AsnRecepcion r = new AsnRecepcion();

                    r.Message = new Entities.Message()
                    {
                        ID = reception.IdMensaje,
                        DateTime = reception.FechaMensaje
                    };

                    r.Integration = new Integration()
                    {
                        Code = reception.IdIntegracion,
                        Process = reception.IntegProceso,
                        Operation = reception.IntegOperacion
                    };

                    r.Country = reception.IdPais;
                    r.DocumentType = reception.TipoDocumento;
                    r.DocumentId = reception.IdDocumento;
                    //r.DocumentTransactionId = reception.IdTransaccion.Value;
                    r.DocumentTransactionId = Convert.ToInt32(reception.IdTransaccion);
                    //r.DocumentDateTime = DateTime.ParseExact(reception.FecTransaccion, "yyyy-MM-ddTHH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                    r.DocumentDateTime = Convert.ToDateTime(reception.FecTransaccion.Substring(0, 10) + " " + reception.FecTransaccion.Substring(10, (reception.FecTransaccion.Length - 10)));
                    r.PurchaseOrder = reception.NroOc;

                    r.ReceiverParty = new ReceiverParty()
                    {
                        Organization = reception.Organizacion,
                        SubInventory = reception.Subinventario,
                        Tracker = reception.Localizador
                    };

                    r.ProductActivityLinesQuantity = reception.CantItems.Value;

                    r.Products = reception.DtvAsnRecProds.Select(x => new Product()
                    {
                        Id = x.IdProducto,
                        Quantity = x.CantProducto.Value,
                        Serials = x.DtvAsnRecSeris.Select(y => new Serial()
                        {
                            SerialNumber = y.NroSerie,
                            PairedProduct = y.IdProductPaired,
                            PairedSerial = y.NroSeriePaired
                        }).ToArray()
                    }).ToArray();
                    r.FileName = filename;
                    XmlDocument doc = GenerateXml(r);

                    SaveDocument(doc, r);

                    //string filename = "INT015a_" + r.DocumentType + "_" + r.DocumentId + "_" + r.DocumentTransactionId + ".xml";
                    //client.UploadFile(stream, "int015a/data/" + filename);

                    JObject order = JObject.FromObject(doc);
                    IntegracionDtvContext context = new IntegracionDtvContext();
                    //Utils.Utils.ProcessLog(order,false, true, false, context);

                    reception.Archivo = filename;
                    reception.Processed = true;
                    _asnReceptionDao.Update(reception);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private XmlDocument GenerateXml(AsnRecepcion recepcion)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration docNode = doc.CreateXmlDeclaration("1.0", null, null);
                doc.AppendChild(docNode);

                XmlElement productActivity = doc.CreateElement("ProductActivity");

                productActivity.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                productActivity.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                productActivity.SetAttribute("xmlns", "dtv-3pl:schema:xsd:ProductActivity");

                //-----------------MESSAGE--------------------------------------------------------
                XmlElement message = doc.CreateElement("Message");

                XmlElement messageId = doc.CreateElement("ID");
                messageId.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");

                messageId.AppendChild(doc.CreateTextNode(recepcion.Message.ID.ToString()));

                XmlElement messageDate = doc.CreateElement("DateTime");
                messageDate.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
                messageDate.AppendChild(doc.CreateTextNode(recepcion.Message.DateTime));

                message.AppendChild(messageId);
                message.AppendChild(messageDate);

                productActivity.AppendChild(message);
                //-----------------MESSAGE--------------------------------------------------------

                //-----------------INTEGRATION----------------------------------------------------
                XmlElement integration = doc.CreateElement("Integration");

                XmlElement code = doc.CreateElement("Code");
                code.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
                code.AppendChild(doc.CreateTextNode(recepcion.Integration.Code));

                XmlElement process = doc.CreateElement("Process");
                process.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
                process.AppendChild(doc.CreateTextNode(recepcion.Integration.Process));

                XmlElement operation = doc.CreateElement("Operation");
                operation.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
                operation.AppendChild(doc.CreateTextNode(recepcion.Integration.Operation));

                integration.AppendChild(code);
                integration.AppendChild(process);
                integration.AppendChild(operation);

                productActivity.AppendChild(integration);
                //-----------------INTEGRATION----------------------------------------------------

                XmlElement country = doc.CreateElement("Country");
                country.AppendChild(doc.CreateTextNode(recepcion.Country));
                productActivity.AppendChild(country);

                XmlElement documentType = doc.CreateElement("DocumentType");
                documentType.AppendChild(doc.CreateTextNode(recepcion.DocumentType));
                productActivity.AppendChild(documentType);

                XmlElement documentId = doc.CreateElement("DocumentId");
                documentId.AppendChild(doc.CreateTextNode(recepcion.DocumentId));
                productActivity.AppendChild(documentId);

                XmlElement documentTsx = doc.CreateElement("DocumentTransactionId");
                documentTsx.AppendChild(doc.CreateTextNode(recepcion.DocumentTransactionId.ToString()));
                productActivity.AppendChild(documentTsx);

                XmlElement documentDate = doc.CreateElement("DocumentDateTime");
                documentDate.AppendChild(doc.CreateTextNode(recepcion.DocumentDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")));
                productActivity.AppendChild(documentDate);

                XmlElement purchaseOrder = doc.CreateElement("PurchaseOrder");
                purchaseOrder.AppendChild(doc.CreateTextNode(recepcion.PurchaseOrder));
                productActivity.AppendChild(purchaseOrder);

                //-----------------RECEIVER PARTY-------------------------------------------------
                XmlElement receiver = doc.CreateElement("ReceiverParty");

                XmlElement organization = doc.CreateElement("Organization");
                organization.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
                organization.AppendChild(doc.CreateTextNode(recepcion.ReceiverParty.Organization));

                XmlElement subinventory = doc.CreateElement("SubInventory");
                subinventory.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
                subinventory.AppendChild(doc.CreateTextNode(recepcion.ReceiverParty.SubInventory));

                XmlElement tracker = doc.CreateElement("Tracker");
                tracker.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
                if (!string.IsNullOrEmpty(recepcion.ReceiverParty.Tracker))
                    tracker.AppendChild(doc.CreateTextNode(recepcion.ReceiverParty.Tracker));

                receiver.AppendChild(organization);
                receiver.AppendChild(subinventory);
                receiver.AppendChild(tracker);

                productActivity.AppendChild(receiver);
                //-----------------RECEIVER PARTY-------------------------------------------------


                XmlElement lineQty = doc.CreateElement("ProductActivityLinesQuantity");
                lineQty.AppendChild(doc.CreateTextNode(recepcion.ProductActivityLinesQuantity.ToString()));
                productActivity.AppendChild(lineQty);


                //-----------------PRODUCTS-------------------------------------------------------
                foreach (Product product in recepcion.Products)
                {
                    XmlElement activityLine = doc.CreateElement("ProductActivityLine");

                    XmlElement actLineQty = doc.CreateElement("ItemLinesQuantity");
                    actLineQty.SetAttribute("xmlns", "dtv-3pl:schema:xsd:DocumentContentComponents");
                    actLineQty.AppendChild(doc.CreateTextNode(recepcion.ProductActivityLinesQuantity.ToString()));
                    activityLine.AppendChild(actLineQty);

                    XmlElement itemLine = doc.CreateElement("ItemLine");
                    itemLine.SetAttribute("xmlns", "dtv-3pl:schema:xsd:DocumentContentComponents");

                    XmlElement qty = doc.CreateElement("Quantity");
                    qty.AppendChild(doc.CreateTextNode(product.Quantity.ToString()));
                    itemLine.AppendChild(qty);

                    XmlElement item = doc.CreateElement("Item");

                    XmlElement prod = doc.CreateElement("Product");
                    prod.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");

                    XmlElement prodId = doc.CreateElement("ID");
                    prodId.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
                    prodId.AppendChild(doc.CreateTextNode(product.Id.ToString()));

                    prod.AppendChild(prodId);

                    item.AppendChild(prod);

                    foreach (Serial serial in product.Serials)
                    {
                        XmlElement instance = doc.CreateElement("ItemInstance");
                        instance.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
                        
                        XmlElement srl = doc.CreateElement("SerialNumber");
                        srl.AppendChild(doc.CreateTextNode(serial.SerialNumber));
                        instance.AppendChild(srl);

                        instance.AppendChild(doc.CreateElement("Status"));

                        if (!string.IsNullOrEmpty(serial.PairedProduct) || !string.IsNullOrEmpty(serial.PairedSerial))
                        {
                            XmlElement paired = doc.CreateElement("PairedItemInstance");

                            if (!string.IsNullOrEmpty(serial.PairedProduct))
                            {
                                XmlElement pprod = doc.CreateElement("Product");

                                XmlElement pid = doc.CreateElement("ID");
                                pid.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");                                  

                                pid.AppendChild(doc.CreateTextNode(serial.PairedProduct));
                                pprod.AppendChild(pid);

                                paired.AppendChild(pprod);
                            }

                            if (!string.IsNullOrEmpty(serial.PairedSerial))
                            {
                                XmlElement pserial = doc.CreateElement("SerialNumber");
                                pserial.AppendChild(doc.CreateTextNode(serial.PairedSerial));
                                paired.AppendChild(pserial);
                            }

                            instance.AppendChild(paired);
                        }

                        item.AppendChild(instance);

                    }

                    itemLine.AppendChild(item);
                    activityLine.AppendChild(itemLine);
                    productActivity.AppendChild(activityLine);

                }
                doc.AppendChild(productActivity);

                return doc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SaveDocument(XmlDocument doc, AsnRecepcion recepcion)
        {
            SftpConfig config = new SftpConfig
            {
                Host = _configuration["SftpServerIp"],
                Port = int.Parse(_configuration["SftpServerPort"]),
                UserName = _configuration["SftpUser"],
                Password = _configuration["SftpPassword"]
            };

            //using SftpClient client = new SftpClient(new PasswordConnectionInfo(config.Host, config.Port, config.UserName, config.Password));
			using SftpClient client = new SftpClient(config.Host, config.Port, config.UserName, config.Password);            
            try
            {
                
                MemoryStream stream = new MemoryStream();
                doc.Save(stream);
                //doc.Save();

                stream.Position = 0;
				
				client.Connect();

                //"c:\wwwroot\dtv\aaa\mmm\ddd\"+filename
                filename = "INT015a_" + recepcion.DocumentType + "_" + recepcion.DocumentId.Replace(":","") + "_" + recepcion.DocumentTransactionId + ".xml";
                //doc.Save(""+ filename);
                if(recepcion.Country == "AR")
                    client.UploadFile(stream, _configuration["Int015a_data"] +"/"+ filename);
                else
                    client.UploadFile(stream, _configuration["Int015a_data_uy"] +"/"+ filename);
                //client.UploadFile(stream, @"\\data\" + filename);
                stream.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                client.Disconnect();
            }
        }
    }
}
