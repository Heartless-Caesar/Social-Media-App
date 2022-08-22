using Social_Media_App.Entities;

namespace Social_Media_App.Interfaces;

public interface ITokenService
{
    string GenerateToken(AppUser user);
}