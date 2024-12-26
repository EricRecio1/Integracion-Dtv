// IntegracionOcasaDtv, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// IntegracionOcasaDtv.Models.DBEntities.DtvDespaProd
using System;
using System.Collections.Generic;
using IntegracionOcasaDtv.Models.DBEntities;

public class DtvDespaProd
{
    public string Clave { get; set; }
    public DateTime? FechaSys { get; set; }
    public DateTime? FechaVcia { get; set; }
    public string Usuario { get; set; }
    public string DescCorta { get; set; }
    public string DescLarga { get; set; }
    public string Estado { get; set; }
    public string IdProducto { get; set; }
    public int? CantProducto { get; set; }
    public string OrganizacionOri { get; set; }
    public string SubinventariOri { get; set; }
    public string LocalizadorOri { get; set; }
    public long IdDespaProd { get; set; }
    public long? IdMensaje { get; set; }
    public string Type { get; set; }
    public string IdProductoOrig { get; set; }
    public virtual DtvDespaTran IdMensajeNavigation { get; set; }
    public virtual ICollection<DtvDespaSerie> DtvDespaSeries { get; set; }
    public DtvDespaProd()
    {
        DtvDespaSeries = new HashSet<DtvDespaSerie>();
    }
}
