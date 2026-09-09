using Microsoft.AspNetCore.Mvc;

namespace PsySchedule.Controllers
{
    public abstract class BaseController : Controller
    {
        protected int GetUserId()
        {
            if (!int.TryParse(User.FindFirst("Id")?.Value, out var Id))
                return -1;

            return Id;
        }
    }
}
