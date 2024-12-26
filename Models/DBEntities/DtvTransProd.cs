using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvTransProd
    {
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
        public string OrganizacionOri { get; set; }
        public string SubinventariOri { get; set; }
        public string LocalizadorOri { get; set; }
        [Key]
        public long IdTransProd { get; set; }
        public virtual DtvTransOrder IdMensajeNavigation { get; set; }
    }
}
