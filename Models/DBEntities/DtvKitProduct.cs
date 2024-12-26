using System;
using System.Collections.Generic;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvKitProduct
    {
        public DtvKitProduct()
        {
            DtvKitSustis = new HashSet<DtvKitSusti>();
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
        public string TipoProducto { get; set; }
        public string OrganizacionOri { get; set; }
        public string SubinventariOri { get; set; }
        public string LocalizadorOri { get; set; }
        public long IdKitProduct { get; set; }

        public virtual DtvKitOrder IdMensajeNavigation { get; set; }
        public virtual ICollection<DtvKitSusti> DtvKitSustis { get; set; }
    }
}
