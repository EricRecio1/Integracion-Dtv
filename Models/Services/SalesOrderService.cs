
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using IntegracionOcasaDtv;
using IntegracionOcasaDtv.Models.Config;
using IntegracionOcasaDtv.Models.DAO;
using IntegracionOcasaDtv.Models.DBEntities;
using IntegracionOcasaDtv.Models.Entities;
using IntegracionOcasaDtv.Models.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using Renci.SshNet.Sftp;

public class SalesOrderService
{
    private readonly IConfiguration _configuration;

    private readonly TransferOrderDAO _salesOrderDao;

    private string path;

    private string pathArchive;

    private string pathError;

    private string pathStage;

    private bool isUyu;

    public SalesOrderService(IntegracionDtvContext context, IConfiguration configuration)
    {
        _configuration = configuration;
        _salesOrderDao = new TransferOrderDAO(context);
    }

    private static XElement RemoveAllNamespaces(XElement xmlDocument)
    {
        if (!xmlDocument.HasElements)
        {
            XElement xElement = new XElement(xmlDocument.Name.LocalName);
            xElement.Value = xmlDocument.Value;
            {
                foreach (XAttribute attribute in xmlDocument.Attributes())
                {
                    xElement.Add(attribute);
                }
                return xElement;
            }
        }
        return new XElement(xmlDocument.Name.LocalName, from el in xmlDocument.Elements()
                                                        select RemoveAllNamespaces(el));
    }

    public void SaveData(TransfersOrder order)
    {
        DtvTransOrder soDb = new DtvTransOrder();
        soDb.Clave = order.Message.ID.ToString();
        soDb.FechaSys = DateTime.Now;
        soDb.Usuario = "IntegOcasaDtv";
        soDb.DescCorta = order.Integration.Code;
        soDb.DescLarga = order.Integration.Operation;
        soDb.Estado = "0";
        soDb.IdMensaje = order.Message.ID;
        soDb.FechaMensaje = order.Message.DateTime;
        soDb.IdIntegracion = order.Integration.Code;
        soDb.IntegProceso = order.Integration.Process;
        soDb.IntegOperacion = order.Integration.Operation;
        soDb.IdPais = order.Country;
        soDb.TipoDocumento = order.DocumentType;
        soDb.IdDocumento = order.DocumentId;
        soDb.FecDocumento = order.DocumentDateTime;
        soDb.FecEstimEntrega = order.EstimatedDateTime;
        soDb.IdModeloFactura = order.BillingModel.Code;
        soDb.DescModFactura = order.BillingModel.Description;
        soDb.OrganizacionDes = order.ReceiverParty.Organization;
        soDb.SubinventariDes = order.ReceiverParty.SubInventory;
        soDb.LocalidadDest = order.ReceiverParty.Tracker;
        soDb.DireccionDest = order.Address._Address;
        soDb.Cpdestino = order.Address.PostalCode;
        soDb.LocalidadDest = order.Address.CitySubdivisionName;
        soDb.ProvinciaDest = order.Address.CountrySubentity;
        soDb.IdPaisDest = order.Address.Country;
        soDb.GeoLatitud = order.Address.Geolocation.Latitude;
        soDb.GeoLongitud = order.Address.Geolocation.Longitude;
        soDb.ContactoDescrip = order.Contact.Name;
        soDb.ContactoTelefon = order.Contact.Telephone;
        soDb.ContactoFax = order.Contact.Fax;
        soDb.ContactoEmail = order.Contact.Email;
        soDb.Observaciones = order.OrderNote;
        soDb.Archivo = order.FileName;
        foreach (OrderLine orderLine in order.OrderLine)
        {
            soDb.CantItems = orderLine.ItemLinesQuantity;
            foreach (ItemLine producto in orderLine.ItemLine)
            {
                DtvTransProd soDbLine = new DtvTransProd();
                soDbLine.Clave = order.Message.ID.ToString();
                soDbLine.FechaSys = DateTime.Now;
                soDbLine.Usuario = "IntegOcasaDtv";
                soDbLine.DescCorta = order.Integration.Code;
                soDbLine.DescLarga = order.Integration.Operation;
                soDbLine.Estado = "0";
                soDbLine.IdMensaje = order.Message.ID;
                soDbLine.CantProducto = producto.Quantity;
                soDbLine.IdProducto = producto.Item.Product.ID;
                soDbLine.OrganizacionOri = orderLine.OriginParty.Organization;
                soDbLine.SubinventariOri = orderLine.OriginParty.SubInventory;
                soDbLine.LocalizadorOri = orderLine.OriginParty.Tracker;
                soDb.DtvTransProds.Add(soDbLine);
            }
        }
        _salesOrderDao.Add(soDb);
    }

    public void ProcessFiles()
    {
        if (!isUyu)
        {
            path = _configuration["Int010a_data"];
            pathArchive = _configuration["Int010a_archive"];
            pathError = _configuration["Int010a_error"];
            pathStage = _configuration["Int010a_stage"];
        }
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
            IEnumerable<SftpFile> res = client.ListDirectory(path);
            List<string> Files = new List<string>();
            Files = res.Select((SftpFile x) => x.FullName).ToList();
            for (int i = 0; i < Files.Count(); i++)
            {
                if (!(res.ElementAt(i).Name != ".") || !(res.ElementAt(i).Name != ".."))
                {
                    continue;
                }
                client.RenameFile(Files.ElementAt(i), pathStage + Path.GetFileName(Files.ElementAt(i)));
                MemoryStream memoryStream = new MemoryStream();
                client.DownloadFile(pathStage + Path.GetFileName(Files.ElementAt(i)), memoryStream);
                memoryStream.Position = 0L;
                StreamReader Reader = new StreamReader(memoryStream);
                string text = Reader.ReadToEnd();
                XElement xdoc = XElement.Parse(text);
                (from a in xdoc.Descendants().Attributes()
                 where a.IsNamespaceDeclaration
                 select a).Remove();
                xdoc = RemoveAllNamespaces(xdoc);
                string XmlNormalizado = xdoc.ToString();
                XmlSerializer serializer = new XmlSerializer(typeof(TransfersOrder));
                using (StringReader Reader2 = new StringReader(XmlNormalizado))
                {
                    TransfersOrder order = (TransfersOrder)serializer.Deserialize(Reader2);
                    order.FileName = res.ElementAt(i).Name;
                    JObject Order = JObject.FromObject(order);
                    string[] values = new string[3]
                    {
                        TypeDocumentConfig.int010a,
                        TypeDocumentConfig.INT010a,
                        TypeDocumentConfig.PP
                    };
                    if (Utils.Validation(Order, client, values))
                    {
                        SaveData(order);
                        client.RenameFile(pathStage + Path.GetFileName(Files.ElementAt(i)), pathArchive + Path.GetFileName(Files.ElementAt(i)));
                    }
                    else
                    {
                        MailHelper.SendMail("Falla de vlidacion del documento");
                        client.RenameFile(pathStage + Path.GetFileName(Files.ElementAt(i)), pathError + Path.GetFileName(Files.ElementAt(i)));
                    }
                }
                memoryStream.Dispose();
            }
            if (!isUyu)
            {
                path = _configuration["Int010a_data_uy"];
                pathArchive = _configuration["Int010a_archive_uy"];
                pathError = _configuration["Int010a_error_uy"];
                pathStage = _configuration["Int010a_stage_uy"];
                isUyu = true;
                ProcessFiles();
            }
        }
        catch (Exception ex)
        {
            MailHelper.SendMail($"{ex}" + " SalesOrder");
        }
        finally
        {
            client.Disconnect();
        }
    }
}
