using AutoMapper;
using Banca_movil.Dto;
using Banca_movil.Models;
using Banca_movil.Repo;
using System.Security.Cryptography;

namespace Banca_movil.Service
{
    public class AuthService(IUserRepository userRepo, ITokenService tokenService, IEmailService emailService, IMapper mapper) : IAuthService
    {
        private readonly IUserRepository _userRepo = userRepo;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IEmailService _emailService = emailService;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> ChangePasswordAsync(string username, ChangePasswordDto dto)
        {
            var user = await _userRepo.GetUserByUserName(username);
            if (user == null || !_userRepo.ValidatePassWord(user, dto.CurrentPassword))
                return false;

            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.NewPassword);
            await _userRepo.SaveAsync();
            return true;
        }

        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userRepo.GetUserByEmail(email);
            if (user == null || user.EmailConfirmationToken != token)
                return false;

            var decodedToken = Uri.UnescapeDataString(token.Trim());

            if (!string.Equals(user.EmailConfirmationToken, decodedToken, StringComparison.Ordinal))
                return false;

            user.EmailConfirmed = true;
            user.EmailConfirmationToken = null;
            await _userRepo.SaveAsync();

            return true;
        }

        public async Task<(bool Success, LoginResponseDto? Result, string? ErrorMessage)> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepo.GetUserByEmail(dto.Email);
            if (user == null || !_userRepo.ValidatePassWord(user, dto.Password))
                return (false, null, "Credenciales inválidas");

            if (!user.EmailConfirmed)
                return (false, null, "Debe confirmar su correo electrónico.");

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userRepo.SaveAsync();

            var result = new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.Now.AddMinutes(15)
            };

            return (true, result, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterRequestDto dto)
        {
            var exists = await _userRepo.GetUserByEmail(dto.Email);
            if (exists != null)
                return (false, "Correo ya en uso, Intente con otro por favor");

            var user = _mapper.Map<User>(dto);
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password);

            //Generamos el token a enviar
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var rawToken = Convert.ToBase64String(tokenBytes);
            var encodedToken = Uri.EscapeDataString(rawToken);

            user.EmailConfirmationToken = rawToken;


            user.EmailConfirmed = false;

            await _userRepo.AddAsync(user);
            await _userRepo.SaveAsync();
            string confirmationLink = $"https://localhost:7255/Auth/ConfirmEmail?email={user.Email}&token={encodedToken}";

            string htmlBody = $"""
                            <!DOCTYPE html>
                            <html><body>
                            <p>Hola,</p>
                            <p>Haz clic en el siguiente botón para confirmar tu cuenta:</p>
                            <p><a href="{confirmationLink}" style='background:#007BFF;color:white;padding:10px 20px;border-radius:5px;text-decoration:none;'>Confirmar Cuenta</a></p>
                            </body></html>
                            """;

            _emailService.SendPasswordResetEmail(user.Email, htmlBody);

            return (true, null);
        }

        public async Task<bool> ResendConfirmationEmailAsync(string email)
        {
            var user = await _userRepo.GetUserByEmail(email);
            if (user == null || user.EmailConfirmed)
                return false;

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var rawToken = Convert.ToBase64String(tokenBytes);
            var encodedToken = Uri.EscapeDataString(rawToken);
            user.EmailConfirmationToken = rawToken;

            await _userRepo.SaveAsync();


            string confirmationLink = $"https://localhost:7255/Auth/ConfirmEmail?email={user.Email}&token={encodedToken}";

            string htmlBody = $"""
                            <!DOCTYPE html>
                            <html><body>
                            <p>Hola,</p>
                            <p>Haz clic en el siguiente botón para confirmar tu cuenta:</p>
                            <p><a href="{confirmationLink}" style='background:#007BFF;color:white;padding:10px 20px;border-radius:5px;text-decoration:none;'>Confirmar Cuenta</a></p>
                            </body></html>
                            """;
            _emailService.SendPasswordResetEmail(user.Email, htmlBody);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            var user = await _userRepo.GetUserByEmail(dto.Email);

            if (user == null)
                return false;

            // Decodificar el token del URL encoding si viene codificado
            var providedToken = Uri.UnescapeDataString(dto.Token ?? string.Empty);
            var storedToken = user.PasswordResetToken ?? string.Empty;

            // Comparar los tokens
            if (!storedToken.Equals(providedToken, StringComparison.Ordinal))
            {
                return false;
            }

            // Verificar expiración
            if (!user.ResetTokenExpiryTime.HasValue || user.ResetTokenExpiryTime.Value < DateTime.Now)
            {
                return false;
            }

            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpiryTime = null;

            await _userRepo.SaveAsync();
            return true;
        }


        public async Task<bool> SendResetPasswordLinkAsync(string email)
        {
            var user = await _userRepo.GetUserByEmail(email);
            if (user == null) return false;

            var token = _tokenService.GeneratePasswordResetToken();
            var encodedToken = Uri.EscapeDataString(token);
            user.PasswordResetToken = token;
            user.ResetTokenExpiryTime = DateTime.UtcNow.AddHours(1); ;
            await _userRepo.SaveAsync();
            string confirmationLink = $"https://localhost:7255/Auth/ResetPassword?email={user.Email}&token={encodedToken}";

            string htmlBody = $"""
                            <!DOCTYPE html>
                            <html><body>
                            <p>Hola,</p>
                            <p>Haz clic en el siguiente botón para confirmar tu cuenta:</p>
                            <p><a href="{confirmationLink}" style='background:#007BFF;color:white;padding:10px 20px;border-radius:5px;text-decoration:none;'>Confirmar Cuenta</a></p>
                            </body></html>
                            """;

            _emailService.SendPasswordResetEmail(user.Email, htmlBody);

            return true;
        }
        public async Task<LoginResponseDto?> RefreshAccessTokenAsync(string refreshToken)
        {
            var user = await _userRepo.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return null;

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepo.SaveAsync();

            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };
        }





    }
}
