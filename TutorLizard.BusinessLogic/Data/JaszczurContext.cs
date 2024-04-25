using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data;

public class JaszczurContext : DbContext
{
    public JaszczurContext(DbContextOptions<JaszczurContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>()
            .HasKey(user => user.Id);
        modelBuilder.Entity<User>()
            .Property(user => user.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<User>()
            .Property(user => user.UserType)
            .HasConversion<byte>();

        modelBuilder.Entity<User>()
            .HasMany(user => user.Ads)
            .WithOne(ad => ad.User)
            .HasForeignKey(ad => ad.TutorId)
            .HasPrincipalKey(user => user.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(user => user.AdRequests)
            .WithOne(adrequest => adrequest.User)
            .HasForeignKey(adrequest => adrequest.StudentId)
            .HasPrincipalKey(user => user.Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(user => user.ScheduleItemRequests)
            .WithOne(itemrequest => itemrequest.User)
            .HasPrincipalKey(user => user.Id)
            .HasForeignKey(itemrequest => itemrequest.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ad 
        modelBuilder.Entity<Ad>()
            .HasKey(ad => ad.Id);
        modelBuilder.Entity<Ad>()
            .Property(ad => ad.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Ad>()
            .Property(ad => ad.Price)
            .HasPrecision(7, 2);

        modelBuilder.Entity<Ad>()
            .HasMany(ad => ad.AdRequests)
            .WithOne(adrequest => adrequest.Ad)
            .HasPrincipalKey(ad => ad.Id)
            .HasForeignKey(adrequest => adrequest.AdId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Ad>()
            .HasMany(ad => ad.ScheduleItems)
            .WithOne(item => item.Ad)
            .HasPrincipalKey(ad => ad.Id)
            .HasForeignKey(item => item.AdId)
            .OnDelete(DeleteBehavior.Cascade);

        // AdRequest
        modelBuilder.Entity<AdRequest>()
            .HasKey(adrequest => adrequest.Id);
        modelBuilder.Entity<AdRequest>()
            .Property(adrequest => adrequest.Id)
            .ValueGeneratedOnAdd();

        // Category
        modelBuilder.Entity<Category>()
            .HasKey(category => category.Id);
        modelBuilder.Entity<Category>()
            .Property(category => category.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Category>()
            .HasMany(category => category.Ads)
            .WithOne(ad => ad.Category)
            .HasPrincipalKey(category => category.Id)
            .HasForeignKey(ad => ad.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // ScheduleItem
        modelBuilder.Entity<ScheduleItem>()
            .HasKey(item => item.Id);
        modelBuilder.Entity<ScheduleItem>()
            .Property(item => item.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ScheduleItem>()
            .HasMany(item => item.ScheduleItemRequests)
            .WithOne(itemrequest => itemrequest.ScheduleItem)
            .HasPrincipalKey(item => item.Id)
            .HasForeignKey(itemrequest => itemrequest.ScheduleItemId)
            .OnDelete(DeleteBehavior.Cascade);

        // ScheduleItemRequest
        modelBuilder.Entity<ScheduleItemRequest>()
            .HasKey(itemrequest => itemrequest.Id);
        modelBuilder.Entity<ScheduleItemRequest>()
            .Property(itemrequest => itemrequest.Id)
            .ValueGeneratedOnAdd();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Ad> Ads { get; set; }
    public DbSet<AdRequest> AdRequests { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ScheduleItem> ScheduleItems { get; set; }
    public DbSet<ScheduleItemRequest> ScheduleItemRequests { get; set; }
}
