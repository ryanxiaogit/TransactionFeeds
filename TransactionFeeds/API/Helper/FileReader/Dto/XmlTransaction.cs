using System;
using System.Xml.Serialization;

namespace API.Helper.FileReader.Dto
{
    [XmlRoot("Transaction")]
    public class XmlTransaction
    {
        [XmlAttribute("id")]
        public string TransactionID { get; set; }
        [XmlElement("TransactionDate")]
        public string TransactionDate { get; set; }
        [XmlElement("PaymentDetails")]
        public PaymentDetails Details { get; set; }
        [XmlElement("Status")]
        public string Status { get; set; }
        public class PaymentDetails
        {
            [XmlElement("Amount")]
            public string Amount { get; set; }
            [XmlElement("CurrencyCode")]
            public string CurrencyCode { get; set; }
        }
    }
}
