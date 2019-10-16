using System.Collections.Generic;
using System.Xml.Serialization;

namespace infranstructure.FileReader.Dto
{
    [XmlRoot("Transactions")]
    public class XmlTransactions
    {
        [XmlElement("Transaction")]
        public List<XmlTransaction> Trnasactions { get; set; }
    }
}
