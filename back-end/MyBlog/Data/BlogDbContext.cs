using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.ModelBuilders;
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

        public virtual DbSet<Post> Posts { get; set; } = null!;

        public virtual DbSet<Category> Categorys { get; set; } = null!;

        public virtual DbSet<Like> Likes { get; set; } = null!;

        public virtual DbSet<Comment> Comments { get; set; } = null!;

        public virtual DbSet<Question> Questions { get; set; } = null!;

        public virtual DbSet<Answer> Answers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(UserBuilder.Build);
            modelBuilder.Entity<UserClaim>(UserClaimBuilder.Build);
            modelBuilder.Entity<UserRole>(UserRoleBuilder.Build);
            modelBuilder.Entity<UserLogin>(UserLoginBuilder.Build);
            modelBuilder.Entity<Role>(RoleBuilder.Build);
            modelBuilder.Entity<RoleClaim>(RoleClaimBuilder.Build);
            modelBuilder.Entity<UserToken>(UserTokenBuilder.Build);

            modelBuilder.Entity<ConfirmCode>(ConfirmCodeBuilder.Build);
            modelBuilder.Entity<Post>(PostBuilder.Build);
            modelBuilder.Entity<Category>(CategoryBuilder.Build);
            modelBuilder.Entity<Like>(LikeBuilder.Build);
            modelBuilder.Entity<Comment>(CommentBuilder.Build);
            modelBuilder.Entity<Question>(QuestionBuilder.Build);
            modelBuilder.Entity<Answer>(AnswerBuilder.Build);
        }
    }
}