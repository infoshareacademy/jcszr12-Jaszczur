using Microsoft.EntityFrameworkCore;

namespace TutorLizard.BusinessLogic.Entities
{
    public class TutorLizardContext : DbContext
    {
        public TutorLizardContext(DbContextOptions<TutorLizardContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AdEntity> Ads { get; set; }
        public DbSet<AdRequestEntity> AdRequests { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ScheduleItemEntity> ScheduleItems { get; set; }
        public DbSet<ScheduleItemRequestEntity> ScheduleItemRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasKey(user => user.Id);
            modelBuilder.Entity<UserEntity>()
                .Property(user => user.Name)
                .IsRequired();
            modelBuilder.Entity<UserEntity>()
                .Property(user => user.Email)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder.Entity<UserEntity>()
                .Property(user => user.DateCreated)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<AdEntity>()
                .HasKey(ad => ad.Id);
            modelBuilder.Entity<AdEntity>()
                .Property(ad => ad.Subject)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder.Entity<AdEntity>()
                .Property(ad => ad.Title)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<AdEntity>()
                .Property(ad => ad.Description)
                .HasMaxLength(6000)
                .IsRequired();
            modelBuilder.Entity<AdEntity>()
                .Property(ad => ad.Price)
                .IsRequired();
            modelBuilder.Entity<AdEntity>()
                .Property(ad => ad.Location)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<AdEntity>()
                .Property(ad => ad.IsRemote)
                .IsRequired();
            modelBuilder.Entity<AdEntity>()
                .HasOne(ad => ad.UserId)
                .WithMany()
                .HasForeignKey(ad => ad.UserId);
            modelBuilder.Entity<AdEntity>()
                .HasOne(ad => ad.CategoryId)
                .WithMany()
                .HasForeignKey(ad => ad.CategoryId);


            modelBuilder.Entity<AdRequestEntity>()
                .HasKey(adRequest => adRequest.Id);
            modelBuilder.Entity<AdRequestEntity>()
                .Property(ar => ar.IsAccepted)
                .IsRequired();
            modelBuilder.Entity<AdRequestEntity>()
                .Property(ar => ar.Message)
                .HasMaxLength(500)
                .IsRequired();
            modelBuilder.Entity<AdRequestEntity>()
                .Property(ar => ar.ReplyMessage)
                .HasMaxLength(500)
                .IsRequired();
            modelBuilder.Entity<AdRequestEntity>()
                .Property(ar => ar.ReviewDate)
                .ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<AdRequestEntity>()
                .Property(ar => ar.IsRemote)
                .IsRequired();
            modelBuilder.Entity<AdRequestEntity>()
                .HasOne(ar => ar.UserId)
                .WithMany()
                .HasForeignKey(ar => ar.UserId);
            modelBuilder.Entity<AdRequestEntity>()
                .HasOne(ar => ar.AdId)
                .WithMany()
                .HasForeignKey(ar => ar.AdId);


            modelBuilder.Entity<CategoryEntity>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<CategoryEntity>()
                .Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder.Entity<CategoryEntity>()
                .Property(c => c.Description)
                .HasMaxLength(1000)
                .IsRequired();


            modelBuilder.Entity<ScheduleItemEntity>()
                .HasKey(si => si.Id);
            modelBuilder.Entity<ScheduleItemEntity>()
                .Property(si => si.Date)
                .IsRequired();
            modelBuilder.Entity<ScheduleItemEntity>()
                .HasOne(si => si.AdId)
                .WithMany()
                .HasForeignKey(si => si.AdId);

            modelBuilder.Entity<ScheduleItemRequestEntity>()
                .HasKey(request => request.Id);
            modelBuilder.Entity<ScheduleItemRequestEntity>()
                .Property(request => request.IsAccepted)
                .IsRequired();
            modelBuilder.Entity<ScheduleItemRequestEntity>()
                .Property(request => request.IsRemote)
                .IsRequired();
            modelBuilder.Entity<ScheduleItemRequestEntity>()
                .HasOne(request => request.ScheduleItemId)
                .WithMany()
                .HasForeignKey(request => request.ScheduleItemId);
            modelBuilder.Entity<ScheduleItemRequestEntity>()
                .HasOne(request => request.UserId)
                .WithMany()
                .HasForeignKey(request => request.UserId);
        }


    }
}
