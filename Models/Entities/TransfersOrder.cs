using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntegracionOcasaDtv.Models.Entities
{
    [XmlRoot(ElementName = "Order")]
    public class TransfersOrder
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

        [XmlElement(ElementName = "DeclaredCostValue")]
        public float DeclaredCostValue { get; set; }

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
        public List<OrderLine> OrderLine { get; set; }

    }

    [XmlRoot(ElementName = "BillingModel")]
    public class BillingModel
    {
        [XmlElement(ElementName = "Code")]
        public string Code { get; set; }

        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }
    }

    [XmlRoot(ElementName = "Address")]
    public class Address
    {
        [XmlElement(ElementName = "Address")]
        public string _Address { get; set; }

        [XmlElement(ElementName = "PostalCode")]
        public string PostalCode { get; set; }

        [XmlElement(ElementName = "CitySubdivisionName")]
        public string CitySubdivisionName { get; set; }

        [XmlElement(ElementName = "CountrySubentity")]
        public string CountrySubentity { get; set; }

        [XmlElement(ElementName = "Country")]
        public string Country { get; set; }

        [XmlElement(ElementName = "Geolocation")]
        public Geolocation Geolocation { get; set; }

    }

    [XmlRoot(ElementName = "Geolocation")]
    public class Geolocation
    {
        [XmlElement(ElementName = "Latitude")]
        public string Latitude { get; set; }

        [XmlElement(ElementName = "Longitude")]
        public string Longitude { get; set; }

    }

    [XmlRoot(ElementName = "Contact")]
    public class Contact
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Telephone")]
        public string Telephone { get; set; }

        [XmlElement(ElementName = "Fax")]
        public string Fax { get; set; }

        [XmlElement(ElementName = "Email")]
        public string Email { get; set; }

    }

    [XmlRoot(ElementName = "OrderLine")]
    public class OrderLine
    {
        [XmlElement(ElementName = "ItemLinesQuantity")]
        public int ItemLinesQuantity { get; set; }

        [XmlElement(ElementName = "OriginParty")]
        public OriginParty OriginParty { get; set; }

        [XmlElement(ElementName = "ItemLine")]
        public List<ItemLine> ItemLine { get; set; }
        
    }

    [XmlRoot(ElementName = "OriginParty")]

    public class OriginParty
    {
        [XmlElement(ElementName = "Organization")]
        public string Organization { get; set; }

        [XmlElement(ElementName = "SubInventory")]
        public string SubInventory { get; set; }

        [XmlElement(ElementName = "Tracker")]
        public string Tracker { get; set; }

    }

    [XmlRoot(ElementName = "ItemLine")]

    public class ItemLine
    {
        [XmlElement(ElementName = "Quantity")]
        public int Quantity { get; set; }

        [XmlElement(ElementName = "Item")]
        public Item Item { get; set; }

    }

    [XmlRoot(ElementName = "Item")]
    public class Item
    {
        [XmlElement(ElementName = "Product")]
        public ProductTraferOrder Product { get; set; }

    }

    [XmlRoot(ElementName = "Product")]
    public class ProductTraferOrder
    {
        [XmlElement(ElementName = "ID")]
        public string ID { get; set; }
    }
}
