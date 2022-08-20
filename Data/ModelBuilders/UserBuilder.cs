using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.ModelBuilders
{
    public static class UserBuilder
    {
        public static void Build(EntityTypeBuilder<Models.DataModels.User> entity)
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.RowId, "IX_User_RowId");

            entity.Property(e => e.FullName)
                .HasMaxLength(552)
                .HasComputedColumnSql("(ltrim(rtrim((((isnull([Title],'')+' ')+[Name])+' ')+isnull([Surname],''))))", true);

            entity.Property(e => e.Name).HasMaxLength(250);

            entity.Property(e => e.PhoneNumber).HasColumnType("char(11)");

            entity.Property(e => e.PasswordHash).HasColumnType("nvarchar(MAX)");

            entity.Property(e => e.RowId).HasDefaultValueSql("(newsequentialid())");

            entity.Property(e => e.Surname).HasMaxLength(250);

            entity.Property(e => e.Title).HasMaxLength(50);
        }
    }
}