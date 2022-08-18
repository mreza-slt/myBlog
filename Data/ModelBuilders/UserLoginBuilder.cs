using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.ModelBuilders
{
    public static class UserLoginBuilder
    {
        public static void Build(EntityTypeBuilder<Models.DataModels.UserLogin> entity)
        {
            entity.ToTable("UserLogin");

            entity.HasIndex(e => e.RowId, "IX_UserLogin_RowId");

            entity.Property(e => e.RowId).HasDefaultValueSql("(newsequentialid())");
        }
    }
}