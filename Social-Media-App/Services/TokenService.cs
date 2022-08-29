using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;
using Social_Media_App.Entities;
using Social_Media_App.Interfaces;

namespace Social_Media_App.Services;

public class TokenService : ITokenService
{
    //SECURITY KEY THAT WILL HANDLE ENCRYPTION AND DECRYPTION
    private readonly SymmetricSecurityKey _securityKey;
    
    //INJECTING THE SYMMETRIC SECURITY KEY TO UTILIZE THE TOKEN SECRET KEY
    public TokenService(IConfiguration config)
    {
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
    }
    
    //METHOD THAT WILL GENERATE TOKENS ON REGISTER AND LOGINS
    public string GenerateToken(AppUser user)
    {
        //CLAIMS THAT WILL BE SET IN THE TOKEN
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
        };
        
        //THE SIGNING CREDENTIALS FOR A GIVEN TOKEN
        var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha512Signature);
        
        //ELEMENTS THAT DEFINE THE TOKEN
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = credentials
        };
        
        //OBJECT THAT WILL CREATE AND WRITE THE TOKEN
        var tokenHandler = new JwtSecurityTokenHandler();
        
        //TOKEN CREATION
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        //RETURNING THE CREATED TOKEN FOR A GIVEN USER
        return tokenHandler.WriteToken(token);
    }
}