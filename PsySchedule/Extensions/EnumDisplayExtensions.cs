using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PsySchedule.Extensions
{
    public static class EnumDisplayExtensions
    {
        /// <summary>
        /// Получить имя из атрибута Display
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value)
        {
            return value.GetType()
                        .GetMember(value.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .GetName() ?? value.ToString();
        }
    }
}
