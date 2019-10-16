using Abstracts;
using Abstracts.ModelBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Transaction
{
    public class TransactionAggregator : ITransactionAggregator
    {

        public async Task<bool> SaveTransactions(IList<TransactionModel> transactions)
        {
            throw new NotImplementedException();
        }

        public Task<IList<TransactionModel>> SearchTransaction(TransactionModel model)
        {
            throw new NotImplementedException();
        }
    }
}
