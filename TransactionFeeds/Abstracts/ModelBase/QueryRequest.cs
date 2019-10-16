using System;

namespace Abstracts.ModelBase
{
    public class QueryRequest
    {        
        public string Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
