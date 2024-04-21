using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data;
public class LizardContext : DbContext
{
    public LizardContext(DbContextOptions options) : base(options)
    {
    }

    DbSet<Ad> Ads { get; set; } = null!;
    DbSet<AdRequest> AdRequests { get; set; } = null!;
    DbSet<Category> Categories { get; set; } = null!;
    DbSet<ScheduleItem> ScheduleItems { get; set; } = null!;
    DbSet<ScheduleItemRequest> ScheduleItemRequests { get; set; } = null!;
    DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(user => user.Ads)
            .WithOne(ad => ad.Tutor)
            .HasForeignKey(ad => ad.TutorId)
            .IsRequired();

        modelBuilder.Entity<User>()
            .HasMany(user => user.ScheduleItemRequests)
            .WithOne(request => request.Student)
            .HasForeignKey(request => request.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(user => user.AdRequests)
            .WithOne(request => request.Student)
            .HasForeignKey(request => request.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ad>()
            .HasOne(ad => ad.Category)
            .WithMany(category => category.Ads)
            .HasForeignKey(ad => ad.CategoryId)
            .IsRequired();

        modelBuilder.Entity<Ad>()
            .HasMany(ad => ad.ScheduleItems)
            .WithOne(item => item.Ad)
            .HasForeignKey(item => item.AdId)
            .IsRequired();

        modelBuilder.Entity<Ad>()
            .HasMany(ad => ad.Requests)
            .WithOne(request => request.Ad)
            .HasForeignKey(request => request.AdId)
            .IsRequired();

        modelBuilder.Entity<ScheduleItem>()
            .HasMany(item => item.Requests)
            .WithOne(request => request.ScheduleItem)
            .HasForeignKey(request => request.ScheduleItemId)
            .IsRequired();


        modelBuilder.Entity<Ad>()
            .Property(ad => ad.Price)
            .HasPrecision(7,2);
    }
}
