using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;
using System.Collections;
using System.Security.Claims;

namespace PsySchedule.Controllers
{
    /// <summary>
    /// Контролер для работы с шаблонами расписания
    /// </summary>
    [ApiController]
    [Route("api/v1/schedule")]
    public class ScheduleTemplateController: Controller
    {
        private readonly IScheduleTemplateService _scheduleTemplateService;
        private readonly IValidator<IEnumerable<ScheduleTemplateDayDto>> _validatorTemplates;
        private readonly IValidator<ScheduleTemplateDayDto> _validatorTemplateDay;

        public ScheduleTemplateController(IScheduleTemplateService scheduleTemplateService, IValidator<IEnumerable<ScheduleTemplateDayDto>> validatorTemplates,
                                          IValidator<ScheduleTemplateDayDto> validatorTemplateDay)
        {
            _scheduleTemplateService = scheduleTemplateService;
            _validatorTemplates = validatorTemplates;
            _validatorTemplateDay = validatorTemplateDay;
        }

        /// <summary>
        /// Получить шаблон расписания
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("templates")]
        [Authorize]
        [ProducesResponseType(typeof(ScheduleTemplatesDto),200)]
        [ProducesResponseType(typeof(Error), 401)]
        [ProducesResponseType(typeof(Error), 404)]
        public async Task<IActionResult> GetScheduleTemplate(CancellationToken cancellationToken)
        {
            var identity = Request.HttpContext.User.Identity as ClaimsIdentity;

            int Id;
            if (identity == null)
                return Unauthorized("Token is empty");

            Id = int.Parse(identity.FindFirst("Id").Value);

            var templates = await _scheduleTemplateService.GetAsync(Id,cancellationToken);

            if (templates.IsSuccess)
                return Ok(templates.Value);

            return templates.Error.ErrorCode switch
            {
                400 => BadRequest(templates.Error),
                401 => Unauthorized(templates.Error),
                404 => NotFound(templates.Error),
                _ => BadRequest()
            };
        }

        /// <summary>
        /// Создать шаблон
        /// </summary>
        /// <param name="scheduleTemplate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("templates")]
        [Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(List<ValidationFailure>), 400)]
        [ProducesResponseType(typeof(Error), 401)]
        [ProducesResponseType(typeof(Error), 409)]
        public async Task<IActionResult> CreateScheduleTemplate(ScheduleTemplatesDto scheduleTemplate, CancellationToken cancellationToken)
        {

            var validate = _validatorTemplates.Validate(scheduleTemplate.TemplateDays);

            if (!validate.IsValid)
                return BadRequest(validate.Errors);

            var identity = Request.HttpContext.User.Identity as ClaimsIdentity;

            int Id;
            if (identity == null)
                return Unauthorized("Token is empty");

            Id = int.Parse(identity.FindFirst("Id").Value);

            var result = await _scheduleTemplateService.CreateAsync(scheduleTemplate.TemplateDays, Id, cancellationToken);


            if (result.IsSuccess)
                return Created();


            return result.Error.ErrorCode switch
            {
                400 => BadRequest(result.Error),
                401 => Unauthorized(result.Error),
                409 => Conflict(result.Error),
                _ => BadRequest()
            };  
        }

        /// <summary>
        /// Обновить или создать шаблон
        /// </summary>
        /// <param name="scheduleTemplate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("template")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<ValidationFailure>), 400)]
        [ProducesResponseType(typeof(Error), 401)]
        public async Task<IActionResult> UpdateScheduleTemplate(ScheduleTemplateDayDto scheduleTemplate, CancellationToken cancellationToken)
        {
            var validate = _validatorTemplateDay.Validate(scheduleTemplate);

            if (!validate.IsValid)
                return BadRequest(validate.Errors);

            var identity = Request.HttpContext.User.Identity as ClaimsIdentity;

            int Id;
            if (identity == null)
                return Unauthorized("Token is empty");

            Id = int.Parse(identity.FindFirst("Id").Value);


            var template = await _scheduleTemplateService.UpdateOrCreateAsync(scheduleTemplate, Id, cancellationToken);

            if (template.IsSuccess)
                return Ok(template);

            return template.Error.ErrorCode switch
            {
                400 => BadRequest(template.Error),
                401 => Unauthorized(template.Error),
                _ => BadRequest()
            };
        }
    }
}
