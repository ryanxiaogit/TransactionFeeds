using Abstracts;
using Abstracts.ModelBase;
using API.Filters;
using API.Options;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
        private IMapper _mapper;
        private IOptions<ServiceSetting> _optionsServiceSetting;
        private IFileStaging _fileStaging;
        //private IFileReader _fileTypeValidation;

        public TransactionController(
            ITransactionAggregator transactionAggregator,
            ILogger<TransactionController> logger,
            IMapper mapper,
            IOptions<ServiceSetting> optionsServiceSetting,
            IFileStaging fileStaging
            //IFileReader fileTypeValidation
            )
        {
            _transactionAggregator = transactionAggregator;
            _logger = logger;
            _mapper = mapper;
            _optionsServiceSetting = optionsServiceSetting;
            _fileStaging = fileStaging;
            //_fileTypeValidation = fileTypeValidation;
        }

        public void LoadDependency(
            ITransactionAggregator transactionAggregator = null,
            ILogger<TransactionController> logger = null,
            IMapper mapper = null,
            IOptions<ServiceSetting> optionsServiceSetting = null,
            IFileStaging fileStaging = null)
        {
            _transactionAggregator = transactionAggregator ?? _transactionAggregator;
            _logger = logger ?? _logger;
            _mapper = mapper ?? _mapper;
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

            var extensionName = string.Empty;

            List<TransactionModel> transactions = null;
            //if (_fileTypeValidation.ReadFile(new StreamReader(fileStream), out var transactions))
            //    extensionName = "csv";
            //else if (_fileTypeValidation.ValidateXmlFile(new StreamReader(fileStream), out transactions))
            //    extensionName = "xml";

            if (transactions != null
                && !string.IsNullOrEmpty(extensionName))
            {
                var targetFilePath = Path.Combine(AppContext.BaseDirectory,
                    _optionsServiceSetting.Value.FileUploadPath,
                    $"{DateTime.Now.ToString()}.{extensionName}");

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
            await Task.Yield();

            return Ok(new List<QueryResponse>());
        }

        //transactions/timerange/20190101150310_20190102010000     
        [Route("transactions/timerange/{startDate}_{endDate}")]
        public async Task<IActionResult> SearchTransations(
            int startdate,
            int enddate)
        {
            await Task.Yield();

            return Ok(new List<QueryResponse>());
        }

        //transactions/status/A        
        [Route("transactions/status/{status}")]
        public async Task<IActionResult> SearchTransations(
            TransactionStatus status)
        {
            await Task.Yield();

            return Ok(new List<QueryResponse>());
        }


        private async Task<IList<QueryResponse>> SearchTransactions(QueryRequest query)
        {
            await Task.Yield();

            return new List<QueryResponse>();
        }
    }
}