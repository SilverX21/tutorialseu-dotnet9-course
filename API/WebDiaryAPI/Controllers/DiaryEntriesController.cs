using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WebDiaryAPI.Data;
using WebDiaryAPI.Models;

namespace WebDiaryAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class DiaryEntriesController : ControllerBase
{
    #region Constructor and interfaces

    private readonly ApplicationDbContext _context;

    public DiaryEntriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    #endregion Constructor and interfaces

    #region Public methods

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DiaryEntry>>> GetDiaryEntries()
    {
        return await _context.DiaryEntries.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DiaryEntry>> GetDiaryEntry(int id)
    {
        var diaryEntry = await _context.DiaryEntries.FindAsync(id);

        if (diaryEntry is null)
        {
            return NotFound();
        }

        return Ok(diaryEntry);
    }

    [HttpPost]
    public async Task<ActionResult<DiaryEntry>> PostDiaryEntry(DiaryEntry diaryEntry)
    {
        diaryEntry.Id = 0;

        _context.Add(diaryEntry);
        await _context.SaveChangesAsync();

        //here we are creating the url to populate the location (this is in the headers)
        var resourceUrl = Url.Action(nameof(GetDiaryEntry), new { id = diaryEntry.Id });

        //here we pass the location and the created object
        return Created(resourceUrl, diaryEntry);
    }

    //here we are requesting the id of the diary entry that we want to update
    //we are requesting the diaryEntry from the json body that comes in the request, the id comes in the url :)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDiaryEntry(int id, [FromBody] DiaryEntry diaryEntry)
    {
        if (id != diaryEntry.Id)
        {
            return BadRequest();
        }

        //here we are providing the diaryEntry that the user provided and inform the EF Core that this entry as been updated
        _context.Entry(diaryEntry).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DiaryEntryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiaryEntry(int id)
    {
        var diaryEntry = await _context.DiaryEntries.FindAsync(id);

        if (diaryEntry is null)
        {
            return BadRequest();
        }

        _context.DiaryEntries.Remove(diaryEntry);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    #endregion Public methods

    #region Private methods

    private bool DiaryEntryExists(int id)
    {
        return _context.DiaryEntries.Any(x => x.Id == id);
    }

    #endregion Private methods
}