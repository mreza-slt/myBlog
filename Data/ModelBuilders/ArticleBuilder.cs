using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.ModelBuilders
{
    public static class ArticleBuilder
    {
        public static void Build(EntityTypeBuilder<Models.DataModels.Article> entity)
        {
            entity.ToTable("Article");

            entity.HasKey(x => x.Id);

            entity.HasIndex(e => e.RowId, "IX_Article_RowId");
            entity.Property(e => e.RowId)
                        .HasDefaultValueSql("newsequentialid()");

            entity.HasOne(d => d.User)
                .WithMany(d => d.Articles)
                .HasForeignKey(d => d.UserId)
                .HasPrincipalKey(d => d.Id)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Subject)
                .WithMany(d => d.Articles)
                .HasForeignKey(d => d.SubjectId)
                .HasPrincipalKey(d => d.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}