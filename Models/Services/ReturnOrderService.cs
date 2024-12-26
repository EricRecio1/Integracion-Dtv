// IntegracionOcasaDtv, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// IntegracionOcasaDtv.Models.Services.ReturnOrderService
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

public class ReturnOrderService
{
    private readonly ReturnOrderDAO _returnOrderDAO;

    private readonly IConfiguration _configuration;

    private string path;

    private string pathArchive;

    private string pathError;

    private string pathStage;

    private bool isUsyu;

    public ReturnOrderService(IntegracionDtvContext _context, IConfiguration configuration)
    {
        _returnOrderDAO = new ReturnOrderDAO(_context);
        _configuration = configuration;
    }

    public void SaveData(ReturnsOrder order)
    {
        DtvDevolPedid toDb = new DtvDevolPedid();
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
        toDb.FecEstimEntrega = order.EstimatedDateTime;
        toDb.IdModeloFactura = order.BillingModel.Code;
        toDb.DescModFactura = order.BillingModel.Description;
        toDb.OrganizacionOri = order.SenderParty.Organization;
        toDb.SubinventariOri = order.SenderParty.SubInventory;
        toDb.LocalizadorOri = order.SenderParty.Tracker;
        toDb.OrganizacionDes = order.ReceiverParty.Organization;
        toDb.SubinventariDes = order.ReceiverParty.SubInventory;
        toDb.LocalizadorDes = order.ReceiverParty.Tracker;
        toDb.DireccionDest = order.Address._Address;
        toDb.Cpdestino = order.Address.PostalCode;
        toDb.LocalidadDest = order.Address.CitySubdivisionName;
        toDb.ProvinciaDest = order.Address.CountrySubentity;
        toDb.IdPaisDest = order.Address.Country;
        toDb.GeoLatitud = order.Address.Geolocation.Latitude;
        toDb.GeoLongitud = order.Address.Geolocation.Longitude;
        toDb.ContactoDescrip = order.Contact.Name;
        toDb.ContactoTelefon = order.Contact.Telephone;
        toDb.ContactoFax = order.Contact.Fax;
        toDb.ContactoEmail = order.Contact.Email;
        toDb.Observaciones = order.OrderNote;
        toDb.CantItems = order.OrderLinesQuantity;
        toDb.Archivo = order.FileName;
        foreach (OrderLineReturnOrder producto in order.OrderLine)
        {
            DtvDevolProd toDbLine = new DtvDevolProd();
            toDbLine.Clave = order.Message.ID.ToString();
            toDbLine.FechaSys = DateTime.Now;
            toDbLine.Usuario = "IntegOcasaDtv";
            toDbLine.DescCorta = order.Integration.Code;
            toDbLine.DescLarga = order.Integration.Operation;
            toDbLine.Estado = "0";
            toDbLine.IdMensaje = order.Message.ID;
            toDbLine.CantProducto = producto.ItemLine.Quantity;
            toDbLine.IdProducto = producto.ItemLine.Item.Product.ID;
            foreach (ItemInstance instancia in producto.ItemLine.Item.ItemInstance)
            {
                if (!(instancia.SerialNumber != "") || instancia.SerialNumber == null)
                {
                    continue;
                }
                DtvDevolSerie toDbInstance = new DtvDevolSerie();
                toDbInstance.Clave = order.Message.ID.ToString();
                toDbInstance.FechaSys = DateTime.Now;
                toDbInstance.Usuario = "IntegOcasaDtv";
                toDbInstance.DescCorta = order.Integration.Code;
                toDbInstance.DescLarga = order.Integration.Operation;
                toDbInstance.Estado = "0";
                toDbInstance.IdProducto = producto.ItemLine.Item.Product.ID;
                toDbInstance.NroSerie = instancia.SerialNumber;
                toDbInstance.Status = instancia.Status;
                toDbInstance.IdMensaje = order.Message.ID;
                foreach (string falla in instancia.Flaw.HistoricFlaw)
                {
                    if (falla != "" && falla != null)
                    {
                        DtvDevolFalla toDbFalla = new DtvDevolFalla();
                        toDbFalla.Clave = order.Message.ID.ToString();
                        toDbFalla.FechaSys = DateTime.Now;
                        toDbFalla.Usuario = "IntegOcasaDtv";
                        toDbFalla.DescCorta = order.Integration.Code;
                        toDbFalla.DescLarga = order.Integration.Operation;
                        toDbFalla.Estado = "0";
                        toDbFalla.Falla = falla;
                        toDbInstance.DtvDevolFallas.Add(toDbFalla);
                    }
                }
                toDbLine.DtvDevolSeries.Add(toDbInstance);
            }
            toDb.DtvDevolProds.Add(toDbLine);
        }
        _returnOrderDAO.Add(toDb);
    }

