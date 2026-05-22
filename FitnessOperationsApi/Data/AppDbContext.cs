using FitnessOperationsApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FitnessOperationsApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Branch> Branches => Set<Branch>();

    public DbSet<Member> Members => Set<Member>();

    public DbSet<MemberBranchAccess> MemberBranchAccesses => Set<MemberBranchAccess>();

    // “How my database should behave”
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*
            Unique Constraints
        */

        modelBuilder.Entity<Branch>().HasIndex(x => x.Email).IsUnique();

        modelBuilder.Entity<Member>().HasIndex(x => x.Email).IsUnique();

        /*
            One-to-Many
            Branch -> Members
        */

        modelBuilder.Entity<Member>()
            .HasOne(x => x.HomeBranch)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.HomeBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        /*
            Many-to-Many
            Member <-> Branch
        */

        modelBuilder.Entity<MemberBranchAccess>()
            .HasOne(x => x.Member)
            .WithMany(x => x.MemberBranchAccesses)
            .HasForeignKey(x => x.MemberId);

        modelBuilder.Entity<MemberBranchAccess>()
            .HasOne(x => x.Branch)
            .WithMany(x => x.MemberBranchAccesses)
            .HasForeignKey(x => x.BranchId);

        /*
            Prevent Duplicate Branch Assignment
        */

        modelBuilder.Entity<MemberBranchAccess>().HasIndex(x => new {x.MemberId, x.BranchId}).IsUnique();
    }
}