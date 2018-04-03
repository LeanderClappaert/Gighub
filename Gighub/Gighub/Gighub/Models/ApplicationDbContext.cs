using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gighub.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>()
                .HasRequired(a => a.Gig) //each attendant has a required Gig.
                .WithMany()
                .WillCascadeOnDelete(false);

            /*
             * An application user has many followers.
             * Each follower has a required followee.
             */
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(f => f.Followers) 
                .WithRequired(f => f.Followee)
                .WillCascadeOnDelete(false);

            /*
             * An application user has many followees.
             * Each followee has a required follower.
             */
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(f => f.Followees)
                .WithRequired(f => f.Follower)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserNotification>()
                .HasRequired(n => n.User)
                .WithMany()
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}