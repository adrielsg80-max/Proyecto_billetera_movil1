using Banca_movil.Models;

namespace Banca_movil.Service
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        string GeneratePasswordResetToken();
    }
}
