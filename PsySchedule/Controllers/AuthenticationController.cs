using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
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

        public AuthenticationController(IRegistrationService registerService, IValidator<RegisterPsychologistDto> validatorRegistry)
        {
            _registerService = registerService;
            _validatorRegistry = validatorRegistry;
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <returns></returns>
        [HttpPost("authentication")]
        [ProducesResponseType(typeof(AuthTokensDto),200)]
        public IActionResult Authentication([FromBody]AuthenticationDto authenticationData, CancellationToken cancellationToken)
        {
            return Ok();
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
        /// Обнуление токенов
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public IActionResult Logout(CancellationToken cancellationToken)
        {
            return NoContent();
        }
    }
}
