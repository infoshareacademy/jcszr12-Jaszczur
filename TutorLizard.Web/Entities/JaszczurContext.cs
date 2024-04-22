using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Entities;

public class JaszczurContext :DbContext
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
            .Property(user => user.Name)
            .HasMaxLength(20);

        modelBuilder.Entity<User>()
            .Property(user => user.UserType)
            .HasConversion<int>();

        modelBuilder.Entity<User>()
            .Property(user => user.Email)
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(user => user.PasswordHash)
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(user => user.DateCreated)
            .HasColumnType("datetime2");

        modelBuilder.Entity<User>()
            .HasMany(user => user.Ads)
            .WithOne(ad => ad.User)
            .HasForeignKey(ad =>ad.TutorId)
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
            .Property(ad => ad.Subject)
            .HasMaxLength(25);

        modelBuilder.Entity<Ad>()
            .Property(ad => ad.Title)
            .HasMaxLength(25);

        modelBuilder.Entity<Ad>()
            .Property(ad => ad.Description)
            .HasMaxLength(250);

        modelBuilder.Entity<Ad>()
            .Property(ad => ad.Price)
            .HasColumnType("money");

        modelBuilder.Entity<Ad>()
            .Property(ad => ad.Location)
            .HasMaxLength(50);

        modelBuilder.Entity<Ad>()
            .Property(ad => ad.IsRemote)
            .HasColumnType("bit");

        modelBuilder.Entity<Ad>()
            .HasOne(ad => ad.Category)
            .WithMany(category => category.Ads)
            .HasForeignKey(ad => ad.CategoryId)
            .HasPrincipalKey(category => category.Id);

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

        modelBuilder.Entity<AdRequest>()
            .Property(adrequest => adrequest.IsAccepted)
            .HasColumnType("bit");

        modelBuilder.Entity<AdRequest>()
            .Property(adrequest => adrequest.Message)
            .HasMaxLength(150);

        modelBuilder.Entity<AdRequest>()
            .Property(adrequest => adrequest.ReplyMessage)
            .HasMaxLength (150);

        modelBuilder.Entity<AdRequest>()
            .Property(adrequest => adrequest.ReviewDate)
            .HasColumnType("datetime2");

        modelBuilder.Entity<AdRequest>()
            .Property(adrequest => adrequest.IsRemote)
            .HasColumnType("bit");


        // Category
        modelBuilder.Entity<Category>()
            .HasKey(category => category.Id);
        modelBuilder.Entity<Category>()
            .Property(category => category.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Category>()
            .Property(category => category.Name)
            .HasMaxLength(20);

        modelBuilder.Entity<Category>()
            .Property(category => category.Description)
            .HasMaxLength(250);

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
            .Property(item => item.DateTime)
            .HasColumnType("datetime2");

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

        modelBuilder.Entity<ScheduleItemRequest>()
            .Property(itemrequest => itemrequest.IsAccepted)
            .HasColumnType("bit");

        modelBuilder.Entity<ScheduleItemRequest>()
            .Property(itemrequest => itemrequest.IsRemote)
            .HasColumnType("bit");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Ad> Ads { get; set; }
    public DbSet<AdRequest> AdRequests { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ScheduleItem> ScheduleItems { get; set; }
    public DbSet<ScheduleItemRequest> ScheduleItemRequests { get; set; }
}
