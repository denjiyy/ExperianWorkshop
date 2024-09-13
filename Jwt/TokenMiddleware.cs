using BankManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (int.TryParse(userIdClaim, out int userId))
            {
                // Verify the user exists in the database
                var dbContext = context.RequestServices.GetRequiredService<BankSystemContext>();
                var userExists = dbContext.Users.Any(u => u.Id == userId);

                if (userExists)
                {
                    context.Session.SetString("UserId", userId.ToString());
                }
            }
        }

        await _next(context);
    }
}