using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvAsn
    {
        public DtvAsn()
        {
            DtvAsnProducts = new HashSet<DtvAsnProduct>();
        }
        [Key]
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
        public string NroOc { get; set; }
        public string FechaOc { get; set; }
        public int? IdTransaccion { get; set; }
        public string Cantlineas { get; set; }
        public string Archivo { get; set; }

        public virtual ICollection<DtvAsnProduct> DtvAsnProducts { get; set; }
    }
}
