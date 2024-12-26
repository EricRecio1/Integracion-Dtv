using System;
using System.Collections.Generic;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvRecepProdu
    {
        public DtvRecepProdu()
        {
            DtvRecepSeries = new HashSet<DtvRecepSerie>();
        }

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
        public long IdRecepProdu { get; set; }
        public long IdMensaje { get; set; }

        public virtual DtvRecepSucur IdMensajeNavigation { get; set; }
        public virtual ICollection<DtvRecepSerie> DtvRecepSeries { get; set; }
    }
}
