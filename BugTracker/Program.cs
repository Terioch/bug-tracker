using BugTracker.Models;
using BugTracker.Contexts;
using BugTracker.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BugTracker.Repositories.Mock;
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
if (isDev)
{
    var connectionString = builder.Configuration.GetConnectionString("LocalSqlServerConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, x => x.MigrationsAssembly("BugTracker.SqlServerMigrations")));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    /*builder.Services.AddScoped<IProjectRepository, EF_ProjectRepository>();
    builder.Services.AddScoped<IRepository<Ticket>, EF_TicketRepository>();
    builder.Services.AddScoped<IRepository<TicketHistoryRecord>, EF_TicketHistoryRepository>();
    builder.Services.AddScoped<IRepository<TicketAttachment>, EF_TicketAttachmentRepository>();
    builder.Services.AddScoped<IRepository<TicketComment>, EF_TicketCommentRepository>();*/
}
else
{
    var connectionString = GetPgsqlConnectionString();
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, x => x.MigrationsAssembly("BugTracker.PgsqlMigrations")));    
    /*builder.Services.AddScoped<IProjectRepository, Mock_ProjectRepository>();
    builder.Services.AddScoped<IRepository<Ticket>, Mock_TicketRepository>();
    builder.Services.AddScoped<IRepository<TicketHistoryRecord>, Mock_TicketHistoryRepository>();
    builder.Services.AddScoped<IRepository<TicketAttachment>, Mock_TicketAttachmentRepository>();
    builder.Services.AddScoped<IRepository<TicketComment>, Mock_TicketCommentRepository>();*/
}

builder.Services.AddScoped<IUnitOfWork, EF_UnitOfWork>();

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
