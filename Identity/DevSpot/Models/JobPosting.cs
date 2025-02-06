using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DevSpot.Models;

public class JobPosting
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string Company { get; set; }

    [Required]
    public string Location { get; set; }

    public DateTime PostedDate { get; set; } = DateTime.UtcNow;

    public bool IsApproved { get; set; }

    //using the two lines below, we are creating a relationship between the JobPosting and the User who posted the job. So, we need and Id and the entity we created to map the foreign key
    [Required]
    public string UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get; set; }
}