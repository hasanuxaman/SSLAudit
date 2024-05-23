using Microsoft.AspNetCore.Mvc;
using Shampan.Models;

namespace SSLAudit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduledController : ControllerBase
    {
        private readonly IHostedService _scheduledFunctionExecutionService;

        public ScheduledController(IHostedService scheduledFunctionExecutionService)
        {
            _scheduledFunctionExecutionService = scheduledFunctionExecutionService;
        }

        [HttpGet("triggerFunction")]
        public IActionResult TriggerFunction()
        {
            // Manually execute the function
            (_scheduledFunctionExecutionService as ScheduledFunctionExecutionService)?.ExecuteScheduledFunction();
            return Ok("Function triggered successfully.");
        }
    }
}
