using DiscordBot.Statistics.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Statistics;

public class StatisticsDbContext(DbContextOptions<StatisticsDbContext> options) : DbContext(options)
{
    public DbSet<Roll> Rolls => Set<Roll>();
    public DbSet<RollDie> RollDice => Set<RollDie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Roll>(roll =>
        {
            roll.HasKey(r => r.Id);
            roll.Property(r => r.RollType).HasConversion<string>();
            roll.HasIndex(r => r.UserId);
            roll.HasIndex(r => r.GuildId);
            roll.HasIndex(r => r.Timestamp);
            roll.HasMany(r => r.Dice)
                .WithOne()
                .HasForeignKey(d => d.RollId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RollDie>(die =>
        {
            die.HasKey(d => d.Id);
            die.HasIndex(d => d.DieType);
        });
    }
}
