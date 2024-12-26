using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvLog
    {
        [Key]
        public string Clave { get; set; }
        public DateTime? FechaSys { get; set; }
        public DateTime? FechaVcia { get; set; }
        public string Usuario { get; set; }
        public string DescCorta { get; set; }
        public string DescLarga { get; set; }
        public string Estado { get; set; }
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Evento { get; set; }
        public long? IdMensaje { get; set; }
        public string NombreArchivo { get; set; }
        public string IdIntegracion { get; set; }
        public string IntegProceso { get; set; }
        public string IntegOperacion { get; set; }
        public string TipoArchivo { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
    }
}
