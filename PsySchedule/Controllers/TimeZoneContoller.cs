using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace PsySchedule.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class TimeZoneContoller : Controller
    {
        /// <summary>
        /// Метод получения списка TimeZone
        /// </summary>
        /// <returns></returns>
        [HttpGet("time_zone")]
        [ProducesResponseType(typeof(ReadOnlyCollection<TimeZoneInfo>),200)]
        public IActionResult GetTimeZone()
        {
            var timeZone = TimeZoneInfo.GetSystemTimeZones();

            return Ok(timeZone);
        }
    }
}
