using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BankManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Register IHttpContextAccessor if needed
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Configure session management
        builder.Services.AddDistributedMemoryCache(); // Required for session
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(120); // Set session timeout
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true; // Make session cookie essential
        });

        // Add CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });

        builder.Services.AddDbContext<BankSystemContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        /*
        // Configure JWT authentication
        var secretKey = builder.Configuration["Jwt:SecretKey"];
        var key = Encoding.ASCII.GetBytes(secretKey);

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true; // Ensure HTTPS is used
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = "your-issuer",
                ValidateAudience = true,
                ValidAudience = "your-audience",
                ValidateLifetime = true, // Validate token expiration
                ClockSkew = TimeSpan.Zero // Optional: adjust if needed for token expiration tolerance
            };
        });
        */

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseCors("AllowAllOrigins"); // Make sure CORS policy is applied

        // Commented out authentication since JWT is disabled for testing
        // app.UseAuthentication(); // Ensure authentication is before authorization
        app.UseAuthorization();
        app.UseSession(); // Use session before endpoint mapping

        // Add the custom TokenMiddleware directly in the pipeline
        app.UseMiddleware<TokenMiddleware>();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}