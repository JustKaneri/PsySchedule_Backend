using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;
using System.Security.Claims;

namespace PsySchedule.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ServiceController : Controller
    {
        private readonly IServiceManager _manager;
        private readonly IValidator<ServiceDto> _validator;
        private readonly IValidator<UpdateServiceRequest> _validatorUpdater;

        public ServiceController(IServiceManager manager, IValidator<ServiceDto> validator, IValidator<UpdateServiceRequest> validatorUpdater)
        {
            _manager = manager;
            _validator = validator;
            _validatorUpdater = validatorUpdater;
        }

        /// <summary>
        /// Получить список услуг
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("services")]
        [ProducesResponseType(typeof(IEnumerable<ServiceResponse>),200)]
        [ProducesResponseType(typeof(Error),400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            if (!int.TryParse(User.FindFirst("Id")?.Value, out var Id))
                return Unauthorized();

            var result = await _manager.GetServicesAsync(Id,cancellationToken);

            if(result.IsSuccess)
                return Ok(result.Value);

            return result.Error.ErrorCode switch
            {
                _ => BadRequest(result.Error)
            };
        }

        /// <summary>
        /// Создание услуги
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("service")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(Error), 404)]
        public async Task<IActionResult> Create([FromBody]CreateServiceRequest request,CancellationToken cancellationToken)
        {
            var valid = _validator.Validate(request);

            if (!valid.IsValid)
                return BadRequest(valid.Errors);


            if (!int.TryParse(User.FindFirst("Id")?.Value, out var Id))
                return Unauthorized();

            var result = await _manager.CreateAsync(Id, request, cancellationToken);

            if (result.IsSuccess)
                return Ok();

            return result.Error.ErrorCode switch
            {
                404 => NotFound(result.Error),
                _ => BadRequest(result.Error)
            };
        }

        /// <summary>
        /// Обновление услуги
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("service")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(Error), 404)]
        [ProducesResponseType(typeof(Error), 409)]
        public async Task<IActionResult> Update([FromBody]UpdateServiceRequest request,CancellationToken cancellationToken)
        {
            var valid = _validatorUpdater.Validate(request);

            if (!valid.IsValid)
                return BadRequest(valid.Errors);

            if (!int.TryParse(User.FindFirst("Id")?.Value, out var Id))
                return Unauthorized();

            var result = await _manager.UpdateAsync(Id, request, cancellationToken);

            if (result.IsSuccess)
                return Ok();

            return result.Error.ErrorCode switch
            {
                403 => Forbid(result.Error.ErrorMessage),
                404 => NotFound(result.Error),
                409 => Conflict(result.Error),
                _ => BadRequest(result.Error)
            };
        }

        /// <summary>
        /// Удалить услугу
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("service")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(Error), 404)]
        [ProducesResponseType(typeof(Error), 409)]
        public async Task<IActionResult> Delete([FromQuery]int serviceId,CancellationToken cancellationToken)
        {
            if (!int.TryParse(User.FindFirst("Id")?.Value, out var id))
                return Unauthorized();

            var result = await _manager.DeleteAsync(id,serviceId,cancellationToken);

            if (result.IsSuccess)
                return Ok();

            return result.Error.ErrorCode switch
            {
                403 => Forbid(result.Error.ErrorMessage),
                404 => NotFound(result.Error),
                409 => Conflict(result.Error),
                _ => BadRequest(result.Error)
            };
        }
    }
}
