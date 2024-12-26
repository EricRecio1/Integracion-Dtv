namespace IntegracionOcasaDtv.Models.Entities
{
    public class AsnTheoretical
    {
        public string FileName { get; set; }
        public Message Message { get; set; }
        public Integration Integration { get; set; }
        public string Country { get; set; }
        public string PurchaseOrder { get; set; }
        public string PurchaseOrderDateTime { get; set; }
        public int TransactionId { get; set; }
        public int LinesQty { get; set; }
        public Line[] Lines { get; set; }
    }

    public class Line
    {
        public string PalletId { get; set; }
        public string Product { get; set; }
        public string PairedProduct { get; set; }
        public int Quantity { get; set; }
        public Serials[] Serials { get; set; }
    }

    public class Serials
    {
        public string Serial { get; set; }
        public string PairedSerial { get; set; }
        public string McSerial { get; set; }
        public bool Relabel { get; set; }
    }
}
