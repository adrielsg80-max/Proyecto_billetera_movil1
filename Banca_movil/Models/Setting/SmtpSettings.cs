namespace Banca_movil.Models.Setting
{
    public class SmtpSettings
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string User { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
