namespace Banca_movil.Models.Setting
{
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenValidityInMinutes { get; set; } = 15;
    }
}
