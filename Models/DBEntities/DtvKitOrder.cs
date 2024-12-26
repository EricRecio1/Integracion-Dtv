﻿using System;
using System.Collections.Generic;

#nullable disable

namespace IntegracionOcasaDtv.Models.DBEntities
{
    public partial class DtvKitOrder
    {
        public DtvKitOrder()
        {
            DtvKitProducts = new HashSet<DtvKitProduct>();
        }

        public string Clave { get; set; }
        public DateTime? FechaSys { get; set; }
        public DateTime? FechaVcia { get; set; }
        public string Usuario { get; set; }
        public string DescCorta { get; set; }
        public string DescLarga { get; set; }
        public string Estado { get; set; }
        public long IdMensaje { get; set; }
        public string FechaMensaje { get; set; }
        public string IdIntegracion { get; set; }
        public string IntegProceso { get; set; }
        public string IntegOperacion { get; set; }
        public string IdPais { get; set; }
        public string TipoDocumento { get; set; }
        public string IdDocumento { get; set; }
        public string FecDocumento { get; set; }
        public string OrganizacionDes { get; set; }
        public string SubinventariDes { get; set; }
        public string LocalizadorDes { get; set; }
        public string DireccionDest { get; set; }
        public string Observaciones { get; set; }
        public string Archivo { get; set; }
        public int? CantItems { get; set; }

        public virtual ICollection<DtvKitProduct> DtvKitProducts { get; set; }
    }
}