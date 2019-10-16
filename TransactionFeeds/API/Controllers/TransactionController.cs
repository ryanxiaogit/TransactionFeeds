using Abstracts;
using Abstracts.FileReader;
using Abstracts.ModelBase;
using API.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers
{

    [ApiController]
    //[ServiceFilter(typeof(RequestValidation))]
    public class TransactionController : ControllerBase
    {
        //private ITransactionAggregator _transactionAggregator;
        private ILogger<TransactionController> _logger;
        private IOptions<ServiceSetting> _optionsServiceSetting;
        private IFileStaging _fileStaging;
        private readonly IRepository _repository;
        IFileReaderChain _fileReaderChain;




        public TransactionController(
            //ITransactionAggregator transactionAggregator,
            ILogger<TransactionController> logger,
            IOptions<ServiceSetting> optionsServiceSetting,
            IFileStaging fileStaging,
            IRepository repository,
            IFileReaderChain fileReaderChain
            )
        {
            //_transactionAggregator = transactionAggregator;
            _logger = logger;
            _optionsServiceSetting = optionsServiceSetting;
            _fileStaging = fileStaging;
            _repository = repository;
            _fileReaderChain = fileReaderChain;
        }

        //transactions
        [Route("transactions")]
        [HttpPost]
        public async Task<IActionResult> UploadFeed()
        {
            try
            {
                var requestID = Guid.NewGuid().ToString();
                Stream fileStream = HttpContext?.Request.Body;

                var targetFilePath = Path.Combine(AppContext.BaseDirectory,
                                                _optionsServiceSetting.Value.FileUploadPath,
                                                $"{DateTime.Now.Ticks}.txt");
                Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, _optionsServiceSetting.Value.FileUploadPath));

                if (await _fileStaging.StagingFile(requestID, fileStream, targetFilePath))
                {
                    var transactions = _fileReaderChain.ReadFile(new StreamReader(targetFilePath));
                    await _repository.Save(transactions);

                    if (transactions == null)
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, ex.Message);
                return StatusCode(500);
            }

            return Ok();
        }


        //transactions/currency/usd      
        [Route("transactions/currency/{currency}")]
        public async Task<IActionResult> SearchTransations(
            string currency)
        {
            IList<QueryResponse> list = null;
            try
            {
                list = await SearchTransactions(new QueryRequest { Currency = currency });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, ex.Message);
                return StatusCode(500);
            }
            return Ok(list);
        }

        //transactions/timerange/20190101150310_20190102010000     
        [Route("transactions/timerange/{startDate}_{endDate}")]
        public async Task<IActionResult> SearchTransations(
            long startdate,
            long enddate)
        {
            IList<QueryResponse> list = null;
            try
            {
                list = await SearchTransactions(new QueryRequest
                {
                    StartDate = DateTime.ParseExact(startdate.ToString(),
                                                    "yyyyMMddhhmmss",
                                                     CultureInfo.InvariantCulture),
                    EndDate = DateTime.ParseExact(enddate.ToString(),
                                                    "yyyyMMddhhmmss",
                                                     CultureInfo.InvariantCulture),
                });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, ex.Message);
                return StatusCode(500);
            }
            return Ok(list);
        }

        //transactions/status/A        
        [Route("transactions/status/{status}")]
        public async Task<IActionResult> SearchTransations(
            TransactionStatus status)
        {
            IList<QueryResponse> list = null;
            try
            {
                list = await SearchTransactions(new QueryRequest
                {
                    Status = status
                });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, ex.Message);
                return StatusCode(500);
            }
            return Ok(list);
        }


        private async Task<IList<QueryResponse>> SearchTransactions(QueryRequest query)
        {
            var list = await _repository.GetTransactions(query);
            return list;
        }
    }
}