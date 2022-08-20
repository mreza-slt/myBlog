using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.ModelBuilders
{
    public static class SubjectBuilder
    {
        public static void Build(EntityTypeBuilder<Models.DataModels.Subject> entity)
        {
            entity.ToTable("Subject");

            entity.HasKey(x => x.Id);

            entity.HasIndex(e => e.RowId);
            entity.Property(e => e.RowId)
                        .HasDefaultValueSql("newsequentialid()");

            entity.HasOne(d => d.Parent)
                .WithMany(d => d.Children)
                .HasForeignKey(d => d.ParentId)
                .HasPrincipalKey(d => d.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}