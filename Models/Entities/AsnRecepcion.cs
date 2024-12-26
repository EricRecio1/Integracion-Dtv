using System;

namespace IntegracionOcasaDtv.Models.Entities
{
    public class AsnRecepcion
    {
        public Message Message { get; set; }
        public Integration Integration { get; set; }
        public string Country { get; set; }
        public string DocumentType  { get; set; }
        public string DocumentId { get; set; }
        public int DocumentTransactionId { get; set; }
        public DateTime DocumentDateTime { get; set; }
        public string PurchaseOrder { get; set; }
        public ReceiverParty ReceiverParty { get; set; }
        public int ProductActivityLinesQuantity { get; set; }
        public string FileName { get; set; }
        public Product[] Products { get; set; }
    }
}
