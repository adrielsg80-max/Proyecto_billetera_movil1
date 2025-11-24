using Banca_movil.Dto;

namespace Banca_movil.Service
{
    public interface IAuthService
    {
        Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterRequestDto dto);
        Task<(bool Success, LoginResponseDto? Result, string? ErrorMessage)> LoginAsync(LoginRequestDto dto);
        Task<bool> ConfirmEmailAsync(string email, string token);
        Task<bool> SendResetPasswordLinkAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto);
        Task<bool> ResendConfirmationEmailAsync(string email);
        Task<bool> ChangePasswordAsync(string username, ChangePasswordDto dto);
        Task<LoginResponseDto?> RefreshAccessTokenAsync(string refreshToken);
    }
}
