using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.ModelBuilders
{
    public static class PostBuilder
    {
        public static void Build(EntityTypeBuilder<Models.DataModels.Post> entity)
        {
            entity.ToTable("Post");

            entity.HasKey(x => x.Id);

            entity.HasIndex(e => e.RowId, "IX_Post_RowId");
            entity.Property(e => e.RowId)
                        .HasDefaultValueSql("newsequentialid()");

            entity.HasOne(d => d.User)
                .WithMany(d => d.Posts)
                .HasForeignKey(d => d.UserId)
                .HasPrincipalKey(d => d.Id)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Subject)
                .WithMany(d => d.Posts)
                .HasForeignKey(d => d.SubjectId)
                .HasPrincipalKey(d => d.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}