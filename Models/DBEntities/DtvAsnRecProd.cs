using System;
using System.Collections.Generic;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvAsnRecProd
    {
        public DtvAsnRecProd()
        {
            DtvAsnRecSeris = new HashSet<DtvAsnRecSeri>();
        }

        public string Clave { get; set; }
        public DateTime? FechaSys { get; set; }
        public DateTime? FechaVcia { get; set; }
        public string Usuario { get; set; }
        public string DescCorta { get; set; }
        public string DescLarga { get; set; }
        public string Estado { get; set; }
        public long? IdMensaje { get; set; }
        public string IdProducto { get; set; }
        public int? CantProducto { get; set; }
        public long IdAsnRecProd { get; set; }

        public virtual DtvAsnRecep IdMensajeNavigation { get; set; }
        public virtual ICollection<DtvAsnRecSeri> DtvAsnRecSeris { get; set; }
    }
}
