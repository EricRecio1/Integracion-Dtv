using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using IntegracionOcasaDtv;
using IntegracionOcasaDtv.Models.Config;
using IntegracionOcasaDtv.Models.DAO;
using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Renci.SshNet;

public class DespatchAdviceService
{
    private readonly IConfiguration _configuration;

    private readonly DespatchAdviceDAO _despatchAdviceDao;
    private string path;
    private bool isUyu;

    public DespatchAdviceService(IntegracionDtvContext context, IConfiguration configuration)
    {
        _despatchAdviceDao = new DespatchAdviceDAO(context);
        _configuration = configuration;
    }

    public void GenerateReception()
    {
        try
        {
            DtvDespaTran[] despachos = _despatchAdviceDao.GetUnprocessed();
            DtvDespaTran[] array = despachos;
            foreach (DtvDespaTran despacho in array)
            {
                XmlDocument doc = GenerateXml(despacho);
                if (despacho.IdPais == "AR")
                {
                    path = _configuration["Int015b_data"];
                }
                else
                {
                    path = _configuration["Int015b_data_uy"];
                }
                despacho.Archivo = SaveDocument(doc, despacho);
                JObject order = JObject.FromObject(doc);
                IntegracionDtvContext context = new IntegracionDtvContext();
                despacho.Processed = true;
                _despatchAdviceDao.Update(despacho);
            }
        }
        catch (Exception ex)
        {
            MailHelper.SendMail($"TransferOrderDespatch: {ex}");
        }
    }

