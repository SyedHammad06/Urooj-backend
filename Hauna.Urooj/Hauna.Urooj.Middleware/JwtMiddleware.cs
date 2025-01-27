using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Text;

namespace Hauna.Urooj.Hauna.Urooj.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                AttachClaimsPrincipalToContext(context, token);
            }

            await _next(context);
        }

        private void AttachClaimsPrincipalToContext(HttpContext context, string token)
        {
            try
            {
                var secretKey = _configuration["AppSettings:AppSecret"];
                var algorithm = _configuration["AppSettings:Algorithm"];

                string securityAlgorithm;
                if (algorithm == "HmacSha256")
                {
                    securityAlgorithm = SecurityAlgorithms.HmacSha256;
                }
                else
                {
                    throw new SecurityException("Invalid signing algorithm");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = key
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                context.User = principal;
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                context.Response.WriteAsync("{\"message\":\"Unauthorized\"}");
            }
        }
    }
}
