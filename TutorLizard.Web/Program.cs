using Microsoft.AspNetCore.Authentication.Cookies;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Data.Repositories.Json;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Options;
using TutorLizard.BusinessLogic.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DataAccess>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserIdentificationService, UserIdentificationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IScheduleItemRepository, ScheduleItemJsonRepository>();
builder.Services.AddScoped<IScheduleItemRequestRepository, ScheduleItemRequestJsonRepository>();
builder.Services.AddScoped<IUserRepository, UserJsonRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryJsonRepository>();
builder.Services
    .AddOptions<DataJsonFilePaths>()
    .Bind(builder.Configuration.GetSection(nameof(DataJsonFilePaths)))
    .ValidateDataAnnotations();

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth",options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/User/AccessDenied";
        options.Cookie.Name = "CookieAuth";
        options.LoginPath = "/User/";
        options.LogoutPath = "/User/Logout";
        options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
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
