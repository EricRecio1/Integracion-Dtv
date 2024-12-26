
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using IntegracionOcasaDtv;
using IntegracionOcasaDtv.Models.Config;
using IntegracionOcasaDtv.Models.DAO;
using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Renci.SshNet;

public class InventoryAdjustmentService
{
	private readonly IConfiguration _configuration;
	private readonly InventoryAdjustmentDAO _inventoryAdjustmentDao;
	private string path;

	public InventoryAdjustmentService(IConfiguration configuration, IntegracionDtvContext context)
	{
		_configuration = configuration;
		_inventoryAdjustmentDao = new InventoryAdjustmentDAO(context);
	}

	public void GenerateInventoryAdjustment()
	{
		try
		{
			DtvTraslado[] ajustes = _inventoryAdjustmentDao.GetUnprocessed();
			DtvTraslado[] array = ajustes;
			foreach (DtvTraslado ajuste in array)
			{
				XmlDocument doc = GenerateXml(ajuste);
				if (ajuste.IdPais == "AR")
				{
					path = _configuration["Int015c_data"];
				}
				else
				{
					path = _configuration["Int015c_data_uy"];
				}
				ajuste.Archivo = SaveDocument(doc, ajuste);
				JObject order = JObject.FromObject(doc);
				IntegracionDtvContext context = new IntegracionDtvContext();
				ajuste.Processed = true;
				_inventoryAdjustmentDao.Update(ajuste);
			}
		}
		catch (Exception ex)
		{
			MailHelper.SendMail($"{ex}" + " InventoryAdjusment");
		}
	}
	private XmlDocument GenerateXml(DtvTraslado ajuste)
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
		messageId.AppendChild(doc.CreateTextNode(ajuste.IdMensaje.ToString()));
		XmlElement messageDate = doc.CreateElement("DateTime");
		messageDate.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
		messageDate.AppendChild(doc.CreateTextNode(ajuste.FechaMensaje));
		message.AppendChild(messageId);
		message.AppendChild(messageDate);
		productActivity.AppendChild(message);
		XmlElement integration = doc.CreateElement("Integration");
		XmlElement code = doc.CreateElement("Code");
		code.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
		code.AppendChild(doc.CreateTextNode(ajuste.IdIntegracion));
		XmlElement process = doc.CreateElement("Process");
		process.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
		process.AppendChild(doc.CreateTextNode(ajuste.IntegProceso));
		XmlElement operation = doc.CreateElement("Operation");
		operation.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonBasicComponents");
		operation.AppendChild(doc.CreateTextNode(ajuste.IntegOperacion));
		integration.AppendChild(code);
		integration.AppendChild(process);
		integration.AppendChild(operation);
		productActivity.AppendChild(integration);
		XmlElement country = doc.CreateElement("Country");
		country.AppendChild(doc.CreateTextNode(ajuste.IdPais));
		productActivity.AppendChild(country);
		XmlElement documentType = doc.CreateElement("DocumentType");
		documentType.AppendChild(doc.CreateTextNode(ajuste.TipoDocumento));
		productActivity.AppendChild(documentType);
		XmlElement documentId = doc.CreateElement("DocumentId");
		documentId.AppendChild(doc.CreateTextNode(ajuste.IdDocumento));
		productActivity.AppendChild(documentId);
		XmlElement documentDate = doc.CreateElement("DocumentDateTime");
		documentDate.AppendChild(doc.CreateTextNode(ajuste.FecDocumento));
		productActivity.AppendChild(documentDate);

		if (!string.IsNullOrEmpty(ajuste.OrganizacionOri))
		{
			XmlElement origin = doc.CreateElement("SenderParty");
			XmlElement orgOrganization = doc.CreateElement("Organization");
			orgOrganization.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
			orgOrganization.AppendChild(doc.CreateTextNode(ajuste.OrganizacionOri));
			XmlElement orgSubinventory = doc.CreateElement("SubInventory");
			orgSubinventory.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
			orgSubinventory.AppendChild(doc.CreateTextNode(ajuste.SubinventariOri));
			XmlElement orgTracker = doc.CreateElement("Tracker");
			orgTracker.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
			if (!string.IsNullOrEmpty(ajuste.LocalizadorOri))
			{
				orgTracker.AppendChild(doc.CreateTextNode(ajuste.LocalizadorOri));
			}
			origin.AppendChild(orgOrganization);
			origin.AppendChild(orgSubinventory);
			origin.AppendChild(orgTracker);
			productActivity.AppendChild(origin);
		}
		XmlElement receiver = doc.CreateElement("ReceiverParty");
		XmlElement organization = doc.CreateElement("Organization");
		organization.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
		organization.AppendChild(doc.CreateTextNode(ajuste.OrganizacionDes));
		XmlElement subinventory = doc.CreateElement("SubInventory");
		subinventory.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
		subinventory.AppendChild(doc.CreateTextNode(ajuste.SubinventariDes));
		XmlElement tracker = doc.CreateElement("Tracker");
		tracker.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
		if (!string.IsNullOrEmpty(ajuste.LocalizadorDes))
		{
			tracker.AppendChild(doc.CreateTextNode(ajuste.LocalizadorDes));
		}
		receiver.AppendChild(organization);
		receiver.AppendChild(subinventory);
		receiver.AppendChild(tracker);
		productActivity.AppendChild(receiver);
		XmlElement observations = doc.CreateElement("ProductActivityNote");
		foreach (DtvTraslaObserv obs in ajuste.DtvTraslaObservs)
		{
			XmlElement observation = doc.CreateElement("ProductActivityNoteItem");
			observation.AppendChild(doc.CreateTextNode(obs.Observacion));
			observations.AppendChild(observation);
		}
		productActivity.AppendChild(observations);
		XmlElement lineQty = doc.CreateElement("ProductActivityLinesQuantity");
		lineQty.AppendChild(doc.CreateTextNode("1"));
		productActivity.AppendChild(lineQty);
		XmlElement activityLine = doc.CreateElement("ProductActivityLine");
		XmlElement actLineQty = doc.CreateElement("ItemLinesQuantity");
		actLineQty.SetAttribute("xmlns", "dtv-3pl:schema:xsd:DocumentContentComponents");
		actLineQty.AppendChild(doc.CreateTextNode(ajuste.CantItems.ToString()));
		activityLine.AppendChild(actLineQty);
		foreach (DtvTraslaProd product in ajuste.DtvTraslaProds)
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
			foreach (DtvTraslaSeri serial in product.DtvTraslaSeries)
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
						pprod.SetAttribute("xmlns", "dtv-3pl:schema:xsd:CommonAgreggateComponents");
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

	private string SaveDocument(XmlDocument doc, DtvTraslado order)
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
			string filename = order.IdIntegracion + "_" + order.TipoDocumento + "_" + order.IdDocumento + "_" + Regex.Replace(order.FecDocumento, "[^0-9]", "") + ".xml";
			client.UploadFile(stream, path + filename);
			stream.Dispose();
			return filename;
		}
		catch (Exception ex)
		{
			MailHelper.SendMail($"{ex}" + " InventoryAdjusment");
			return ex.Message;
		}
		finally
		{
			client.Disconnect();
		}
	}
}
