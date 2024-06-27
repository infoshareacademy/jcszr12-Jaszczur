using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Options;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.Web.Interfaces.Services;
using TutorLizard.Web.Models;
using TutorLizard.Web.Services;
using TutorLizard.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddTransient<IBrowseService, BrowseService>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services
    .AddOptions<DataJsonFilePaths>()
    .Bind(builder.Configuration.GetSection(nameof(DataJsonFilePaths)))
    .ValidateDataAnnotations();
builder.Services.AddScoped<ITutorService, TutorService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUiMessagesService, UiMessagesService>();

builder.Services.AddAuthentication(options => 
    {
        options.DefaultScheme = "CookieAuth";
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Auth:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"];
        options.SaveTokens = true;
        options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
        {
            OnRemoteFailure = context =>
            {
                context.HandleResponse();
                context.Response.Redirect("/Account/Login");
                return Task.FromResult(0);
            }
        };
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
                    b => b.MigrationsAssembly("TutorLizard.Web"));
});

builder.Services.AddTutorLizardDbRepositories<JaszczurContext>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
        name: "ActivateAccount",
        pattern: "{controller=Account}/{action=ActivateAccount}/{activationCode?}");
});

app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(TutorLizard.Blazor.Components._Imports).Assembly);

app.Run();
