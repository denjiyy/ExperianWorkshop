//using BankManagementSystem.DataProcessor;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using System.Linq;
//using BankManagementSystem.Models;
//using System.Security.Cryptography;
//using Microsoft.EntityFrameworkCore;
//using Flurl.Http;
//using Microsoft.AspNetCore.Authorization;

//[ApiController]
//[Route("/auth")]
//public class AuthController : ControllerBase
//{
//    private readonly BankSystemContext _context;
//    private readonly IConfiguration _configuration;

//    public AuthController(BankSystemContext context, IConfiguration configuration)
//    {
//        _context = context;
//        _configuration = configuration;
//    }

//    [HttpPost]
//    [Route("login")]
//    [AllowAnonymous]
//    public IActionResult Login([FromBody] LoginDto loginDto)
//    {
//        User? user = _context.Users
//            .FirstOrDefault(x => x.Username == loginDto.Username && x.Password == loginDto.Password);
//        if (user != null)
//        {
//            var claims = new[]
//            {
//                 new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
//                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                 new Claim("UserId", user.Id.ToString()),
//                 new Claim("Username", user.Username.ToString())
//            };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
//            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddHours(1), signingCredentials: signIn);

//            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

//            return Ok(new { Token = tokenValue });
//        }

//        return NoContent();
//    }

//    [HttpPost("refresh")]
//    public async Task<IActionResult> Refresh()
//    {
//        var refreshToken = Request.Cookies["refreshToken"]; // Retrieve refresh token from cookie

//        if (refreshToken == null)
//        {
//            return Unauthorized("Refresh accessToken is missing");
//        }

//        var storedToken = await _context.RefreshTokens
//            .Include(rt => rt.User)
//            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

//        if (storedToken == null || storedToken.ExpiryDate <= DateTime.UtcNow)
//        {
//            return Unauthorized("Invalid or expired refresh accessToken");
//        }

//        var user = storedToken.User;
//        if (user == null)
//        {
//            return Unauthorized("User not found");
//        }

//        var newToken = GenerateJwtToken(user.Username, _configuration["Jwt:SecretKey"]);

//        ///var newRefreshToken = GenerateRefreshTokenString();
//        //storedToken.Token = newRefreshToken;
//        //storedToken.ExpiryDate = DateTime.UtcNow.AddDays(7);
//        //_context.RefreshTokens.Update(storedToken);
//        await _context.SaveChangesAsync();

//        //SetRefreshTokenCookie(newRefreshToken);

//        return Ok(new { token = newToken });
//    }

//    private void SetRefreshTokenCookie(string refreshToken)
//    {
//        var cookieOptions = new CookieOptions
//        {
//            HttpOnly = true, // This ensures the token is not accessible by JavaScript
//            Secure = true,
//            Expires = DateTime.UtcNow.AddDays(7)
//        };

//        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
//    }

//    private string GenerateRefreshTokenString()
//    {
//        var randomNumber = new byte[64];

//        using (var numberGenerator = RandomNumberGenerator.Create())
//        {
//            numberGenerator.GetBytes(randomNumber);
//        }

//        return Convert.ToBase64String(randomNumber);
//    }

//    private string GenerateJwtToken(string username, string secretKey)
//    {
//        var tokenHandler = new JwtSecurityTokenHandler();
//        var key = Encoding.ASCII.GetBytes(secretKey);

//        var tokenDescriptor = new SecurityTokenDescriptor
//        {
//            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
//            Expires = DateTime.UtcNow.AddHours(1),
//            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//        };

//        var token = tokenHandler.CreateToken(tokenDescriptor);
//        return tokenHandler.WriteToken(token);
//    }
//}
