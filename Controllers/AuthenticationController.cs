using System.Security.Authentication;
using System.Threading.Tasks;
using Clients.DataTransfer;
using Clients.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Clients.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthService _authService;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        /// <summary>
        /// Аутентифицирует жителя Новороссийска в системе по email и паролю
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Пароль</param>
        [HttpPost("Citizen")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FullClientDto>> AuthenticateCitizenAsync(string email, string password)
        {
            try
            {
                return Ok(await _authService.AuthenticateCitizen(email, password));
            }
            catch (AuthenticationException)
            {
                return BadRequest();
            }
        }
        
        /// <summary>
        /// Аутентифицирует гостя Новороссийска в системе по номеру телефона и паролю
        /// </summary>
        /// <param name="phoneNumber">Номер телефона</param>
        /// <param name="password">Пароль</param>
        [HttpPost("Guest")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FullClientDto>> AuthenticateGuestAsync(string phoneNumber, string password)
        {
            try
            {
                return Ok(await _authService.AuthenticateGuest(phoneNumber, password));
            }
            catch (AuthenticationException)
            {
                return BadRequest();
            }
        }
    }
}