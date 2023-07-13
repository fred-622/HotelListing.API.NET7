using HotelListing.API.Contracts;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;

        //from vid 62 inject logger
        public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
        {
            this._authManager = authManager;
            this._logger = logger;
        }

        // POST: api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
        {
            // vid 62
            _logger.LogInformation($"Registratio Attempt for : {apiUserDto.Email}");

            var errors = await _authManager.Register(apiUserDto);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok(); // 200
        }

        // POST: api/Account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            // vid 62
            _logger.LogInformation($"Registratio Attempt for : {loginDto.Email}");

            var authResponse = await _authManager.Login(loginDto);

            if (authResponse == null)
            {
                return Unauthorized(); //401 did not pass authentication
            }
            return Ok(authResponse); // 200
        }

        // add vid 58
        // POST: api/Account/refreshtoken
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
        {
            // vid 62
            _logger.LogInformation($"RefreshToken Attempt for : {request.UserId}");

            var authResponse = await _authManager.VerifyRefreshToken(request);

            if (authResponse == null)
            {
                return Unauthorized(); //401 did not pass authentication
            }
            return Ok(authResponse); // 200
        }
    }
}
