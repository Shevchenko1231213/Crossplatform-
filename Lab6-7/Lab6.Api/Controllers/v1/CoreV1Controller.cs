using Lab6.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab6.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/customers")]
    [ApiVersion("1.0")]
    public class CoreV1Controller : ControllerBase
    {
        private readonly CoreService _service;

        public CoreV1Controller(CoreService service)
            => _service = service;

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthRequest request)
            => Ok(await _service.LoginAsync(request));

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
            => Ok(await _service.RegisterAsync(request));

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCustomersByFiltersAsync([FromQuery] GetCustomersRequest request)
            => Ok(await _service.GetCustomersByFiltersAsync(request));

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetCustomersWithOrdersListAsync()
            => Ok(await _service.GetListAsync());
    }
}
