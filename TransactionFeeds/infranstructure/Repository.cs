using Abstracts;
using Abstracts.ModelBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace infranstructure
{
    public class Repository : IRepository
    {
        public Task<List<TransactionModel>> GetTransactions(TransactionModel targetModel)
        {
            throw new NotImplementedException();
        }

        public Task Save(List<TransactionModel> transactions)
        {
            throw new NotImplementedException();
        }
    }
}
