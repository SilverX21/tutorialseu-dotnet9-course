using DiaryApp.Data;
using DiaryApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiaryApp.Controllers;

public class DiaryEntriesController : Controller
{
    #region Constructor and interfaces

    private readonly ApplicationDbContext _db;

    public DiaryEntriesController(ApplicationDbContext db)
    {
        _db = db;
    }

    #endregion Constructor and interfaces

    #region Public methods

    public IActionResult Index()
    {
        List<DiaryEntry> objDiaryEntryList = _db.DiaryEntries.ToList();

        return View(objDiaryEntryList);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(DiaryEntry diaryEntry)
    {
        if (diaryEntry != null && diaryEntry.Title.Length < 3)
        {
            //here we add an error to the Title property
            ModelState.AddModelError("Title", "The title must be at least 3 characters long!");
        }

        if (ModelState.IsValid)
        {
            _db.DiaryEntries.Add(diaryEntry);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(diaryEntry);
    }

    public IActionResult Edit(int? id)
    {
        if (id is null || id == 0)
        {
            return NotFound();
        }

        DiaryEntry? diaryEntry = _db.DiaryEntries.Find(id);

        if (diaryEntry is null)
        {
            return NotFound();
        }

        return View(diaryEntry);
    }

    [HttpPost]
    public IActionResult Edit(DiaryEntry diaryEntry)
    {
        if (diaryEntry != null && diaryEntry.Title.Length < 3)
        {
            ModelState.AddModelError("Title", "The title must be at least 3 characters long!");
        }

        if (ModelState.IsValid)
        {
            _db.DiaryEntries.Update(diaryEntry);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(diaryEntry);
    }

    public IActionResult Delete(int? id)
    {
        if (id is null || id == 0)
        {
            return NotFound();
        }

        DiaryEntry? diaryEntry = _db.DiaryEntries.Find(id);

        if (diaryEntry is null)
        {
            return NotFound();
        }

        return View(diaryEntry);
    }

    [HttpPost]
    public IActionResult Delete(DiaryEntry diaryEntry)
    {
        _db.DiaryEntries.Remove(diaryEntry);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }

    #endregion Public methods
}