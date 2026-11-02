using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Poucno.Backend.Api.Data.Entities;

namespace Poucno.Backend.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Collection> Collections => Set<Collection>();
    public DbSet<CollectionQuestion> CollectionQuestions => Set<CollectionQuestion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(e => e.Id);
        
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).IsRequired().HasMaxLength(80);
            entity.Property(e => e.Subject).IsRequired().HasMaxLength(80);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.GameId).IsRequired().HasMaxLength(64);

            entity.HasIndex(e => e.OwnerId);

            entity.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Questions)
                .WithOne()
                .HasForeignKey(q => q.CollectionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CollectionQuestion>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Text).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Hint).HasMaxLength(200);
            entity.Property(e => e.CorrectAnswer).IsRequired().HasMaxLength(40);
        });
    }
}
