using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Data.Repositories.Json;
using TutorLizard.BusinessLogic.Entities;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Options;
using TutorLizard.BusinessLogic.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddTransient<IBrowseService, BrowseService>();
builder.Services.AddScoped<IAdRequestRepository, AdRequestJsonRepository>();
builder.Services.AddScoped<IAdRepository, AdJsonRepository>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IScheduleItemRepository, ScheduleItemJsonRepository>();
builder.Services.AddScoped<IScheduleItemRequestRepository, ScheduleItemRequestJsonRepository>();
builder.Services.AddScoped<IUserRepository, UserJsonRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryJsonRepository>();
builder.Services
    .AddOptions<DataJsonFilePaths>()
    .Bind(builder.Configuration.GetSection(nameof(DataJsonFilePaths)))
    .ValidateDataAnnotations();
builder.Services.AddScoped<ITutorService, TutorService>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth",options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/User/AccessDenied";
        options.Cookie.Name = "CookieAuth";
        options.LoginPath = "/User/Login";
        options.LogoutPath = "/User/Logout";
    });

builder.Services.AddDbContext<JaszczurContext>(configuration =>
{
    configuration
        .UseSqlServer(builder.Configuration.GetConnectionString("Jaszczur"))
        .LogTo(Console.WriteLine, LogLevel.Information);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
