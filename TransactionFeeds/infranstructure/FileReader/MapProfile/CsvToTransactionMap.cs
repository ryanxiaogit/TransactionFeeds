using Abstracts.ModelBase;
using CsvHelper.Configuration;

namespace infranstructure.FileReader.MapProfile
{
    public class CsvToTransactionMap : ClassMap<TransactionModel>
    {
        public CsvToTransactionMap()
        {
            Map(t => t.Amount).TypeConverterOption.NumberStyles(
                System.Globalization.NumberStyles.AllowDecimalPoint
                | System.Globalization.NumberStyles.AllowThousands
                | System.Globalization.NumberStyles.Float);
            Map(t => t.Amount).ConvertUsing(r => r.Amount.ToString().Replace(",", string.Empty) + "D")
                .Validate(x => x.Length > 0);
            Map(t => t.CurrencyCode).Name("Currency Code").Validate(x => x.Length > 0);
            Map(t => t.TransactionDate).Name("Transaction Date").Validate(x => x.Length > 0);
            Map(t => t.Status).Validate(x => x.Length > 0);
            Map(t => t.TransactionIdentificator).Name("Transaction Identificator").Validate(x => x.Length > 0);
        }
    }
}
