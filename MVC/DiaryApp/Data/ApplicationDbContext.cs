using DiaryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DiaryApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    //steps to create a table using ef core:
    /*
     1. create a class
     2. add DB set
     3. add-migration Name_of_the_migration
     4. update-database
     */

    public DbSet<DiaryEntry> DiaryEntries { get; set; }

    //this method is used to seed data into the database. We use the override keyword because we are overring the implementation of the default one that is in the DbCotnext class
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // here we are seeding the database with data, we use HasData() to check if there is any data in the database. To make this to seed the database, we need to add a migration
        modelBuilder.Entity<DiaryEntry>().HasData(
            new DiaryEntry
            {
                Id = 1,
                Title = "Went Hiking",
                Content = "Went hiking with Joe",
                Created = DateTime.Now
            },
            new DiaryEntry
            {
                Id = 2,
                Title = "Went Shopping",
                Content = "Went shopping with Joe",
                Created = DateTime.Now
            },
            new DiaryEntry
            {
                Id = 3,
                Title = "Went Diving",
                Content = "Went diving with Joe",
                Created = DateTime.Now
            }
        );
    }
}