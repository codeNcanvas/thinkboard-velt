using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThinkboardApi.Data;
using ThinkboardApi.DTOs;
using ThinkboardApi.Models;

namespace ThinkboardApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotesController : ControllerBase
{
    private readonly ThinkboardDbContext _context;

    public NotesController(ThinkboardDbContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        var notes = await _context.Notes.Where(n => n.UserId == userId).ToListAsync();
        return Ok(notes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserId();
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        if (note == null)
            return NotFound();

        return Ok(note);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateNoteRequest request)
    {
        var note = new Note
        {
            Title = request.Title,
            Content = request.Content,
            UserId = GetUserId()
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateNoteRequest request)
    {
        var userId = GetUserId();
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        if (note == null)
            return NotFound();

        note.Title = request.Title;
        note.Content = request.Content;
        note.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(note);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        if (note == null)
            return NotFound();

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}