using NodaTime;
using PsySchedule.Dto;
using PsySchedule.Interfaces;

namespace PsySchedule.Services
{
    public class TimeZoneService : ITimeZoneService
    {
        private readonly IDateTimeZoneProvider _provider;

        public TimeZoneService()
        {
            _provider = DateTimeZoneProviders.Tzdb;
        }

        public bool CheckIsExists(string timeZoneId)
        {
            if (string.IsNullOrWhiteSpace(timeZoneId))
                return false;

            return _provider.GetZoneOrNull(timeZoneId) != null;
        }

        public DateTimeOffset ConvertFromUtc(DateTimeOffset utc, string timeZoneId)
        {
            var timeZone = _provider.GetZoneOrNull(timeZoneId);

            if (timeZone == null)
                throw new ArgumentException($"Unknown TimeZone: {timeZoneId}", nameof(timeZoneId));

            var instant = Instant.FromDateTimeOffset(utc);

            return instant.InZone(timeZone).ToDateTimeOffset();
        }

        public IReadOnlyCollection<TimeZoneInfoDto> GetTimeZones()
        {
            var now = SystemClock.Instance.GetCurrentInstant();

            return _provider.Ids
                .Select(id =>
                {
                    var timeZone = _provider[id];
                    var offset = timeZone.GetUtcOffset(now);

                    return new TimeZoneInfoDto(
                        Id: id,
                        DisplayName: id,
                        Offset: offset.ToString(),
                        OffsetSeconds: (int)offset.ToTimeSpan().TotalSeconds
                    );
                })
                .OrderBy(x => x.Id)
                .ToArray();
        }
    }
}
