using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThinkboardApi.Data;

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
}