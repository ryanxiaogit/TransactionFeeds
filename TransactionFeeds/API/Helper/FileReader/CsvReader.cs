using Abstracts.ModelBase;
using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace API.Helper.FileReader
{
    public class CsvReader : FileReaderBase
    {
        readonly ILogger<CsvReader> _logger;
        public CsvReader(ILogger<CsvReader> logger)
        {
            _logger = logger;
        }

        protected override object ImplementHandle(object request)
        {
            List<TransactionModel> transactions = null;

            var fileReader = request as StreamReader;

            try
            {
                var line = fileReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    if (transactions == null)
                    {
                        transactions = new List<TransactionModel>();
                    }
                    transactions.Add(GetModel(line));

                    line = fileReader.ReadLine();
                }
            }
            catch (CsvHelperException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
            }
            return transactions;
        }

        private TransactionModel GetModel(string line)
        {
            TransactionModel model = null;
            try
            {
                var stringValues = line.Split("\"").Where(x => x.Trim().Length > 0 && x.Trim() != ",").ToList();
                if (stringValues.Count == 5)
                {
                    model = new TransactionModel
                    {
                        TransactionIdentificator = stringValues[0],
                        Amount = Decimal.Parse(stringValues[1],
                                                    NumberStyles.AllowThousands
                                                    | NumberStyles.AllowDecimalPoint),
                        CurrencyCode = stringValues[2],
                        TransactionDate = DateTime.ParseExact(stringValues[3], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                        Status = stringValues[4] == "Approved" ? TransactionStatus.A
                        : stringValues[4] == "Failed" ? TransactionStatus.R : TransactionStatus.D
                        //TODO: optimize this cast method
                    };
                }
            }
            catch (Exception ex)
            {
                model = null;
                _logger.Log(LogLevel.Error, $"Transform CSV error for the line {line}  =============>Message {ex.Message}");
            }

            return model;

        }
    }
}
