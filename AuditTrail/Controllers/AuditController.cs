using AuditTrail.Models;
using AuditTrail.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditTrail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpPost("audit")]
        public IActionResult AuditChanges([FromBody] AuditRequest request)
        {
            var AuditTrailLog = _auditService.CreateAuditEntry(request.Before, request.After, request.Action, request.UserId);
            return Ok(AuditTrailLog);
        }
    }


}
