using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TutorLizard.BusinessLogic.Data.Repositories.DataBase;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Extensions;
public static class ServiceExtensions
{
    public static void AddDbRepositories<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
    {
        services.AddScoped<IDbRepository<Ad>, DbRepository<Ad, TDbContext>>();
        services.AddScoped<IDbRepository<AdRequest>, DbRepository<AdRequest, TDbContext>>();
        services.AddScoped<IDbRepository<Category>, DbRepository<Category, TDbContext>>();
        services.AddScoped<IDbRepository<ScheduleItem>, DbRepository<ScheduleItem, TDbContext>>();
        services.AddScoped<IDbRepository<ScheduleItemRequest>, DbRepository<ScheduleItemRequest, TDbContext>>();
        services.AddScoped<IDbRepository<User>, DbRepository<User, TDbContext>>();
    }
}
