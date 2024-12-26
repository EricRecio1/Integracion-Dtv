using IntegracionOcasaDtv.Models.Entities;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System.Linq;
using System.Xml.Linq;
using IntegracionOcasaDtv.Models.Config;
using IntegracionOcasaDtv.Models.DAO;
using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntegracionOcasaDtv.Models.Utils
{
    public class Utils
    {
        protected readonly DtvLogDAO _dtvLogDao;
        public static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
        public static bool Validation(JObject orden, SftpClient client, string[] parametrosValidacion)
        {
            try
            {
                List<Error> errors = new List<Error>();

                string[] filename = (orden["FileName"]).ToString().Split("_");

                //message
                JToken message = orden["Message"];
                if (message.Type == JTokenType.Null)
                {
                    errors.Add(new Error()
                    {
                        Summary = "Falta cabecera: Message.",
                        Detail = "No se encuentra presente una de las cabeceras requeridas."
                    });
                }
                else if (parametrosValidacion[0] != "int009" && parametrosValidacion[0] !="int008")
                {
                    if (filename[3] != orden["Message"]["ID"].ToString())
                    {
                        errors.Add(new Error()
                        {
                            Summary = "Numero de transaccion incorrecto",
                            Detail = "El numero de transaccion en el nombre del archivo no corresponde con el contenido en el archivo."
                        });
                    }
                }
                else if(parametrosValidacion[0] != "int008")
                {
                    if (filename[3] != orden["TransactionId"].ToString())
                    {
                        errors.Add(new Error()
                        {
                            Summary = "Numero de transaccion incorrecto",
                            Detail = "El numero de transaccion en el nombre del archivo no corresponde con el contenido en el archivo."
                        });
                    }
                }

                //integration
                JToken integration = orden["Integration"];
                if (integration.Type == JTokenType.Null)
                {
                    errors.Add(new Error()
                    {
                        Summary = "Falta cabecera: Integration.",
                        Detail = "No se encuentra presente una de las cabeceras requeridas."
                    });
                }

                if (filename[0] != parametrosValidacion[1])
                {
                    errors.Add(new Error()
                    {
                        Summary = "Codigo de integracion incorrecto",
                        Detail = "El codigo de integracion en el nombre del archivo no corresponde con el del contenido."
                    });
                }

                //documentType
                if (parametrosValidacion[0] != "int009")
                {
                    JToken documentType = orden["DocumentType"];
                    if (documentType.Type == JTokenType.Null)
                    {
                        errors.Add(new Error()
                        {
                            Summary = "Falta cabecera: DocumentType.",
                            Detail = "No se encuentra presente una de las cabeceras requeridas."
                        });
                    }
                    else
                    {
                        if (filename[1] != orden["DocumentType"].ToString() && parametrosValidacion[0] != "int008")
                        {
                            errors.Add(new Error()
                            {
                                Summary = "Tipo de documento incorrecto",
                                Detail = "El tipo de documento en el nombre del archivo no es el que corresponde."
                            });
                        }
                    }
                }
                int cantErr = 0;
                for (int i = 2; i < parametrosValidacion.Length; i++)
                {
                    if (filename[1] != parametrosValidacion[i])
                        cantErr++;
                }
                if (cantErr == parametrosValidacion.Length - 2)
                {
                    errors.Add(new Error()
                    {
                        Summary = "Tipo de documento incorrecto",
                        Detail = "El tipo de documento en el nombre del archivo no es el que corresponde."
                    });
                }

                //country
                JToken country = orden["Country"];
                if (country.Type == JTokenType.Null)
                {
                    errors.Add(new Error()
                    {
                        Summary = "Falta cabecera: Country.",
                        Detail = "No se encuentra presente una de las cabeceras requeridas."
                    });
                }

                //documentDateTime
                if (parametrosValidacion[0] != "int009")
                {
                    JToken documentDateTime = orden["DocumentDateTime"];
                    if (documentDateTime.Type == JTokenType.Null)
                    {
                        errors.Add(new Error()
                        {
                            Summary = "Falta cabecera: DocumentDateTime.",
                            Detail = "No se encuentra presente una de las cabeceras requeridas."
                        });
                    }
                }else
                {
                    JToken documentDateTime = orden["PurchaseOrderDateTime"];
                    if (documentDateTime.Type == JTokenType.Null)
                    {
                        errors.Add(new Error()
                        {
                            Summary = "Falta cabecera: DocumentDateTime.",
                            Detail = "No se encuentra presente una de las cabeceras requeridas."
                        });
                    }
                }

                //document id
                if (parametrosValidacion[0] != "int009")
                {
                    JToken documentId = orden["DocumentId"];
                    if (documentId.Type == JTokenType.Null)
                    {
                        errors.Add(new Error()
                        {
                            Summary = "Falta cabecera: DocumentId.",
                            Detail = "No se encuentra presente una de las cabeceras requeridas."
                        });
                    }
                    else
                    {
                        if (filename[2] != orden["DocumentId"].ToString())
                        {
                            errors.Add(new Error()
                            {
                                Summary = $"Numero de DocumentId incorrecto",
                                Detail = $"El numero de DocumentId en el nombre del archivo no corresponde con el contenido en el archivo."
                            });
                        }
                    }
                }
                else
                {
                    if (filename[2] != orden["PurchaseOrder"].ToString())
                    {
                        errors.Add(new Error()
                        {
                            Summary = $"Numero de {filename[1]} incorrecto",
                            Detail = $"El numero de {filename[1]} en el nombre del archivo no corresponde con el contenido en el archivo."
                        });
                    }
                }

                //linesQuantity
                if (parametrosValidacion[0] != "int009")
                {
                    if ((int)orden["OrderLinesQuantity"] == 1 && parametrosValidacion[0] != "int012")
                    {
                        //if (((int)orden["OrderLine"]["ItemLinesQuantity"]) !=
                        //(orden["OrderLine"]["ItemLine"].Count()))
                        //{
                        //    errors.Add(new Error()
                        //    {
                        //        Summary = "Cantidad de lineas incorrecta",
                        //        Detail = "La cantidad de lineas no se corresponde con el numero de lineas efectivamente presentes."
                        //    });
                        //}
                    }else 
                    {
                        //foreach (var line in orden["OrderLine"])
                        //{
                        //    if (line["ItemLine"]["Item"]?["ItemInstance"] != null)
                        //    {
                        //        if (((int)orden["OrderLinesQuantity"] != 1 && parametrosValidacion[0] != "int012")  &&
                        //            (int)line["ItemLine"]["Quantity"] !=
                        //        (line["ItemLine"]["Item"]["ItemInstance"].Count()))
                        //        {
                        //            errors.Add(new Error()
                        //            {
                        //                Summary = "Cantidad de lineas incorrecta",
                        //                Detail = "La cantidad de lineas no se corresponde con el numero de lineas efectivamente presentes."
                        //            });
                        //        }
                        //    }
                            
                        //}
                    }
                }

                if (parametrosValidacion[0] == "int009")
                {
                    
                    {
                        if(orden["Lines"].Count() != (int)orden["LinesQty"])
                        {
                            errors.Add(new Error()
                            {
                                Summary = "Cantidad de lineas incorrecta",
                                Detail = "La cantidad de lineas no se corresponde con el numero de lineas efectivamente presentes."
                            });
                        }

                        foreach (var palet in orden["Lines"])
                        {
                            /*JToken serials = orden["Serials"];
                            if (serials != null)
                            {
                                if (palet["Serials"].Count() != (int)palet["Quantity"])
                                {
                                    errors.Add(new Error()
                                    {
                                        Summary = "Cantidad de lineas incorrecta",
                                        Detail = "La cantidad de lineas no se corresponde con el numero de lineas efectivamente presentes."
                                    });
                                }
                            }*/
                        }
                    }                   
                }

                //fin validacion
                if (errors.Count() > 0)
                {
                    Entities.Message nullMessage = null;
                    Entities.Integration nullIntegration = null;

                    ErrorLog errorLog = new ErrorLog()
                    {
                        Message = message.Type == JTokenType.Null? nullMessage : orden["Message"].ToObject<Entities.Message>(),
                        Integration = integration.Type == JTokenType.Null? nullIntegration: orden["Integration"].ToObject<Entities.Integration>(),
                        Country = country.Type == JTokenType.Null ? "":orden["Country"].ToString(),
                        DocumentType = orden.ContainsKey("DocumentType") ? orden["DocumentType"].ToString() : parametrosValidacion[0] != "int009" ? "PO":"",
                        DocumentId = orden.ContainsKey("DocumentId") ? (orden["DocumentId"].Type == JTokenType.Null ? "" : orden["DocumentId"].ToString()) : orden["PurchaseOrder"].ToString(),
                        ProcessData = new ProcessData()
                        {
                            Entity = integration.Type == JTokenType.Null ? "":orden["Integration"]["Process"].ToString(),
                            Value = "",
                            ProcessDate = message.Type == JTokenType.Null ? "" : orden["Message"]["DateTime"].ToString(),
                            ErrorDetail = new ErrorDetail()
                            {
                                Code = "",
                                Summary = string.Join(", ", errors.Select(e => e.Summary)),
                                Detail = string.Join("\n", errors.Select(e => e.Detail)),
                                ErrorDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")
                            }
                        }
                    };

                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(errorLog.GetType());

                    MemoryStream stream = new MemoryStream();
                    serializer.Serialize(stream, errorLog);
                    stream.Position = 0;

                    string error_file = orden["FileName"].ToString().Split(".")[0] + ".err";
                    //client.UploadFile(stream, parametrosValidacion[0] + "/error/" + error_file);

                    stream.Dispose();

                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static void ProcessLog(JObject loggedObject, bool lectura,bool exito, bool Outbound, IntegracionDtvContext context)
        {
            try
            {
                string evento = lectura ? "Lectura de archivo SFTP" : exito ? "Procesamiento de archivo exitoso" : "Procesamiento de archivo fallido";
                string tipoArchivo = Outbound ? "Outbound" : "Inbound";
                string origen = Outbound ? "DTV" : "Ocasa";
                string destino = Outbound ? "Ocasa" : "DTV";

                DtvLogDAO _dtvLogDao;
                _dtvLogDao = new DtvLogDAO(context);

                if (loggedObject.ContainsKey("ProductActivity"))
                {
                    loggedObject = (JObject)loggedObject.SelectToken("ProductActivity");
                }

                DtvLog dtvLog = new DtvLog();
                dtvLog.Clave = loggedObject.ContainsKey("Message") ? (loggedObject["Message"]?["ID"] != null ? loggedObject["Message"]["ID"].ToString() : null) : null;
                dtvLog.FechaSys = DateTime.Now;
                dtvLog.Usuario = "IntegOcasaDtv"; 
                dtvLog.DescCorta = loggedObject.ContainsKey("Integration") ? (loggedObject["Integration"]?["Code"] != null ? loggedObject["Integration"]["Code"].ToString() : null) : null;
                dtvLog.DescLarga = loggedObject.ContainsKey("Integration") ? (loggedObject["Integration"]?["Operation"] != null ? loggedObject["Integration"]["Operation"].ToString() : null) : null;
                dtvLog.Estado = "0";
                dtvLog.FechaHora = DateTime.Now;
                dtvLog.Evento = evento;
                dtvLog.IdMensaje = loggedObject.ContainsKey("Message") ? (loggedObject["Message"]?["ID"] != null ? (long)loggedObject["Message"]["ID"] : 0) : 0;
                dtvLog.NombreArchivo = loggedObject.ContainsKey("FileName") ? loggedObject["FileName"].ToString() : "No está disponible";
                dtvLog.IdIntegracion = loggedObject.ContainsKey("Integration") ? (loggedObject["Integration"]?["Code"] != null ? loggedObject["Integration"]["Code"].ToString() : null) : null;
                dtvLog.IntegProceso = loggedObject.ContainsKey("Integration") ? (loggedObject["Integration"]?["Process"] != null ? loggedObject["Integration"]["Process"].ToString() : null) : null;
                dtvLog.IntegOperacion = loggedObject.ContainsKey("Integration") ? (loggedObject["Integration"]?["Operation"] != null ? loggedObject["Integration"]["Operation"].ToString() : null) : null;
                dtvLog.TipoArchivo = tipoArchivo;
                dtvLog.Origen = origen;
                dtvLog.Destino = destino;

                
                _dtvLogDao.Add(dtvLog);
               
            }
            catch (Exception e)
            {
                //throw;
            }
            
        }
    }
}
