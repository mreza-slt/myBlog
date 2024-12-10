using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Models.DataModels;

namespace MyBlog.Data.ModelBuilders
{
    public class LikeBuilder
    {
        public static void Build(EntityTypeBuilder<Like> entity)
        {
            entity.ToTable("Like");
            entity.HasKey(x => x.Id);

            entity.HasKey(x => new { x.UserId, x.CommentId });

            entity.HasIndex(e => e.RowId);
            entity.Property(e => e.RowId)
                        .HasDefaultValueSql("newsequentialid()");

            entity.HasOne(d => d.User)
                .WithMany(d => d.Likes)
                .HasForeignKey(d => d.UserId)
                .HasPrincipalKey(d => d.Id)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Comment)
             .WithMany(d => d.Likes)
             .HasForeignKey(d => d.CommentId)
             .HasPrincipalKey(d => d.Id)
             .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
