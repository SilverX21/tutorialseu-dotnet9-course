using DevSpot.Data;
using DevSpot.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSpot.Repositories;

public class JobPostingRepository : IRepository<JobPosting>
{
    private readonly ApplicationDbContext _context;

    public JobPostingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(JobPosting entity)
    {
        await _context.JobPostings.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var jobPosting = await _context.JobPostings.FindAsync(id);

        if (jobPosting is null)
        {
            throw new KeyNotFoundException();
        }

        _context.JobPostings.Remove(jobPosting);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<JobPosting>> GetAllAsync()
    {
        // AsNoTracking() is used to avoid tracking entities in the context, it helps to improve performance :)
        return await _context.JobPostings.AsNoTracking().ToListAsync();
    }

    public async Task<JobPosting> GetByIdAsync(int id)
    {
        var jobPosting = await _context.JobPostings.FindAsync(id);

        if (jobPosting is null)
        {
            throw new KeyNotFoundException();
        }

        return jobPosting;
    }

    public async Task UpdateAsync(JobPosting entity)
    {
        _context.JobPostings.Update(entity);
        await _context.SaveChangesAsync();
    }
}