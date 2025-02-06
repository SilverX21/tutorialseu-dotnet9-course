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
            Title = "Test title",
            Description = "Teste description",
            PostedDate = DateTime.Now,
            Company = "Test company",
            Location = "Teste location",
            UserId = "1234"
        };

        await repository.AddAsync(jobPosting);

        var result = await db.JobPostings.SingleOrDefaultAsync(x => x.Title == "Test title");

        //here we are checking if the job posting was added to the database
        Assert.NotNull(result);
        Assert.Equal("Test title", result.Title);
    }
}