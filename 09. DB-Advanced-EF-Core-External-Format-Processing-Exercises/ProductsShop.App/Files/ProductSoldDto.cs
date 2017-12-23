using System.Xml.Serialization;

namespace ProductsShop.App.Files
{
    [XmlType("product")]
    public class ProductSoldDto
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}