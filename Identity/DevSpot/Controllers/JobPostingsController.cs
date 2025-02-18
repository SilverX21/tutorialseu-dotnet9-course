using DevSpot.Constants;
using DevSpot.Models;
using DevSpot.Repositories;
using DevSpot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevSpot.Controllers;

//authorize attribute is used to restrict access to the controller to only authenticated users
[Authorize]
public class JobPostingsController : Controller
{
    private readonly IRepository<JobPosting> _repository;
    private readonly UserManager<IdentityUser> _userManager;

    public JobPostingsController(
        IRepository<JobPosting> repository,
        UserManager<IdentityUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    //allow anonymous users to access the index page
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        if (User.IsInRole(Roles.Employer))
        {
            var allJobPostings = await _repository.GetAllAsync();
            var userId = _userManager.GetUserId(User);
            var filteredJobPostings = allJobPostings.Where(jp => jp.UserId == userId);

            return View(filteredJobPostings);
        }

        var jobPostings = await _repository.GetAllAsync();

        return View(jobPostings);
    }

    //restrict access to the create page to only users with the roles Admin and Employer
    [Authorize(Roles = "Admin,Employer")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Employer")]
    public async Task<IActionResult> Create(JobPostingViewModel jobPostingVm)
    {
        //we can mofdify the validations for the model and remove some of the validations like this: (this is not a good practice)
        //ModelState.Remove("UserId");
        //ModelState.Remove("User");

        if (ModelState.IsValid)
        {
            //we can get the current user by using the object "User", that has the current user data. This will only work if there is a user logged in
            var jobPosting = new JobPosting
            {
                Title = jobPostingVm.Title,
                Description = jobPostingVm.Description,
                Company = jobPostingVm.Company,
                Location = jobPostingVm.Location,
                UserId = _userManager.GetUserId(User)
            };

            await _repository.AddAsync(jobPosting);
            return RedirectToAction(nameof(Index));
        }

        return View(jobPostingVm);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,Employer")]
    public async Task<IActionResult> Delete(int id)
    {
        var jobPosting = await _repository.GetByIdAsync(id);

        if (jobPosting is null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);

        if (User.IsInRole(Roles.Admin) is false && jobPosting.UserId != userId)
        {
            return Forbid();
        }

        await _repository.DeleteAsync(id);
        return Ok();
    }

    [Authorize(Roles = "Admin,Employer")]
    public async Task<IActionResult> DeleteEasy(int id)
    {
        var jobPosting = await _repository.GetByIdAsync(id);

        if (jobPosting is null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);

        if (User.IsInRole(Roles.Admin) is false && jobPosting.UserId != userId)
        {
            return Forbid();
        }

        await _repository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}