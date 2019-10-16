using Abstracts;
using Abstracts.ModelBase;
using API.Filters;
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
    [ServiceFilter(typeof(RequestValidation))]
    public class TransactionController : ControllerBase
    {
        private ITransactionAggregator _transactionAggregator;
        private ILogger<TransactionController> _logger;
        private IOptions<ServiceSetting> _optionsServiceSetting;
        private IFileStaging _fileStaging;
        private readonly IRepository _repository;


        public TransactionController(
            ITransactionAggregator transactionAggregator,
            ILogger<TransactionController> logger,
            IOptions<ServiceSetting> optionsServiceSetting,
            IFileStaging fileStaging,
            IRepository repository
            )
        {
            _transactionAggregator = transactionAggregator;
            _logger = logger;
            _optionsServiceSetting = optionsServiceSetting;
            _fileStaging = fileStaging;
            _repository = repository;
        }

        public void LoadDependency(
            ITransactionAggregator transactionAggregator = null,
            ILogger<TransactionController> logger = null,
            IOptions<ServiceSetting> optionsServiceSetting = null,
            IFileStaging fileStaging = null)
        {
            _transactionAggregator = transactionAggregator ?? _transactionAggregator;
            _logger = logger ?? _logger;
            _optionsServiceSetting = optionsServiceSetting ?? _optionsServiceSetting;
            _fileStaging = fileStaging ?? _fileStaging;
        }

        //transactions
        [Route("transactions")]
        [HttpPost]
        public async Task<IActionResult> UploadFeed()
        {
            var processSuccessfully = true;

            var requestID = Guid.NewGuid().ToString();
            var fileStream = HttpContext?.Request.Body;
            await Task.Yield();

            //var readerChain =

            List < TransactionModel > transactions = null;

            if (transactions != null)
            {
                var targetFilePath = Path.Combine(AppContext.BaseDirectory,
                    _optionsServiceSetting.Value.FileUploadPath,
                    $"{DateTime.Now.ToString()}.txt");

                if (await _fileStaging.StagingFile(requestID, fileStream, targetFilePath))
                {
                    processSuccessfully &= await _transactionAggregator.SaveTransactions(transactions);
                }
            }

            if (processSuccessfully)
                return Ok();
            else
                return BadRequest();
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