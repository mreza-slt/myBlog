using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.ModelBuilders
{
    public static class ConfirmCodeBuilder
    {
        public static void Build(EntityTypeBuilder<Models.DataModels.ConfirmCode> entity)
        {
            entity.ToTable("ConfirmCode");

            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.RowId).IsUnique();
            entity.HasIndex(x => new { x.UserId, x.Code, x.CodeType, });
            entity.HasIndex(x => new { x.UserId, x.CodeType, });

            entity.Property(e => e.RowId)
                        .HasDefaultValueSql("newsequentialid()");

            entity.HasOne(d => d.User)
                .WithMany(d => d.ConfirmCodes)
                .HasForeignKey(d => d.UserId)
                .HasPrincipalKey(d => d.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
