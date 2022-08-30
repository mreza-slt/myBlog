using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.ModelBuilders
{
    public static class RoleClaimBuilder
    {
        public static void Build(EntityTypeBuilder<Models.DataModels.RoleClaim> entity)
        {
            entity.ToTable("UserRoleClaim");

            entity.HasIndex(e => e.RowId, "IX_UserRoleClaim_RowId");

            entity.Property(e => e.RowId).HasDefaultValueSql("(newsequentialid())");
        }
    }
}