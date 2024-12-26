using System;
using System.Collections.Generic;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvTraslaProd
    {
        public DtvTraslaProd()
        {
            DtvTraslaSeries = new HashSet<DtvTraslaSeri>();
        }

        public string Clave { get; set; }
        public DateTime? FechaSys { get; set; }
        public DateTime? FechaVcia { get; set; }
        public string Usuario { get; set; }
        public string DescCorta { get; set; }
        public string DescLarga { get; set; }
        public string Estado { get; set; }
        public long IdMensaje { get; set; }
        public string IdProducto { get; set; }
        public int? CantProducto { get; set; }
        public long IdTraslaProd { get; set; }

        public virtual DtvTraslado IdMensajeNavigation { get; set; }
        public virtual ICollection<DtvTraslaSeri> DtvTraslaSeries { get; set; }
    }
}