    private XmlDocument GenerateXml(DtvDespaTran despacho)
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
            XmlElement message = doc.CreateElement("Message");
            XmlElement messageId = doc.CreateElement("ID");
            messageId.AppendChild(doc.CreateTextNode(despacho.IdMensaje.ToString()));
            messageId.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
            XmlElement messageDate = doc.CreateElement("DateTime");
            messageDate.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
            messageDate.AppendChild(doc.CreateTextNode(despacho.FechaMensaje));
            message.AppendChild(messageId);
            message.AppendChild(messageDate);
            productActivity.AppendChild(message);
            XmlElement integration = doc.CreateElement("Integration");
            XmlElement code = doc.CreateElement("Code");
            code.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
            code.AppendChild(doc.CreateTextNode(despacho.IdIntegracion));
            XmlElement process = doc.CreateElement("Process");
            process.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
            process.AppendChild(doc.CreateTextNode(despacho.IntegProceso));
            XmlElement operation = doc.CreateElement("Operation");
            operation.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
            operation.AppendChild(doc.CreateTextNode(despacho.IntegOperacion));
            integration.AppendChild(code);
            integration.AppendChild(process);
            integration.AppendChild(operation);
            productActivity.AppendChild(integration);
            XmlElement country = doc.CreateElement("Country");
            country.AppendChild(doc.CreateTextNode(despacho.IdPais));
            productActivity.AppendChild(country);
            XmlElement documentType = doc.CreateElement("DocumentType");
            documentType.AppendChild(doc.CreateTextNode(despacho.TipoDocumento));
            productActivity.AppendChild(documentType);
            XmlElement documentId = doc.CreateElement("DocumentId");
            documentId.AppendChild(doc.CreateTextNode(despacho.IdDocumento));
            productActivity.AppendChild(documentId);
            XmlElement documentDate = doc.CreateElement("DocumentDateTime");
            documentDate.AppendChild(doc.CreateTextNode(despacho.FecDocumento));
            productActivity.AppendChild(documentDate);
            XmlElement receiver = doc.CreateElement("ReceiverParty");
            XmlElement organization = doc.CreateElement("Organization");
            organization.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
            organization.AppendChild(doc.CreateTextNode(despacho.OrganizacionDes));
            XmlElement subinventory = doc.CreateElement("SubInventory");
            subinventory.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
            subinventory.AppendChild(doc.CreateTextNode(despacho.SubinventariDes));
            XmlElement tracker = doc.CreateElement("Tracker");
            tracker.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
            if (!string.IsNullOrEmpty(despacho.LocalizadorDes))
            {
                tracker.AppendChild(doc.CreateTextNode(despacho.LocalizadorDes));
            }
            receiver.AppendChild(organization);
            receiver.AppendChild(subinventory);
            receiver.AppendChild(tracker);
            productActivity.AppendChild(receiver);
            XmlElement lineQty = doc.CreateElement("ProductActivityLinesQuantity");
            if (despacho.SubinventariDes == "PROD_TERM")
            {
                lineQty.AppendChild(doc.CreateTextNode("2"));
            }
            else
            {
                List<DtvDespaProd> find = (from x in despacho.DtvDespaProds
                                           group x by x.SubinventariOri into y
                                           select y.First()).ToList();
                lineQty.AppendChild(doc.CreateTextNode(find.Count.ToString()));
            }
            productActivity.AppendChild(lineQty);
            if (despacho.IntegOperacion == "KITTING_WORK_ORDER_PRODUCTION_ADVICE")
            {
                XmlElement _doc = NewFormat_15b(despacho, doc, "1", productActivity);
                XmlElement actLineQty2 = doc.CreateElement("ItemLinesQuantity");
                actLineQty2.SetAttribute("xmlns", "dtv-3pl:schema:xsd:DocumentContentComponents");
                actLineQty2.AppendChild(doc.CreateTextNode((despacho.CantItems - 1).ToString()));
                XmlElement _doc2 = NewFormat_15b(despacho, doc, "2", productActivity);
            }
            else
            {
                doc.AppendChild(NewFormat_15b(despacho, doc, "3", productActivity));
            }
            doc.AppendChild(productActivity);
            return doc;
        }
        catch (Exception ex)
        {
            MailHelper.SendMail($"{ex}" + " DespatchAdvice");
            throw new Exception(ex.Message, ex.InnerException);
        }
    }

    private XmlElement NewFormat_15b(DtvDespaTran despacho, XmlDocument doc, string flag, XmlElement productActivity)
    {
        IEnumerable<DtvDespaProd> enumerable2;
        IEnumerable<DtvDespaProd> firstProd;
        if (!(flag == "1"))
        {
            IEnumerable<DtvDespaProd> enumerable = (firstProd = from x in despacho.DtvDespaProds.Where((DtvDespaProd x) => x.Type != "FINAL_PRODUCT").ToList()
                                                                orderby x.SubinventariOri == "DEPOSITO" descending
                                                                select x);
            enumerable2 = enumerable;
        }
        else
        {
            IEnumerable<DtvDespaProd> enumerable = despacho.DtvDespaProds.Where((DtvDespaProd x) => x.Type == "FINAL_PRODUCT").ToList();
            enumerable2 = enumerable;
        }
        firstProd = enumerable2;
        string subinventory = "";
        foreach (DtvDespaProd produc in firstProd)
        {
            if (firstProd == null || string.IsNullOrEmpty(produc.OrganizacionOri) || !(subinventory != produc.SubinventariOri))
            {
                continue;
            }
            XmlElement productActivityLine = doc.CreateElement("ProductActivityLine");
            XmlElement actLineQty = doc.CreateElement("ItemLinesQuantity");
            actLineQty.SetAttribute("xmlns", "dtv-3pl:schema:xsd:DocumentContentComponents");
            if (produc.Type == "FINAL_PRODUCT")
            {
                actLineQty.AppendChild(doc.CreateTextNode("1"));
            }
            else
            {
                actLineQty.AppendChild(doc.CreateTextNode(despacho.DtvDespaProds.Where((DtvDespaProd x) => x.SubinventariOri == produc.SubinventariOri).ToList().Count.ToString()));
            }
            productActivityLine.AppendChild(actLineQty);
            XmlElement origin = doc.CreateElement("OriginParty");
            origin.SetAttribute("xmlns", "dtv-3pl:schema:xsd:DocumentContentComponents");
            XmlElement orgOrganization = doc.CreateElement("Organization");
            orgOrganization.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
            orgOrganization.AppendChild(doc.CreateTextNode(produc.OrganizacionOri));
            XmlElement orgSubinventory = doc.CreateElement("SubInventory");
            orgSubinventory.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
            if (produc.Type == "FINAL_PRODUCT")
            {
                orgSubinventory.AppendChild(doc.CreateTextNode("PROD_TERM"));
                subinventory = "PROD_TERM";
            }
            else
            {
                orgSubinventory.AppendChild(doc.CreateTextNode(produc.SubinventariOri));
                subinventory = produc.SubinventariOri;
            }
            XmlElement orgTracker = doc.CreateElement("Tracker");
            orgTracker.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
            if (!string.IsNullOrEmpty(produc.LocalizadorOri))
            {
                orgTracker.AppendChild(doc.CreateTextNode(produc.LocalizadorOri));
            }
            origin.AppendChild(orgOrganization);
            origin.AppendChild(orgSubinventory);
            origin.AppendChild(orgTracker);
            productActivityLine.AppendChild(origin);
            IEnumerable<DtvDespaProd> _DtvDespaProds = despacho.DtvDespaProds.Where((DtvDespaProd x) => x.SubinventariOri == subinventory);
            foreach (DtvDespaProd product in _DtvDespaProds)
            {
                XmlElement itemLine = doc.CreateElement("ItemLine");
                itemLine.SetAttribute("xmlns", "dtv-3pl:schema:xsd:DocumentContentComponents");
                XmlElement qty = doc.CreateElement("Quantity");
                qty.AppendChild(doc.CreateTextNode(product.CantProducto.ToString()));
                itemLine.AppendChild(qty);
                XmlElement item = doc.CreateElement("Item");
                XmlElement prod = doc.CreateElement("Product");
                prod.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
                XmlElement prodId = doc.CreateElement("ID");
                prodId.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
                prodId.AppendChild(doc.CreateTextNode((!string.IsNullOrEmpty(product.IdProductoOrig)) ? product.IdProductoOrig : product.IdProducto));
                prod.AppendChild(prodId);
                item.AppendChild(prod);
                if (!string.IsNullOrEmpty(product.Type))
                {
                    XmlElement type = doc.CreateElement("Type");
                    type.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
                    type.AppendChild(doc.CreateTextNode(product.Type));
                    item.AppendChild(type);
                }
                if (!string.IsNullOrEmpty(product.IdProductoOrig))
                {
                    XmlElement substitute = doc.CreateElement("Substitute");
                    substitute.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
                    XmlElement substituteId = doc.CreateElement("ID");
                    substituteId.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
                    substituteId.AppendChild(doc.CreateTextNode(product.IdProducto));
                    substitute.AppendChild(substituteId);
                    item.AppendChild(substitute);
                }
                foreach (DtvDespaSerie serial in product.DtvDespaSeries)
                {
                    XmlElement instance = doc.CreateElement("ItemInstance");
                    instance.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
                    XmlElement srl = doc.CreateElement("SerialNumber");
                    srl.AppendChild(doc.CreateTextNode(serial.NroSerie));
                    instance.AppendChild(srl);
                    XmlElement status = doc.CreateElement("Status");
                    if (!string.IsNullOrEmpty(serial.Estado))
                    {
                        status.AppendChild(doc.CreateTextNode(serial.Estado));
                    }
                    instance.AppendChild(status);
                    if (!string.IsNullOrEmpty(serial.IdProductPaired) || !string.IsNullOrEmpty(serial.NroSeriePaired))
                    {
                        XmlElement pairedInstance = doc.CreateElement("PairedItemInstance");
                        if (!string.IsNullOrEmpty(serial.IdProductPaired))
                        {
                            XmlElement pprod = doc.CreateElement("Product");
                            XmlElement pprodId = doc.CreateElement("ID");
                            pprodId.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
                            pprodId.AppendChild(doc.CreateTextNode(serial.IdProductPaired));
                            pprod.AppendChild(pprodId);
                            pairedInstance.AppendChild(pprod);
                        }
                        if (!string.IsNullOrEmpty(serial.NroSeriePaired))
                        {
                            XmlElement psrl = doc.CreateElement("SerialNumber");
                            psrl.AppendChild(doc.CreateTextNode(serial.NroSeriePaired));
                            pairedInstance.AppendChild(psrl);
                        }
                        instance.AppendChild(pairedInstance);
                    }
                    item.AppendChild(instance);
                }
                itemLine.AppendChild(item);
                productActivityLine.AppendChild(itemLine);
            }
            productActivity.AppendChild(productActivityLine);
        }
        return productActivity;
    }

    private string SaveDocument(XmlDocument doc, DtvDespaTran order)
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
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            stream.Position = 0L;
            client.Connect();
            string filename = "INT015b_" + order.TipoDocumento + "_" + order.IdDocumento + "_" + Regex.Replace(order.FecDocumento, "[^0-9]", "") + ".xml";
            client.UploadFile(stream, path + filename);
            stream.Dispose();
            return filename;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            client.Disconnect();
        }
    }
}
