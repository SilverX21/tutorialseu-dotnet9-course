// we've added Microsoft.EntityFrameworkCore.InMemory nuget to the DevSpot.Tests project to create a in memory database so we can test our repository :)

using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSpot.Tests;

public class JobPostingRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public JobPostingRepositoryTests()
    {
        //here we are creating a new instance of DbContextOptionsBuilder and passing the name of the in memory database we want to create
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("JobPostingDb")
            .Options;
    }

    //here we create a new context for the database
    private ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_options);

    [Fact]
    public async Task AddAsync_ShouldAddJobPosting()
    {
        var db = CreateDbContext();
        var repository = new JobPostingRepository(db);

        var jobPosting = new JobPosting
        {
            Title = "Test title adding",
            Description = "Test description",
            PostedDate = DateTime.Now,
            Company = "Test company",
            Location = "Test location",
            UserId = "TestUserIdAdding"
        };

        await repository.AddAsync(jobPosting);

        var result = db.JobPostings.Find(jobPosting.Id);

        //here we are checking if the job posting was added to the database
        Assert.NotNull(result);
        Assert.Equal("Test title adding", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnJobPosting()
    {
        var db = CreateDbContext();
        var repository = new JobPostingRepository(db);

        var jobPosting = new JobPosting
        {
            Title = "Test title",
            Description = "Test description",
            PostedDate = DateTime.Now,
            Company = "Test company",
            Location = "Test location",
            UserId = "1234"
        };

        await db.JobPostings.AddAsync(jobPosting);
        await db.SaveChangesAsync();

        var result = await repository.GetByIdAsync(jobPosting.Id);

        Assert.NotNull(result);
        Assert.Equal("Test title", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowKeyNotFoundException()
    {
        var db = CreateDbContext();
        var repository = new JobPostingRepository(db);

        //here we are trying to get a job posting that doesn't exist in the database, so we can get the error
        await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.GetByIdAsync(9999));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllJobPostings()
    {
        var db = CreateDbContext();
        var repository = new JobPostingRepository(db);

        var jobPosting1 = new JobPosting
        {
            Title = "Test title 1",
            Description = "Test description 1",
            PostedDate = DateTime.Now,
            Company = "Test company 1",
            Location = "Test location 1",
            UserId = "TestUserId1"
        };

        var jobPosting2 = new JobPosting
        {
            Title = "Test title 2",
            Description = "Test description 2",
            PostedDate = DateTime.Now,
            Company = "Test company 2",
            Location = "Test location 2",
            UserId = "TestUserId2"
        };

        await db.JobPostings.AddRangeAsync(jobPosting1, jobPosting2);
        await db.SaveChangesAsync();

        var result = await repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.True(result.Count() >= 2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateJobPosting()
    {
        var db = CreateDbContext();
        var repository = new JobPostingRepository(db);

        var jobPosting = new JobPosting
        {
            Title = "Test title",
            Description = "Test description",
            PostedDate = DateTime.Now,
            Company = "Test company",
            Location = "Test location",
            UserId = "1234"
        };

        await db.JobPostings.AddAsync(jobPosting);
        await db.SaveChangesAsync();

        jobPosting.Description = "Updated description";
        await repository.UpdateAsync(jobPosting);

        var result = db.JobPostings.Find(jobPosting.Id);

        Assert.NotNull(result);
        Assert.Equal("Updated description", result.Description);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteJobPosting()
    {
        var db = CreateDbContext();
        var repository = new JobPostingRepository(db);

        var jobPosting = new JobPosting
        {
            Title = "Test title delete",
            Description = "Test description",
            PostedDate = DateTime.Now,
            Company = "Test company",
            Location = "Test location",
            UserId = "TestUserIdDelete"
        };

        await db.JobPostings.AddAsync(jobPosting);
        await db.SaveChangesAsync();

        await repository.DeleteAsync(jobPosting.Id);

        var result = db.JobPostings.Find(jobPosting.Id);

        Assert.Null(result);
    }
}