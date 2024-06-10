using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Options;
using TutorLizard.BusinessLogic.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddTransient<IBrowseService, BrowseService>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services
    .AddOptions<DataJsonFilePaths>()
    .Bind(builder.Configuration.GetSection(nameof(DataJsonFilePaths)))
    .ValidateDataAnnotations();
builder.Services.AddScoped<ITutorService, TutorService>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "CookieAuth";
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Auth:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"];
        options.SaveTokens = true;
    })
    .AddCookie("CookieAuth", options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "CookieAuth";
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });


builder.Services.AddDbContext<JaszczurContext>(configuration =>
{
    configuration
        .UseSqlServer(builder.Configuration.GetConnectionString("Default"),
                    b => b.MigrationsAssembly("TutorLizard.Web"))
        .LogTo(Console.WriteLine, LogLevel.Information);
});

builder.Services.AddTutorLizardDbRepositories<JaszczurContext>();



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
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.Always
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
