using Abstracts.ModelBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstracts
{
    public interface IRepository
    {
        Task Save(List<TransactionModel> transactions);
        Task<List<QueryResponse>> GetTransactions(QueryRequest targetModel);
    }
}
