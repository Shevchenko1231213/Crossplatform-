using Lab6.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab6.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/customers")]
    [ApiVersion("2.0")]
    public class CoreV2Controller : ControllerBase
    {
        private readonly CoreService _service;

        public CoreV2Controller(CoreService service)
            => _service = service;

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthRequest request, CancellationToken cancellationToken)
            => Ok(await _service.LoginAsync(request));
    }
}
