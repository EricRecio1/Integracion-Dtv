namespace IntegracionOcasaDtv.Models.Entities
{
    public class Product
    {
        public string Id { get; set; }
        public int Quantity { get; set; }

        public Serial[] Serials { get; set; }
    }

    public class Serial
    {
        public string SerialNumber { get; set; }
        public string PairedProduct { get; set; }
        public string PairedSerial { get; set; }
    }
}
