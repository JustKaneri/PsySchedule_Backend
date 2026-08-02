using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;
using PsySchedule.Validations;

namespace PsySchedule.Controllers
{
    /// <summary>
    /// Контроллер авторизациии
    /// </summary>
    [ApiController]
    [Route("api/v1/auth/")]
    public class AuthenticationController : Controller
    {
        private readonly IRegistrationService _registerService;
        private readonly IValidator<RegisterPsychologistDto> _validatorRegistry;
        private readonly IValidator<AuthenticationDto> _validatorAuth;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IRegistrationService registerService, IValidator<RegisterPsychologistDto> validatorRegistry, 
                                        IValidator<AuthenticationDto> validatorAuth, IAuthenticationService authenticationService)
        {
            _registerService = registerService;
            _validatorRegistry = validatorRegistry;
            _validatorAuth = validatorAuth;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <returns></returns>
        [HttpPost("authentication")]
        [ProducesResponseType(typeof(AuthTokensDto),200)]
        public async Task<IActionResult> Authentication([FromBody]AuthenticationDto authenticationData, CancellationToken cancellationToken)
        {
            var validationResult = _validatorAuth.Validate(authenticationData);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var userData = new UserDataDto(Request.HttpContext.Connection.RemoteIpAddress.ToString(), Request.Headers["User-Agent"].ToString());

            var tokens = await _authenticationService.AuthenticateAsync(authenticationData, userData, cancellationToken);

            if (tokens.IsSuccess)
                return Ok(new AccessTokenDto(tokens.Value.AccessToken));

            return BadRequest(tokens.Error.errorMessage);
        }

        /// <summary>
        /// Регистрация психолога
        /// </summary>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AccessTokenDto), 201)]
        public async Task<IActionResult> Register([FromBody]RegisterPsychologistDto registerData, CancellationToken cancellationToken)
        {

            var validationResult = _validatorRegistry.Validate(registerData);

            if(!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var userData = new UserDataDto(Request.HttpContext.Connection.RemoteIpAddress.ToString(), Request.Headers["User-Agent"].ToString());

            var result = await _registerService.RegisterPsychologistAsync(registerData, userData, cancellationToken);

            if (result.IsSuccess)
                return Created(nameof(AccessTokenDto), new AccessTokenDto(result.Value.AccessToken));

            return BadRequest(result.Error.errorMessage);
        }

        /// <summary>
        /// Обновление Refresh и Accses токенов
        /// </summary>
        /// <returns></returns>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AuthTokensDto), 200)]
        public IActionResult Refresh(CancellationToken cancellationToken)
        {
            return Ok();
        }

        /// <summary>
        /// Отзыв токена
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            Request.Cookies.TryGetValue("rftkn", out string refreshToken);

            if (string.IsNullOrWhiteSpace(refreshToken))
                return Unauthorized("Токен не может быть пустым");

            var result = await _authenticationService.LogoutAsync(refreshToken, cancellationToken);

            if (result.IsSuccess)
                return Ok();

            return BadRequest(result.Error.errorMessage);
        }
    }
}
