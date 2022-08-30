using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.ModelBuilders
{
    public static class UserTokenBuilder
    {
        public static void Build(EntityTypeBuilder<Models.DataModels.UserToken> entity)
        {
            entity.ToTable("UserToken");

            entity.HasIndex(e => e.RowId, "IX_UserToken_RowId");

            entity.Property(e => e.RowId).HasDefaultValueSql("(newsequentialid())");
        }
    }
}