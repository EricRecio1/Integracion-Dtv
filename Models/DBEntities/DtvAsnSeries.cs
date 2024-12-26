using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvAsnSeries
    {
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
        public string NroSerie { get; set; }
        public string NroSeriePaired { get; set; }
        public string NroMacAddress { get; set; }
        public long IdAsnSerie { get; set; }
        public long IdAsnProduct { get; set; }
        public bool? Reetiquetar { get; set; }
        public long IdMensaje { get; set; }
        public virtual DtvAsnProduct IdAsnProductNavigation { get; set; }
    }
}
