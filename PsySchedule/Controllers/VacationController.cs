using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;

namespace PsySchedule.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class VacationController : BaseController
    {
        private readonly IVacationService _service;
        private readonly IValidator<CreateVacationRequest> _validator;

        public VacationController(IVacationService service,IValidator<CreateVacationRequest> validator)
        {
            _service = service;
            _validator = validator;
        }

        /// <summary>
        /// Получить список отпусков
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("vacations")]
        [ProducesResponseType(typeof(IEnumerable<VacationResponse>),200)]
        [ProducesResponseType(typeof(Error),400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            int id = GetUserId();

            if (id == -1) return Unauthorized();

            var vacations = await _service.GetAsync(id, cancellationToken);

            if(vacations.IsSuccess)
                return Ok(vacations.Value);

            return BadRequest(vacations.Error);
        }

        /// <summary>
        /// Создать отпуск
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("vacation")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Create([FromBody]CreateVacationRequest request, CancellationToken cancellationToken)
        {
            var resultValidation = _validator.Validate(request);

            if (!resultValidation.IsValid)
                return BadRequest(resultValidation.Errors);

            int id = GetUserId();

            if (id == -1) return Unauthorized();

            var resultCreate = await _service.CreateAsync(id, request, cancellationToken);

            if (resultCreate.IsSuccess)
                return Ok();

            return NotFound(resultCreate.Error);
        }

        /// <summary>
        /// Отменить отпуск
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("vacation")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(Error), 409)]
        public async Task<IActionResult> Cancel([FromQuery]int Id, CancellationToken cancellationToken) 
        {
            int psyId = GetUserId();

            if (psyId == -1) return Unauthorized();

            var resultCancel = await _service.CancelAsync(psyId, Id, cancellationToken);

            if (resultCancel.IsSuccess)
                return Ok();

            return Conflict(resultCancel.Error);
        }
    }
}
