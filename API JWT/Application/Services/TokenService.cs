using API_JWT.Application.Domain;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_JWT.Application.Services
{
    public static class TokenService
    {
        public static class Settings
        {
            public static string Secret = "46070D4BF934FB0D4B06D9E2C46E346944E322444900A435D7D9A95E6D7435F5";
        }

        public static string GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                { // Dentro do token pode se armazenar informações sobre o usuário, como o nome.
                    new Claim(ClaimTypes.Name, user.Name.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(3), // Pode se configurar o tempo do token para determinar quando deve ser expirado. 3 horas.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            //CRIA O TOKEN 
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);  
        }
    }
}
