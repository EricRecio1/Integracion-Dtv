using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntegracionOcasaDtv.Models.Entities
{
    [XmlRoot(ElementName = "Order")]
    public class KittingWorkOrders
    {
        public string FileName { get; set; }
        [XmlElement(ElementName = "Message")]
        public Message Message { get; set; }

        [XmlElement(ElementName = "Integration")]
        public Integration Integration { get; set; }

        [XmlElement(ElementName = "Country")]
        public string Country { get; set; }

        [XmlElement(ElementName = "DocumentType")]
        public string DocumentType { get; set; }

        [XmlElement(ElementName = "DocumentId")]
        public string DocumentId { get; set; }

        [XmlElement(ElementName = "DocumentDateTime")]
        public string DocumentDateTime { get; set; }

        [XmlElement(ElementName = "ReceiverParty")]
        public ReceiverParty ReceiverParty { get; set; }

        [XmlElement(ElementName = "OrderNote")]
        public string OrderNote { get; set; }

        [XmlElement(ElementName = "OrderLinesQuantity")]
        public int OrderLinesQuantity { get; set; }

        [XmlElement(ElementName = "OrderLine")]
        public KittingOrderLine OrderLine { get; set; }
    }

    [XmlRoot(ElementName = "OrderLine")]
    public class KittingOrderLine
    {
        [XmlElement(ElementName = "ItemLinesQuantity")]
        public int ItemLinesQuantity { get; set; }

        [XmlElement(ElementName = "OriginParty")]
        public OriginParty OriginParty { get; set; }

        [XmlElement(ElementName = "ItemLine")]
        public List<KittingItemLine> ItemLine { get; set; }

    }

    [XmlRoot(ElementName = "ItemLine")]

    public class KittingItemLine
    {
        [XmlElement(ElementName = "Quantity")]
        public int Quantity { get; set; }

        [XmlElement(ElementName = "Item")]
        public KittingItem Item { get; set; }

    }

    [XmlRoot(ElementName = "Item")]
    public class KittingItem
    {
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }

        [XmlElement(ElementName = "Product")]
        public ProductTraferOrder Product { get; set; }

        [XmlElement(ElementName = "Substitute")]
        public List<Sustitute> Sustitute { get; set; }

    }

    [XmlRoot(ElementName = "Sustitute")]
    public class Sustitute
    {
        [XmlElement(ElementName = "ID")]
        public string ID { get; set; }
    }
}
