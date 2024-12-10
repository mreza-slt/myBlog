using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.ModelBuilders
{
    public static class QuestionBuilder
    {
        public static void Build(EntityTypeBuilder<Models.DataModels.Question> entity)
        {
            entity.ToTable("Question");

            entity.HasKey(x => x.Id);

            entity.HasIndex(e => e.RowId);
            entity.Property(e => e.RowId)
                        .HasDefaultValueSql("newsequentialid()");

            entity.HasOne(d => d.User)
             .WithMany(d => d.Questions)
             .HasForeignKey(d => d.UserId)
             .HasPrincipalKey(d => d.Id)
             .OnDelete(DeleteBehavior.NoAction);
        }
    }
}