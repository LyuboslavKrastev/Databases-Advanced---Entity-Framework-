using System.Xml.Serialization;

namespace Instagraph.DataProcessor.DtoModels
{
    [XmlType("user")]
    public class UserTopPostDto
    {
       
        public string Username { get; set; }
        public int MostComments { get; set; }
    }
}
