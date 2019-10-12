using System;

namespace Abstracts.ModelBase
{
    public abstract class TransactionModelBase
    {
        public string TransactionIdentificator { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
