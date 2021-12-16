using BugTracker.Areas.Identity.Data;
using BugTracker.Data;
using BugTracker.Helpers;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BugTrackerDbContext>(options =>
    options.UseSqlServer(connectionString));builder.Services.AddDbContext<BugTrackerDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Dependency Injection
builder.Services.AddScoped<IProjectRepository, ProjectDbRepository>();
builder.Services.AddScoped<ITicketRepository, TicketDbRepository>();
builder.Services.AddScoped<ITicketHistoryRecordRepository, TicketHistoryRecordDbRepository>();
builder.Services.AddScoped<IUserProjectRepository, UserProjectDbRepository>();
builder.Services.AddScoped<ProjectHelper, ProjectHelper>();
builder.Services.AddScoped<RoleHelper, RoleHelper>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
        options.SignIn.RequireConfirmedAccount = true;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BugTrackerDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Project}/{action=ListProjects}/{id?}");
app.MapRazorPages();

app.Run();
