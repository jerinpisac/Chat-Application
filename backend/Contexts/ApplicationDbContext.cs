using System;
using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Users> Users { get; set; }
    public DbSet<Friends> Friends { get; set; }
    public DbSet<FriendRequest> FriendRequest { get; set; }
    public DbSet<Messages> Messages { get; set; }
    public DbSet<Groups> Groups { get; set; }
    public DbSet<GroupMessages> GroupMessages { get; set; }
    public DbSet<GroupMembers> GroupMembers { get; set; }
    public DbSet<GroupMessagesSeen> GroupMessagesSeen { get; set; }
    public DbSet<Notifications> Notifications { get; set; }
    public DbSet<VideoCalls> VideoCalls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
       
        modelBuilder.Entity<Notifications>().HasOne(n => n.Users).WithMany().HasForeignKey(n => n.UserId2);
        modelBuilder.Entity<Notifications>().HasOne(n => n.Groups).WithMany().HasForeignKey(n => n.GroupId);
        modelBuilder.Entity<Friends>().HasKey(uta => new { uta.UserId1, uta.UserId2 });
        modelBuilder.Entity<FriendRequest>().HasKey(uta => new { uta.UserId1, uta.UserId2 });
        modelBuilder.Entity<GroupMembers>().HasKey(ta => new { ta.GroupId, ta.UserId, ta.JoinedAt });
        modelBuilder.Entity<GroupMessagesSeen>().HasKey(tf => new { tf.GroupMessageId, tf.UserId });
    }
}
