using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThinkboardApi.Data;
using ThinkboardApi.DTOs;
using ThinkboardApi.Models;
using ThinkboardApi.Services;

namespace ThinkboardApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ThinkboardDbContext _context;
    private readonly PasswordService _passwordService;

    public AuthController(ThinkboardDbContext context, PasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existing != null)
            return BadRequest("Email already in use.");

        var user = new User
        {
            Email = request.Email,
            PasswordHash = _passwordService.Hash(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { user.Id, user.Email });
    }
}