using Microsoft.IdentityModel.Tokens;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Security
{
    public class TokenService
    {
        private readonly JWTSettings _jwtSettings;

        public TokenService(JWTSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public int GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // Set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken)
                throw new SecurityTokenException("Invalid token");

            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            return Convert.ToInt32(userId);
        }
    }
}
