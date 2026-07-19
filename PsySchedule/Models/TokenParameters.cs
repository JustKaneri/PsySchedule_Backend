namespace PsySchedule.Models
{
    /// <summary>
    /// Параметры Jwt
    /// </summary>
    public class TokenParameters
    {
        public string Key { get; set; }
        public string ExpireTime { get; set; }
        public string ExpireTimeRefresh { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
    }
}
