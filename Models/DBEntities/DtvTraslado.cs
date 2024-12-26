using System;
using System.Collections.Generic;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvTraslado
    {
        public DtvTraslado()
        {
            DtvTraslaObservs = new HashSet<DtvTraslaObserv>();
            DtvTraslaProds = new HashSet<DtvTraslaProd>();
        }

        public string Clave { get; set; }
        public DateTime? FechaSys { get; set; }
        public DateTime? FechaVcia { get; set; }
        public string Usuario { get; set; }
        public string DescCorta { get; set; }
        public string DescLarga { get; set; }
        public string Estado { get; set; }
        public string FechaMensaje { get; set; }
        public string IdIntegracion { get; set; }
        public string IntegProceso { get; set; }
        public string IntegOperacion { get; set; }
        public string IdPais { get; set; }
        public string TipoDocumento { get; set; }
        public string IdDocumento { get; set; }
        public string FecDocumento { get; set; }
        public string OrganizacionOri { get; set; }
        public string SubinventariOri { get; set; }
        public string LocalizadorOri { get; set; }
        public string OrganizacionDes { get; set; }
        public string SubinventariDes { get; set; }
        public string LocalizadorDes { get; set; }
        public int? CantItems { get; set; }
        public long IdMensaje { get; set; }
        public bool Processed { get; set; }
        public string Archivo { get; set; }

        public virtual ICollection<DtvTraslaObserv> DtvTraslaObservs { get; set; }
        public virtual ICollection<DtvTraslaProd> DtvTraslaProds { get; set; }
    }
}
