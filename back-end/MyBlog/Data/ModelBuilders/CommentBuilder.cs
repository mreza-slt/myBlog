using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Models.DataModels;

namespace MyBlog.Data.ModelBuilders
{
    public class CommentBuilder
    {
        public static void Build(EntityTypeBuilder<Comment> entity)
        {
            entity.ToTable("Comment");
            entity.HasKey(x => x.Id);

            entity.HasIndex(e => e.RowId);
            entity.Property(e => e.RowId)
                        .HasDefaultValueSql("newsequentialid()");

            entity.HasOne(d => d.User)
                .WithMany(d => d.Comments)
                .HasForeignKey(d => d.UserId)
                .HasPrincipalKey(d => d.Id)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Post)
             .WithMany(d => d.Comments)
             .HasForeignKey(d => d.PostId)
             .HasPrincipalKey(d => d.Id)
             .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.CommentParent)
             .WithMany(d => d.Children)
             .HasForeignKey(d => d.ParentId)
             .HasPrincipalKey(d => d.Id)
             .OnDelete(DeleteBehavior.NoAction);
        }
    }
}