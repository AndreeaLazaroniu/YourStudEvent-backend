using BEYourStudEvents.Entities;

namespace BEYourStudEvents.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}