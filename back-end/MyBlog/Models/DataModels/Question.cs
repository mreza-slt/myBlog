namespace MyBlog.Models.DataModels
{
    public class Question(string title, long userId, string body)
    {
        public long Id { get; set; }

        public Guid RowId { get; set; }

        public long UserId { get; set; } = userId;

        public string Title { get; set; } = title;

        public string Body { get; set; } = body;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }

        public ICollection<Answer> Answers { get; set; } = [];
    }
}
