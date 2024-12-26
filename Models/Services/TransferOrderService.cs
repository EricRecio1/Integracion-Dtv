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

public class TransferOrderService
{

    private readonly IConfiguration _configuration;
    private readonly TransferOrderDAO _TransferOrderDao;

    public TransferOrderService(IntegracionDtvContext context, IConfiguration configuration)
    {
        _configuration = configuration;
        _TransferOrderDao = new TransferOrderDAO(context);
    }

    public void SaveData(TransfersOrder order)
    {
        DtvTransOrder toDb = new DtvTransOrder();
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
        toDb.Archivo = order.FileName;

        foreach (OrderLine orderLine in order.OrderLine)
        {
            toDb.CantItems = orderLine.ItemLinesQuantity;
            foreach (ItemLine producto in orderLine.ItemLine)
            {
                DtvTransProd ToDbLine = new DtvTransProd();
                ToDbLine.Clave = order.Message.ID.ToString();
                ToDbLine.FechaSys = DateTime.Now;
                ToDbLine.Usuario = "IntegOcasaDtv";
                ToDbLine.DescCorta = order.Integration.Code;
                ToDbLine.DescLarga = order.Integration.Operation;
                ToDbLine.Estado = "0";
                ToDbLine.IdMensaje = order.Message.ID;
                ToDbLine.CantProducto = producto.Quantity;
                ToDbLine.IdProducto = producto.Item.Product.ID;
                ToDbLine.OrganizacionOri = orderLine.OriginParty.Organization;
                ToDbLine.SubinventariOri = orderLine.OriginParty.SubInventory;
                ToDbLine.LocalizadorOri = orderLine.OriginParty.Tracker;
                toDb.DtvTransProds.Add(ToDbLine);
            }
        }
        _TransferOrderDao.Add(toDb);
    }

    public void ProcessFiles()
    {
        ProcessFiles(_configuration["Int010b_data"], _configuration["Int010b_archive"], _configuration["Int010b_error"], _configuration["Int010b_stage"]);
        ProcessFiles(_configuration["Int010b_data_uy"], _configuration["Int010b_archive_uy"], _configuration["Int010b_error_uy"], _configuration["Int010b_stage_uy"]);        
    }
    public void ProcessFiles(string data, string archive, string error, string stage)
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
            IEnumerable<SftpFile> res = client.ListDirectory(data);
            List<string> Files = new List<string>();
            Files = res.Select((SftpFile x) => x.FullName).ToList();
            for (int i = 0; i < Files.Count(); i++)
            {
                if ((res.ElementAt(i).Name != ".") && (res.ElementAt(i).Name != ".."))
                {
                    client.RenameFile(Files.ElementAt(i), stage + Path.GetFileName(Files.ElementAt(i)));
                    MemoryStream memoryStream = new MemoryStream();
                    client.DownloadFile(stage + Path.GetFileName(Files.ElementAt(i)), memoryStream);
                    memoryStream.Position = 0L;
                    StreamReader Reader = new StreamReader(memoryStream);
                    string text = Reader.ReadToEnd();
                    XElement xdoc = XElement.Parse(text);
                    (from a in xdoc.Descendants().Attributes()
                     where a.IsNamespaceDeclaration
                     select a).Remove();
                    xdoc = Utils.RemoveAllNamespaces(xdoc);
                    string XmlNormalizado = xdoc.ToString();
                    XmlSerializer serializer = new XmlSerializer(typeof(TransfersOrder));
                    using (StringReader Reader2 = new StringReader(XmlNormalizado))
                    {
                        TransfersOrder Order = (TransfersOrder)serializer.Deserialize(Reader2);
                        Order.FileName = res.ElementAt(i).Name;
                        JObject order = JObject.FromObject(Order);
                        IntegracionDtvContext context = new IntegracionDtvContext();
                        Utils.ProcessLog(order, lectura: true, exito: false, Outbound: true, context);
                        string[] values = new string[14]
                        {
                        TypeDocumentConfig.int010b,
                        TypeDocumentConfig.INT010b,
                        TypeDocumentConfig.PE,
                        TypeDocumentConfig.PH,
                        TypeDocumentConfig.PL,
                        TypeDocumentConfig.PM,
                        TypeDocumentConfig.PV,
                        TypeDocumentConfig.PP,
                        TypeDocumentConfig.PW,
                        TypeDocumentConfig.TP,
                        TypeDocumentConfig.PG,
                        TypeDocumentConfig.PQ,
                        TypeDocumentConfig.TR,
                        TypeDocumentConfig.TW
                        };
                        if (Utils.Validation(order, client, values))
                        {
                            try
                            {
                                SaveData(Order);
                                client.RenameFile(stage + Path.GetFileName(Files.ElementAt(i)), archive + Path.GetFileName(Files.ElementAt(i)));
                                Utils.ProcessLog(order, lectura: false, exito: true, Outbound: true, context);
                            }
                            catch (Exception ex)
                            {
                                //MailHelper.SendMail("Falla persistencia de la base de datos, IntegracionDTV");
                                client.RenameFile(stage + Path.GetFileName(Files.ElementAt(i)), error + Path.GetFileName(Files.ElementAt(i)));
                                Utils.ProcessLog(order, lectura: false, exito: false, Outbound: true, context);
                                throw new Exception(ex.Message + "catch de base de datos");
                            }
                        }
                        else
                        {
                            //MailHelper.SendMail("Falla validacion de formato del documento TransferOrderService");
                            client.RenameFile(stage + Path.GetFileName(Files.ElementAt(i)), error + Path.GetFileName(Files.ElementAt(i)));
                            Utils.ProcessLog(order, lectura: false, exito: false, Outbound: true, context);
                        }
                    }
                    memoryStream.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message +" - "+ ex.StackTrace);
            //MailHelper.SendMail($"{ex}" + " TransferOrder");
        }
        finally
        {
            client.Disconnect();
        }
    }
}
