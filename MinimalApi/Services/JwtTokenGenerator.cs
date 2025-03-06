using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalApi.DbContexts;
using MinimalApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MinimalApi.Services;
public class JwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    public JwtTokenGenerator(IConfiguration configuration, AppDbContext dbContext)
    {
        _configuration = configuration ??
             throw new ArgumentNullException(nameof(configuration));
        _context = dbContext ??
            throw new ArgumentNullException(nameof(dbContext));
    }
    public async Task<string?> GenerateToken(
        AuthenticationRequestBodyDto authenticationRequestBodyDto)
    {
        var user = await ValidateUserCredentials(
                authenticationRequestBodyDto.UserName,
                authenticationRequestBodyDto.Password
        );

        if (user == null)
        {
            return null;
        }

        var claims = new[]
        {
            new Claim("Sub", user.UserId.ToString()),
            new Claim("user_name", user.UserName)
        };

        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Authentication:Secret"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Authentication:Issuer"],
            audience: _configuration["Authentication:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Authentication:ExpiresInMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<User> ValidateUserCredentials(string? userName, string? password)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password);
    }
}

