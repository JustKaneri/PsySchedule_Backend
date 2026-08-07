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
        /// Аутентификация
        /// </summary>
        /// <returns></returns>
        [HttpPost("authentication")]
        [ProducesResponseType(typeof(AccessTokenDto),200)]
        [ProducesResponseType(typeof(Error),401)]
        public async Task<IActionResult> Authentication([FromBody]AuthenticationDto authenticationData, CancellationToken cancellationToken)
        {
            var validationResult = _validatorAuth.Validate(authenticationData);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var userData = new MetaDataDto(Request.HttpContext.Connection.RemoteIpAddress.ToString(), Request.Headers["User-Agent"].ToString());

            var tokens = await _authenticationService.AuthenticateAsync(authenticationData, userData, cancellationToken);

            if (tokens.IsSuccess)
                return Ok(new AccessTokenDto(tokens.Value.AccessToken));

            return Unauthorized(tokens.Error);
        }

        /// <summary>
        /// Регистрация психолога
        /// </summary>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AccessTokenDto), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> Register([FromBody]RegisterPsychologistDto registerData, CancellationToken cancellationToken)
        {

            var validationResult = _validatorRegistry.Validate(registerData);

            if(!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var userData = new MetaDataDto(Request.HttpContext.Connection.RemoteIpAddress.ToString(), Request.Headers["User-Agent"].ToString());

            var result = await _registerService.RegisterPsychologistAsync(registerData, userData, cancellationToken);

            if (result.IsSuccess)
            {
                Response.Cookies.Append("rftkn", result.Value.RefreshToken, new CookieOptions()
                {
                    HttpOnly = true, 
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMonths(1)
                });

                return Created(nameof(AccessTokenDto), new AccessTokenDto(result.Value.AccessToken));
            }


            return BadRequest(result.Error);
        }

        /// <summary>
        /// Обновление Refresh и Accses токенов
        /// </summary>
        /// <returns></returns>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AccessTokenDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 401)]
        [ProducesResponseType(typeof(Error), 409)]
        public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
        {
            Request.Cookies.TryGetValue("rftkn", out string refreshToken);

            if (string.IsNullOrWhiteSpace(refreshToken))
                return Unauthorized("Токен не может быть пустым");

            var userData = new MetaDataDto(Request.HttpContext.Connection.RemoteIpAddress.ToString(), Request.Headers["User-Agent"].ToString());

            var result = await _authenticationService.RefreshTokenAsync(refreshToken, userData, cancellationToken);

            if (result.IsSuccess)
            {
                Response.Cookies.Append("rftkn", result.Value.RefreshToken, new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMonths(1)
                });

                return Created(nameof(AccessTokenDto), new AccessTokenDto(result.Value.AccessToken));
            }


            switch (result.Error.ErrorCode)
            {
                case 400:
                    return BadRequest(result.Error);
                case 401:
                    return Unauthorized(result.Error);
                case 409:
                    return Conflict(result.Error);
                default:
                    return BadRequest();
            }
        }

        /// <summary>
        /// Отзыв токена
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        [ProducesResponseType(typeof(Error), 401)]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            Request.Cookies.TryGetValue("rftkn", out string refreshToken);

            if (string.IsNullOrWhiteSpace(refreshToken))
                return Unauthorized("Токен не может быть пустым");

            var result = await _authenticationService.LogoutAsync(refreshToken, cancellationToken);

            if (result.IsSuccess)
            {
                Response.Cookies.Delete("rftkn");
                return Ok();
            }


            return Unauthorized(result.Error);
        }
    }
}
