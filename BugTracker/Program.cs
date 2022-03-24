using BugTracker.Models;
using BugTracker.Contexts;
using BugTracker.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BugTracker.Repositories.Mock;
using BugTracker.Repositories.Interfaces;
using BugTracker.Repositories.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);
bool isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

string GetPgsqlConnectionString()
{
    string? databaseUrl = builder.Configuration["DATABASE_URL"];
    Uri uri = new(databaseUrl);
    return $"host={uri.Host};username={uri.UserInfo.Split(':')[0]};password={uri.UserInfo.Split(':')[1]};database={uri.LocalPath.Substring(1)};pooling=true;";
}

// Add services to the container.
if (isDev && !isDev)
{
    var connectionString = builder.Configuration.GetConnectionString("BugTrackerDbContextConnection");
    builder.Services.AddDbContext<BugTrackerDbContext>(options =>
    options.UseSqlServer(connectionString, x => x.MigrationsAssembly("BugTracker.SqlServerMigrations")));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddScoped<IProjectRepository, ProjectDbRepository>();
    builder.Services.AddScoped<IUserProjectRepository, UserProjectDbRepository>();
    builder.Services.AddScoped<ITicketRepository, TicketDbRepository>();
    builder.Services.AddScoped<ITicketHistoryRepository, TicketHistoryDbRepository>();
    builder.Services.AddScoped<ITicketAttachmentRepository, TicketAttachmentDbRepository>();
    builder.Services.AddScoped<ITicketCommentRepository, TicketCommentDbRepository>();
}
else
{
    var connectionString = GetPgsqlConnectionString();
    builder.Services.AddDbContext<BugTrackerDbContext>(options =>
    options.UseNpgsql(connectionString, x => x.MigrationsAssembly("BugTracker.PgsqlMigrations")));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddScoped<IProjectRepository, ProjectMockRepository>();
    builder.Services.AddScoped<IUserProjectRepository, UserProjectMockRepository>();
    builder.Services.AddScoped<ITicketRepository, TicketMockRepository>();
    builder.Services.AddScoped<ITicketHistoryRepository, TicketHistoryMockRepository>();
    builder.Services.AddScoped<ITicketAttachmentRepository, TicketAttachmentMockRepository>();
    builder.Services.AddScoped<ITicketCommentRepository, TicketCommentMockRepository>();
}

builder.Services.AddScoped<ProjectHelper, ProjectHelper>();
builder.Services.AddScoped<TicketHelper, TicketHelper>();
builder.Services.AddScoped<RoleHelper, RoleHelper>();
builder.Services.AddScoped<TicketAttachmentHelper, TicketAttachmentHelper>();
builder.Services.AddScoped<TicketHistoryHelper, TicketHistoryHelper>();
builder.Services.AddScoped<AccountHelper, AccountHelper>();
builder.Services.AddScoped<ChartHelper, ChartHelper>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BugTrackerDbContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddMvc(options =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

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
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
