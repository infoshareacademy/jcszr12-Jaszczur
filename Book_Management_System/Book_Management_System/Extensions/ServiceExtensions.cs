using Book_Management_System.Data.Models;
using Book_Management_System.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TutorLizard.BusinessLogic.Data.Repositories.DataBase;

namespace Book_Management_System.Extensions;

public static class ServiceExtensions
{
    public static void AddDbRepositories<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddScoped<IDbRepository<User>, DbRepository<User, TDbContext>>();
    }
}
