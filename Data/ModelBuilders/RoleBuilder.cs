using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.ModelBuilders
{
    public static class RoleBuilder
    {
        public static void Build(EntityTypeBuilder<Models.DataModels.Role> entity)
        {
            entity.ToTable("UserRole");

            entity.HasIndex(e => e.RowId, "IX_UserRole_RowId");

            entity.Property(e => e.RowId).HasDefaultValueSql("(newsequentialid())");
        }
    }
}