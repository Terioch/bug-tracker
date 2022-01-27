using BugTracker.Models;
using BugTracker.Data;
using BugTracker.Helpers;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BugTracker.Services.Mock;
using Npgsql;

static string GetHerokuConnectionString()
{
    string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URI") ?? "postgres://xofsabfoeatohw:c11fd9533cc550e302820db1453b010ee88c4f997636cfa37df617fff4f5a42a@ec2-107-21-146-133.compute-1.amazonaws.com:5432/d9b915952ucg8c";

    var databaseUri = new Uri(connectionUrl);

    string db = databaseUri.LocalPath.TrimStart('/');
    string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

    return $"Host={databaseUri.Host};Port={databaseUri.Port};Username={userInfo[0]};Password={userInfo[1]};Pooling=true;sslmode=Prefer;Trust Server Certificate=true";
}

var builder = WebApplication.CreateBuilder(args);
bool isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

// Add services to the container.
if (!isDev)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<BugTrackerDbContext>(options =>
    options.UseSqlServer(connectionString)); builder.Services.AddDbContext<BugTrackerDbContext>(options =>
    options.UseSqlServer(connectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
} else
{
    var connectionString = GetHerokuConnectionString();
    builder.Services.AddDbContext<BugTrackerDbContext>(options =>
    options.UseNpgsql(connectionString)); builder.Services.AddDbContext<BugTrackerDbContext>(options =>
    options.UseNpgsql(connectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

// Dependency Injection
builder.Services.AddScoped<IProjectRepository, ProjectDbRepository>();
builder.Services.AddScoped<IUserProjectRepository, UserProjectDbRepository>();
builder.Services.AddScoped<ITicketRepository, TicketDbRepository>();
builder.Services.AddScoped<ITicketHistoryRecordRepository, TicketHistoryRecordDbRepository>();
builder.Services.AddScoped<ITicketAttachmentRepository, TicketAttachmentDbRepository>();
builder.Services.AddScoped<ITicketCommentRepository, TicketCommentDbRepository>();
builder.Services.AddScoped<ProjectHelper, ProjectHelper>();
builder.Services.AddScoped<TicketHelper, TicketHelper>();
builder.Services.AddScoped<RoleHelper, RoleHelper>();
builder.Services.AddScoped<TicketAttachmentHelper, TicketAttachmentHelper>();
builder.Services.AddScoped<BugTrackerMockContext, BugTrackerMockContext>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BugTrackerDbContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
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
