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
    string databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "postgres://goztfvfaridqxa:597a2d5ffcd5f5a25bb10eac832acd4e6b250368f2d7b9b8207630fddad9401b@ec2-54-211-96-137.compute-1.amazonaws.com:5432/df257q70amg4e2";
    bool isUrl = Uri.TryCreate(databaseUrl, UriKind.Absolute, out Uri? url);

    if (isUrl)
    {       
        return $"host={url.Host};username={url.UserInfo.Split(':')[0]};password={url.UserInfo.Split(':')[1]};database={url.LocalPath.Substring(1)};pooling=true;";              
    }
    return "";
}

var builder = WebApplication.CreateBuilder(args);
bool isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

// Add services to the container.
var connectionString = GetHerokuConnectionString();
builder.Services.AddDbContext<BugTrackerDbContext>(options =>
options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
/*if (!isDev)
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
    options.UseNpgsql(connectionString));    
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}*/

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
