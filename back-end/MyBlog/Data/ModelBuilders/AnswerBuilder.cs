using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.ModelBuilders
{
    public static class AnswerBuilder
    {
        public static void Build(EntityTypeBuilder<Models.DataModels.Answer> entity)
        {
            entity.ToTable("Answer");

            entity.HasKey(x => x.Id);

            entity.HasIndex(e => e.RowId);
            entity.Property(e => e.RowId)
                        .HasDefaultValueSql("newsequentialid()");

            entity.HasOne(d => d.Question)
                  .WithMany(d => d.Answers)
                  .HasForeignKey(d => d.QuestionId)
                  .HasPrincipalKey(d => d.Id)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.User)
             .WithMany(d => d.Answers)
             .HasForeignKey(d => d.UserId)
             .HasPrincipalKey(d => d.Id)
             .OnDelete(DeleteBehavior.NoAction);
        }
    }
}