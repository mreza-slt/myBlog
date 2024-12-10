namespace MyBlog.Models.DataModels
{
    public class Answer(long questionId, long userId, string body)
    {
        public long Id { get; set; }

        public Guid RowId { get; set; }

        public long UserId { get; set; } = userId;

        public long QuestionId { get; set; } = questionId;

        public string Body { get; set; } = body;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Question Question { get; set; }

        public User User { get; set; }
    }
}
