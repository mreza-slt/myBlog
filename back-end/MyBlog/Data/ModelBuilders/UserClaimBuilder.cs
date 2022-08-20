using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.ModelBuilders
{
    public static class UserClaimBuilder
    {
        public static void Build(EntityTypeBuilder<Models.DataModels.UserClaim> entity)
        {
            entity.ToTable("UserClaim");

            entity.HasIndex(e => e.RowId, "IX_UserClaim_RowId");

            entity.Property(e => e.RowId).HasDefaultValueSql("(newsequentialid())");
        }
    }
}