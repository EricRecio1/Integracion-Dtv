using System;
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

public class ReceiptAdviceService
{
    private readonly IConfiguration _configuration;

    private readonly ReceiptAdviceDAO _receiptAdviceDAO;

    private string path = "";

    private bool isUyu;

    public ReceiptAdviceService(IntegracionDtvContext context, IConfiguration configuration)
    {
        _receiptAdviceDAO = new ReceiptAdviceDAO(context);
        _configuration = configuration;
    }

    public void GenerateReception()
    {
        try
        {
            DtvRecepSucur[] receptions = _receiptAdviceDAO.GetUnprocessed();
            DtvRecepSucur[] array = receptions;
            foreach (DtvRecepSucur reception in array)
            {
                XmlDocument doc = GenerateXml(reception);
                if (reception.IdPais == "AR")
                {
                    path = _configuration["Int038_data"];
                }
                else
                {
                    path = _configuration["Int038_data_uy"];
                }
                reception.Archivo = SaveDocument(doc, reception);
                JObject order = JObject.FromObject(doc);
                IntegracionDtvContext context = new IntegracionDtvContext();
                reception.Processed = true;
                _receiptAdviceDAO.Update(reception);
            }
        }
        catch (Exception ex)
        {
            MailHelper.SendMail(ex.Message + " ReceiptAdvice");
        }
    }

    private XmlDocument GenerateXml(DtvRecepSucur reception)
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
        messageId.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
        messageId.AppendChild(doc.CreateTextNode(reception.IdMensaje.ToString()));
        XmlElement messageDate = doc.CreateElement("DateTime");
        messageDate.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
        messageDate.AppendChild(doc.CreateTextNode(reception.FechaMensaje));
        message.AppendChild(messageId);
        message.AppendChild(messageDate);
        productActivity.AppendChild(message);
        XmlElement integration = doc.CreateElement("Integration");
        XmlElement code = doc.CreateElement("Code");
        code.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
        code.AppendChild(doc.CreateTextNode(reception.IdIntegracion));
        XmlElement process = doc.CreateElement("Process");
        process.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
        process.AppendChild(doc.CreateTextNode(reception.IntegProceso));
        XmlElement operation = doc.CreateElement("Operation");
        operation.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
        operation.AppendChild(doc.CreateTextNode(reception.IntegOperacion));
        integration.AppendChild(code);
        integration.AppendChild(process);
        integration.AppendChild(operation);
        productActivity.AppendChild(integration);
        XmlElement country = doc.CreateElement("Country");
        country.AppendChild(doc.CreateTextNode(reception.IdPais));
        productActivity.AppendChild(country);
        XmlElement documentType = doc.CreateElement("DocumentType");
        documentType.AppendChild(doc.CreateTextNode(reception.TipoDocumento));
        productActivity.AppendChild(documentType);
        XmlElement documentId = doc.CreateElement("DocumentId");
        documentId.AppendChild(doc.CreateTextNode(reception.IdDocumento));
        productActivity.AppendChild(documentId);
        XmlElement documentDate = doc.CreateElement("DocumentDateTime");
        documentDate.AppendChild(doc.CreateTextNode(reception.FecDocumento));
        productActivity.AppendChild(documentDate);
        XmlElement receiver = doc.CreateElement("ReceiverParty");
        XmlElement organization = doc.CreateElement("Organization");
        organization.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
        organization.AppendChild(doc.CreateTextNode(reception.OrganizacionDes));
        XmlElement subinventory = doc.CreateElement("SubInventory");
        subinventory.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
        subinventory.AppendChild(doc.CreateTextNode(reception.SubinventariDes));
        XmlElement tracker = doc.CreateElement("Tracker");
        tracker.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
        if (!string.IsNullOrEmpty(reception.LocalizadorDes))
        {
            tracker.AppendChild(doc.CreateTextNode(reception.LocalizadorDes));
        }
        receiver.AppendChild(organization);
        receiver.AppendChild(subinventory);
        receiver.AppendChild(tracker);
        productActivity.AppendChild(receiver);
        XmlElement lineQty = doc.CreateElement("ProductActivityLinesQuantity");
        lineQty.AppendChild(doc.CreateTextNode("1"));
        productActivity.AppendChild(lineQty);
        XmlElement activityLine = doc.CreateElement("ProductActivityLine");
        XmlElement actLineQty = doc.CreateElement("ItemLinesQuantity");
        actLineQty.SetAttribute("xmlns", "dtv-3pl:schema:xsd:DocumentContentComponents");
        actLineQty.AppendChild(doc.CreateTextNode(reception.CantItems.ToString()));
        activityLine.AppendChild(actLineQty);
        DtvRecepProdu firstProd = reception.DtvRecepProdus.First();
        if (firstProd != null && !string.IsNullOrEmpty(firstProd.OrganizacionOri))
        {
            XmlElement origin = doc.CreateElement("OriginParty");
            origin.SetAttribute("xmlns", "dtv-3pl:schema:xsd:DocumentContentComponents");
            XmlElement orgOrganization = doc.CreateElement("Organization");
            orgOrganization.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
            orgOrganization.AppendChild(doc.CreateTextNode(firstProd.OrganizacionOri));
            XmlElement orgSubinventory = doc.CreateElement("SubInventory");
            orgSubinventory.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
            orgSubinventory.AppendChild(doc.CreateTextNode(firstProd.SubinventariOri));
            XmlElement orgTracker = doc.CreateElement("Tracker");
            orgTracker.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
            if (!string.IsNullOrEmpty(firstProd.LocalizadorOri))
            {
                orgTracker.AppendChild(doc.CreateTextNode(firstProd.LocalizadorOri));
            }
            origin.AppendChild(orgOrganization);
            origin.AppendChild(orgSubinventory);
            origin.AppendChild(orgTracker);
            activityLine.AppendChild(origin);
        }
        foreach (DtvRecepProdu product in reception.DtvRecepProdus)
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
            prodId.AppendChild(doc.CreateTextNode(product.IdProducto));
            prod.AppendChild(prodId);
            item.AppendChild(prod);
            foreach (DtvRecepSerie serial in product.DtvRecepSeries)
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
            activityLine.AppendChild(itemLine);
            productActivity.AppendChild(activityLine);
        }
        doc.AppendChild(productActivity);
        return doc;
    }

    private string SaveDocument(XmlDocument doc, DtvRecepSucur order)
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
            string filename = "INT038_" + order.TipoDocumento + "_" + order.IdDocumento + "_" + Regex.Replace(order.FecDocumento, "[^0-9]", "") + ".xml";
            client.UploadFile(stream, path + filename);
            stream.Dispose();
            return filename;
        }
        catch (Exception ex)
        {
            MailHelper.SendMail($"{ex}" + " ReceiptAdvice");
            return ex.Message;
        }
        finally
        {
            client.Disconnect();
        }
    }
}
