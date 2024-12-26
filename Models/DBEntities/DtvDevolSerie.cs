using System;
using System.Collections.Generic;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvDevolSerie
    {
        public DtvDevolSerie()
        {
            DtvDevolFallas = new HashSet<DtvDevolFalla>();
        }

        public string Clave { get; set; }
        public DateTime? FechaSys { get; set; }
        public DateTime? FechaVcia { get; set; }
        public string Usuario { get; set; }
        public string DescCorta { get; set; }
        public string DescLarga { get; set; }
        public string Estado { get; set; }
        public string IdProducto { get; set; }
        public string NroSerie { get; set; }
        public string Status { get; set; }
        public long IdDevolProd { get; set; }
        public long IdDevolSerie { get; set; }
        public long IdMensaje { get; set; }
        public virtual DtvDevolProd IdDevolProdNavigation { get; set; }
        public virtual ICollection<DtvDevolFalla> DtvDevolFallas { get; set; }
    }
}
