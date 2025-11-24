namespace Banca_movil.Dto
{
    public class RegisterRequestDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int RoleId { get; set; } = 2; // Usuario por defecto
    }
}
