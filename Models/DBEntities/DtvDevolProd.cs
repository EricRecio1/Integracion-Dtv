using System;
using System.Collections.Generic;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvDevolProd
    {
        public DtvDevolProd()
        {
            DtvDevolSeries = new HashSet<DtvDevolSerie>();
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
        public long IdMensaje { get; set; }
        public long IdDevolProd { get; set; }

        public virtual DtvDevolPedid IdMensajeNavigation { get; set; }
        public virtual ICollection<DtvDevolSerie> DtvDevolSeries { get; set; }
    }
}
