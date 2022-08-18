using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.ModelBuilders
{
    public static class UserRoleBuilder
    {
        public static void Build(EntityTypeBuilder<Models.DataModels.UserRole> entity)
        {
            entity.ToTable("UserInRole");

            entity.HasIndex(e => e.RowId, "IX_UserInRole_RowId");

            entity.Property(e => e.RowId).HasDefaultValueSql("(newsequentialid())");
        }
    }
}