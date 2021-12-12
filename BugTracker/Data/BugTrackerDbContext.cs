using BugTracker.Models;
using BugTracker.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Data;

public class BugTrackerDbContext : IdentityDbContext<ApplicationUser>
{
    public BugTrackerDbContext(DbContextOptions<BugTrackerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project>? Projects { get; set; }

    public DbSet<Ticket>? Tickets { get; set; }

    public DbSet<UserProjects>? UserProjects { get; set; }

    public DbSet<TicketHistoryRecord>? TicketHistoryRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
