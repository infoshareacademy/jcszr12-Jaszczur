using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Interfaces.Data;
public class LizardContext : DbContext
{
    DbSet<Ad> Ads { get; set; }
    DbSet<AdRequest> AdRequests { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<ScheduleItem> ScheduleItems { get; set; }
    DbSet<ScheduleItemRequest> ScheduleItemRequests { get; set; }
    DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


    }
}
