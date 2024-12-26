// IntegracionOcasaDtv, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// IntegracionOcasaDtv.Models.DBEntities.DtvDevolPedid
using System;
using System.Collections.Generic;
using IntegracionOcasaDtv.Models.DBEntities;

public class DtvDevolPedid
{
	public string Clave { get; set; }

	public DateTime? FechaSys { get; set; }

	public DateTime? FechaVcia { get; set; }

	public string Usuario { get; set; }

	public string DescCorta { get; set; }

	public string DescLarga { get; set; }

	public string Estado { get; set; }

	public long IdMensaje { get; set; }

	public string FechaMensaje { get; set; }

	public string IdIntegracion { get; set; }

	public string IntegProceso { get; set; }

	public string IntegOperacion { get; set; }

	public string IdPais { get; set; }

	public string TipoDocumento { get; set; }

	public string IdDocumento { get; set; }

	public string FecDocumento { get; set; }

	public string FecEstimEntrega { get; set; }

	public string IdModeloFactura { get; set; }

	public string DescModFactura { get; set; }

	public string OrganizacionOri { get; set; }

	public string SubinventariOri { get; set; }

	public string LocalizadorOri { get; set; }

	public string OrganizacionDes { get; set; }

	public string SubinventariDes { get; set; }

	public string LocalizadorDes { get; set; }

	public string DireccionDest { get; set; }

	public string Cpdestino { get; set; }

	public string LocalidadDest { get; set; }

	public string ProvinciaDest { get; set; }

	public string IdPaisDest { get; set; }

	public string GeoLatitud { get; set; }

	public string GeoLongitud { get; set; }

	public string ContactoDescrip { get; set; }

	public string ContactoTelefon { get; set; }

	public string ContactoFax { get; set; }

	public string ContactoEmail { get; set; }

	public string Observaciones { get; set; }

	public int? CantItems { get; set; }

	public string Archivo { get; set; }

	public virtual ICollection<DtvDevolProd> DtvDevolProds { get; set; }

	public DtvDevolPedid()
	{
		DtvDevolProds = new HashSet<DtvDevolProd>();
	}
}
