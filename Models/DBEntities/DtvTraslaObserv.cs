using System;
using System.Collections.Generic;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvTraslaObserv
    {
        public string Clave { get; set; }
        public DateTime? FechaSys { get; set; }
        public DateTime? FechaVcia { get; set; }
        public string Usuario { get; set; }
        public string DescCorta { get; set; }
        public string DescLarga { get; set; }
        public string Estado { get; set; }
        public string Observacion { get; set; }
        public long IdTraslaObserv { get; set; }
        public long IdMensaje { get; set; }

        public virtual DtvTraslado IdMensajeNavigation { get; set; }
    }
}
