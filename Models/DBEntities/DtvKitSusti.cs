using System;
using System.Collections.Generic;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvKitSusti
    {
        public string Clave { get; set; }
        public DateTime? FechaSys { get; set; }
        public DateTime? FechaVcia { get; set; }
        public string Usuario { get; set; }
        public string DescCorta { get; set; }
        public string DescLarga { get; set; }
        public string Estado { get; set; }
        public long? IdMensaje { get; set; }
        public string IdSustituto { get; set; }
        public long IdKitSusti { get; set; }
        public long IdKitProduct { get; set; }

        public virtual DtvKitProduct IdKitProductNavigation { get; set; }
    }
}
