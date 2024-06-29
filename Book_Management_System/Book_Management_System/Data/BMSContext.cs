using Book_Management_System.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Book_Management_System.Data;

public class BMSContext : DbContext
{
    public BMSContext(DbContextOptions<BMSContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
}
