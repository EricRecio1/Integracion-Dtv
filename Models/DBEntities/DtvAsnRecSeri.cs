using System;
using System.Collections.Generic;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvAsnRecSeri
    {
        public string Clave { get; set; }
        public DateTime? FechaSys { get; set; }
        public DateTime? FechaVcia { get; set; }
        public string Usuario { get; set; }
        public string DescCorta { get; set; }
        public string DescLarga { get; set; }
        public string Estado { get; set; }
        public string IdProducto { get; set; }
        public string NroSerie { get; set; }
        public string IdProductPaired { get; set; }
        public string NroSeriePaired { get; set; }
        public long IdAsnRecSeri { get; set; }
        public long IdAsnRecProd { get; set; }

        public virtual DtvAsnRecProd IdAsnRecProdNavigation { get; set; }
    }
}
