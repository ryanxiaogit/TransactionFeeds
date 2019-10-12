using API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(RequestValidation))]
    public class TransactionController : ControllerBase
    {

    }
}