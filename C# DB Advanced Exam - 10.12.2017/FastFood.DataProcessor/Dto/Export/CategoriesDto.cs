using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FastFood.DataProcessor.Dto.Export
{
    [XmlType("Category")]
    public class CategoriesDto
    {

        public string Name { get; set; }

        public MostPopularItemDto MostPopularItem { get; set; }

    }
}
