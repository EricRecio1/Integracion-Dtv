using System;
using System.Collections.Generic;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvDevolFalla
    {
        public string Clave { get; set; }
        public DateTime? FechaSys { get; set; }
        public DateTime? FechaVcia { get; set; }
        public string Usuario { get; set; }
        public string DescCorta { get; set; }
        public string DescLarga { get; set; }
        public string Estado { get; set; }
        public string Falla { get; set; }
        public long IdDevolFalla { get; set; }
        public long IdDevolSerie { get; set; }

        public virtual DtvDevolSerie IdDevolSerieNavigation { get; set; }
    }
}
