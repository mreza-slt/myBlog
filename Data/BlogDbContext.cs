using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBlog.Models.DataModels;

namespace MyBlog.Data
{
    public partial class BlogDbContext : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public BlogDbContext()
        {
        }

        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ConfirmCode> ConfirmCodes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(ModelBuilders.UserBuilder.Build);
            modelBuilder.Entity<UserClaim>(ModelBuilders.UserClaimBuilder.Build);
            modelBuilder.Entity<UserRole>(ModelBuilders.UserRoleBuilder.Build);
            modelBuilder.Entity<UserLogin>(ModelBuilders.UserLoginBuilder.Build);
            modelBuilder.Entity<Role>(ModelBuilders.RoleBuilder.Build);
            modelBuilder.Entity<RoleClaim>(ModelBuilders.RoleClaimBuilder.Build);
            modelBuilder.Entity<UserToken>(ModelBuilders.UserTokenBuilder.Build);

            modelBuilder.Entity<ConfirmCode>(ModelBuilders.ConfirmCodeBuilder.Build);
        }
    }
}