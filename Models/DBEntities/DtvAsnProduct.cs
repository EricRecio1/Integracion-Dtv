using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvAsnProduct
    {
        public DtvAsnProduct()
        {
            DtvAsnSeries = new HashSet<DtvAsnSeries>();
        }
        [Key]
        public string Clave { get; set; }
        public DateTime? FechaSys { get; set; }
        public DateTime? FechaVcia { get; set; }
        public string Usuario { get; set; }
        public string DescCorta { get; set; }
        public string DescLarga { get; set; }
        public string Estado { get; set; }
        public string IdPallet { get; set; }
        public string IdProducto { get; set; }
        public string IdProductPaired { get; set; }
        public int? CantProducto { get; set; }
        public long IdMensaje { get; set; }
        public long IdAsnProduct { get; set; }

        public virtual DtvAsn IdMensajeNavigation { get; set; }
        public virtual ICollection<DtvAsnSeries> DtvAsnSeries { get; set; }
    }
}
