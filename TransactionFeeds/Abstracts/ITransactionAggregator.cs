using Abstracts.ModelBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstracts
{
    public interface ITransactionAggregator
    {
        Task<bool> SaveTransactions(IList<TransactionModel> transactions);
        Task<IList<TransactionModel>> SearchTransaction(TransactionModel model);
    }
}
