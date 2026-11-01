using Microsoft.AspNetCore.Mvc;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using System.Collections.ObjectModel;

namespace PsySchedule.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class TimeZoneContoller : Controller
    {
        private readonly ITimeZoneService _timeZoneService;

        public TimeZoneContoller(ITimeZoneService timeZoneService)
        {
            _timeZoneService = timeZoneService;
        }

        /// <summary>
        /// Метод получения списка TimeZone
        /// </summary>
        /// <returns></returns>
        [HttpGet("time-zone")]
        [ProducesResponseType(typeof(ReadOnlyCollection<TimeZoneInfoDto>),200)]
        public IActionResult GetTimeZone()
        {
            var timeZone = _timeZoneService.GetTimeZones();

            return Ok(timeZone);
        }
    }
}
