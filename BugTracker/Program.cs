using BugTracker.Models;
using BugTracker.Contexts;
using BugTracker.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BugTracker.Repositories.Interfaces;
using BugTracker.Repositories.EF;
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

// Add services to the DI container.
if (isDev && !isDev)
{
    var connectionString = builder.Configuration.GetConnectionString("LocalSqlServerConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, x => x.MigrationsAssembly("BugTracker.SqlServerMigrations")));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}
else
{
    var connectionString = GetPgsqlConnectionString();
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, x => x.MigrationsAssembly("BugTracker.PgsqlMigrations")));    
}

builder.Services.AddTransient<IUnitOfWork, EF_UnitOfWork>();

builder.Services.AddTransient<ProjectHelper, ProjectHelper>();
builder.Services.AddTransient<TicketHelper, TicketHelper>();
builder.Services.AddTransient<RoleHelper, RoleHelper>();
builder.Services.AddTransient<TicketAttachmentHelper, TicketAttachmentHelper>();
builder.Services.AddTransient<TicketHistoryHelper, TicketHistoryHelper>();
builder.Services.AddTransient<AccountHelper, AccountHelper>();
builder.Services.AddTransient<ChartHelper, ChartHelper>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
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
