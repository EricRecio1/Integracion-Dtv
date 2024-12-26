using System;
using System.Collections.Generic;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvAsnRecep
    {
        public DtvAsnRecep()
        {
            DtvAsnRecProds = new HashSet<DtvAsnRecProd>();
        }

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
        public Int64 IdTransaccion { get; set; }
        public string FecTransaccion { get; set; }
        public string NroOc { get; set; }
        public string Organizacion { get; set; }
        public string Subinventario { get; set; }
        public string Localizador { get; set; }
        public int? CantItems { get; set; }
        public bool? Processed { get; set; }
        public string Archivo { get; set; }
        public virtual ICollection<DtvAsnRecProd> DtvAsnRecProds { get; set; }
    }
}
