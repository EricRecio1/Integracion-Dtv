using IntegracionOcasaDtv.Models.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntegracionOcasaDtv.Models.Entities
{
    [XmlRoot(ElementName = "Order")]
    public class ReturnsOrder
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

        [XmlElement(ElementName = "EstimatedDateTime")]
        public string EstimatedDateTime { get; set; }

        [XmlElement(ElementName = "BillingModel")]
        public BillingModel BillingModel { get; set; }

        [XmlElement(ElementName = "SenderParty")]
        public SenderParty SenderParty { get; set; }
        
        [XmlElement(ElementName = "ReceiverParty")]
        public ReceiverParty ReceiverParty { get; set; }

        [XmlElement(ElementName = "Address")]
        public Address Address { get; set; }

        [XmlElement(ElementName = "Contact")]
        public Contact Contact { get; set; }

        [XmlElement(ElementName = "OrderNote")]
        public string OrderNote { get; set; }

        [XmlElement(ElementName = "OrderLinesQuantity")]
        public int OrderLinesQuantity { get; set; }

        [XmlElement(ElementName = "OrderLine")]
        public List<OrderLineReturnOrder> OrderLine { get; set; }

    }

    [XmlRoot(ElementName = "SenderParty")]
    public class SenderParty
    {
        [XmlElement(ElementName = "Organization")]
        public string Organization { get; set; }

        [XmlElement(ElementName = "SubInventory")]
        public string SubInventory { get; set; }

        [XmlElement(ElementName = "Tracker")]
        public string Tracker { get; set; }
    }

    [XmlRoot(ElementName = "OrderLine")]
    public class OrderLineReturnOrder
    {
        [XmlElement(ElementName = "ItemLinesQuantity")]
        public int ItemLinesQuantity { get; set; }
             
        [XmlElement(ElementName = "ItemLine")]
        public ItemLineReturnOrder ItemLine { get; set; }      
    }
   
    [XmlRoot(ElementName = "ItemLine")]

    public class ItemLineReturnOrder
    {
        [XmlElement(ElementName = "Quantity")]
        public int Quantity { get; set; }

        [XmlElement(ElementName = "Item")]
        public ItemReturnOrder Item { get; set; }

    }

    [XmlRoot(ElementName = "Item")]
    public class ItemReturnOrder
    {
        [XmlElement(ElementName = "Product")]
        public ProductTraferOrder Product { get; set; }

        [XmlElement(ElementName = "ItemInstance")]
        public List<ItemInstance> ItemInstance { get; set; }
    }

    [XmlRoot(ElementName = "ItemInstance")]
    public class ItemInstance
    {
        [XmlElement(ElementName = "SerialNumber")]
        public string SerialNumber { get; set; }

        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "Flaw")]
        public Flaw Flaw { get; set; }
    }

    [XmlRoot(ElementName = "Flaw")]
    public class Flaw
    {
        [XmlElement(ElementName = "LastFlaw")]
        public string LastFlaw{ get; set; }

        [XmlElement(ElementName = "HistoricFlaw")]
        public List<string> HistoricFlaw { get; set; }
    }

}


