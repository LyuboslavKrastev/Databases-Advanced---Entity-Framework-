using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductsShop.App.Files
{
    [XmlType("user")]
    public class SoldProductsDto
    {
        [XmlAttribute("first-name")]
        public string firstname { get; set; }

        [XmlAttribute("last-name")]
        public string lastname { get; set; }

        [XmlArray("sold-products")]
        public List<ProductSoldDto> soldproducts { get; set; }
    }
}
