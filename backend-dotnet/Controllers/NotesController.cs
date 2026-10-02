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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notes = await _context.Notes.ToListAsync();
        return Ok(notes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var note = await _context.Notes.FindAsync(id);
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
        Content = request.Content
    };

    _context.Notes.Add(note);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
}

[HttpPut("{id}")]
public async Task<IActionResult> Update(int id, UpdateNoteRequest request)
{
    var note = await _context.Notes.FindAsync(id);
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
    var note = await _context.Notes.FindAsync(id);
    if (note == null)
        return NotFound();

    _context.Notes.Remove(note);
    await _context.SaveChangesAsync();

    return NoContent();
}

}