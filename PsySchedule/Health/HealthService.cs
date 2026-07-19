using Prometheus;
using System.Reflection.Emit;

namespace PsySchedule.Health
{
    public static class HealthService
    {
        /// <summary>
        /// Метрика кол-во успешных регистраций
        /// </summary>
        public static readonly Counter RegistrationsCountMetric = Metrics.CreateCounter(
            "registrations_success_total", // имя метрики
            "Total count of registrations success" // описание
        );
    }
}
