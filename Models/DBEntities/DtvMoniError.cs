// IntegracionOcasaDtv, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// IntegracionOcasaDtv.Models.DBEntities.DtvMoniError
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("DtvMoniError")]
public class DtvMoniError
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Clave { get; set; }
	public DateTime? FechaSys { get; set; }
	public DateTime? FechaVcia { get; set; }
	public string Usuario { get; set; }
	public string DescCorta { get; set; }
	public string DescLarga { get; set; }
	public string Estado { get; set; }
	public decimal IdDocumento { get; set; }
	public string FechaError { get; set; }
	public string NombreArchivo { get; set; }
	public decimal IdIntegracion { get; set; }
	public string IdPais { get; set; }
	public decimal IdEstado { get; set; }
	public decimal IdResponsable { get; set; }
	public string FechaCierre { get; set; }
	public string Error { get; set; }
	public string Observacion { get; set; }
}
