using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security;
using System.Text;
using Hauna.Urooj.Hauna.Urooj.Models;

namespace Hauna.Urooj.Hauna.Urooj.Services.Service
{
    public class TokenManager
    {
        private readonly IConfiguration _configuration;

        public TokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Method to generate JWT token
        public string GenerateWebToken(string username,string isAdmin)
        {
            // Define the claims (user details)
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, isAdmin) 
            };

            // Get the AppSecret and Algorithm from configuration
            var secretKey = _configuration["AppSettings:AppSecret"];
            var algorithm = _configuration["AppSettings:Algorithm"];

            // Select the signing algorithm (HmacSha256 in this case)
            string securityAlgorithm;
            if (algorithm == "HmacSha256")
            {
                securityAlgorithm = SecurityAlgorithms.HmacSha256;
            }
            else
            {
                throw new SecurityException("Invalid signing algorithm");
            }

            // Create a symmetric security key from the secret
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, securityAlgorithm);

            // Generate the JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["AppSettings:Appname"],  // Issuer
                audience: _configuration["AppSettings:Appname"], // Audience
                claims: claims,  // Claims (user data)
                expires: DateTime.Now.AddHours(1),  // Token expiration (1 hour)
                signingCredentials: credentials
            );

            // Return the JWT token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenExtractedClass GetUserFromToken(string token)
        {
            TokenExtractedClass tokenExtractedClass = new TokenExtractedClass();
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Get the AppSecret from the configuration
                var secretKey = _configuration["AppSettings:AppSecret"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                // Validate the token using the secret key
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = key
                };

                // Validate the token and get the principal (user)
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                // Extract username and userId claims from the principal
                tokenExtractedClass.userName = principal.FindFirst(ClaimTypes.Name)?.Value;
                tokenExtractedClass.isAdmin = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return tokenExtractedClass;
            }
            catch
            {
                // If token is invalid or expired, return null or handle the error
                return null;
            }
        }
    }
}