    public void ProcessFiles()
    {
        if (!isUsyu)
        {
            path = _configuration["Int012_data"];
            pathArchive = _configuration["Int012_archive"];
            pathError = _configuration["Int012_error"];
            pathStage = _configuration["Int012_stage"];
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
                xdoc = Utils.RemoveAllNamespaces(xdoc);
                string XmlNormalizado = xdoc.ToString();
                XmlSerializer serializer = new XmlSerializer(typeof(ReturnsOrder));
                using (StringReader Reader2 = new StringReader(XmlNormalizado))
                {
                    ReturnsOrder Order = (ReturnsOrder)serializer.Deserialize(Reader2);
                    Order.FileName = res.ElementAt(i).Name;
                    JObject order = JObject.FromObject(Order);
                    IntegracionDtvContext context = new IntegracionDtvContext();
                    Utils.ProcessLog(order, lectura: true, exito: false, Outbound: true, context);
                    string[] values = new string[15]
                    {
                        TypeDocumentConfig.int012,
                        TypeDocumentConfig.INT012,
                        TypeDocumentConfig.Retiros_Laboratorio,
                        TypeDocumentConfig.Retiros,
                        TypeDocumentConfig.Retiro_Herramientas,
                        TypeDocumentConfig.Retiro_Interno,
                        TypeDocumentConfig.Reversiones_de_Laboratorio,
                        TypeDocumentConfig.Retiro_Prepago,
                        TypeDocumentConfig.Retiro_Indumentaria,
                        TypeDocumentConfig.Retiros_WiMax,
                        TypeDocumentConfig.Entrega_Retiro_en_OL,
                        TypeDocumentConfig.REF,
                        TypeDocumentConfig.PL,
                        TypeDocumentConfig.TR,
                        TypeDocumentConfig.RT
                    };
                    if (Utils.Validation(order, client, values))
                    {
                        SaveData(Order);
                        client.RenameFile(pathStage + Path.GetFileName(Files.ElementAt(i)), pathArchive + Path.GetFileName(Files.ElementAt(i)));
                        Utils.ProcessLog(order, lectura: false, exito: true, Outbound: true, context);
                    }
                    else
                    {
                        MailHelper.SendMail("Falla validacion del documento");
                        client.RenameFile(pathStage + Path.GetFileName(Files.ElementAt(i)), pathError + Path.GetFileName(Files.ElementAt(i)));
                        Utils.ProcessLog(order, lectura: false, exito: false, Outbound: true, context);
                    }
                }
                memoryStream.Dispose();
            }
            if (!isUsyu)
            {
                path = _configuration["Int012_data_uy"];
                pathArchive = _configuration["Int012_archive_uy"];
                pathStage = _configuration["Int012_stage_uy"];
                pathError = _configuration["Int012_error_uy"];
                isUsyu = true;
                ProcessFiles();
            }
        }
        catch (Exception ex)
        {
            MailHelper.SendMail($"{ex}" + " ReturnOrder");
        }
        finally
        {
            client.Disconnect();
        }
    }
}
