using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bankai.Domain.Entities;
using Bankai.Domain.Enums;
using Bankai.Infrastructure.Data;

namespace Bankai.API.Controllers;

/// <summary>
/// Authentication Controller (VG Requirement: JWT + RBAC)
/// 
/// POST /api/auth/register   → Create a new user account
/// POST /api/auth/login      → Authenticate and receive JWT token
/// 
/// The JWT token contains the user's Role as a claim,
/// enabling Role-Based Access Control (RBAC) across all controllers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public record RegisterRequest(string Email, string Password);
    public record LoginRequest(string Email, string Password);
    public record AuthResponse(string Token, string Email, string Role, DateTime ExpiresAt);

    // ── POST /api/auth/register ───────────────────────────────
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        bool emailTaken = await _context.Users
            .AnyAsync(u => u.Email == request.Email.ToLower(), cancellationToken);

        if (emailTaken)
            return Conflict($"Email '{request.Email}' is already registered.");

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User(request.Email, passwordHash, UserRole.User);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        var token = GenerateJwtToken(user);
        return CreatedAtAction(null, null, token);
    }

    // ── POST /api/auth/login ──────────────────────────────────
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email.ToLower(), cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Invalid email or password.");

        var token = GenerateJwtToken(user);
        return Ok(token);
    }

    // ── JWT Token Generation ──────────────────────────────────
    private AuthResponse GenerateJwtToken(User user)
    {
        var key    = _config["Jwt:Key"] ?? "BankaiSecretKeyForDevelopment2026!";
        var issuer = _config["Jwt:Issuer"] ?? "bankai.se";
        var expiry = DateTime.UtcNow.AddHours(8);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email,           user.Email),
            new Claim(ClaimTypes.Role,            user.Role.ToString()), // RBAC claim
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:   issuer,
            audience: null,
            claims:   claims,
            expires:  expiry,
            signingCredentials: credentials
        );

        return new AuthResponse(
            Token:     new JwtSecurityTokenHandler().WriteToken(token),
            Email:     user.Email,
            Role:      user.Role.ToString(),
            ExpiresAt: expiry
        );
    }
}
