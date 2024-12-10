namespace MyBlog.Models.DataModels
{
    public class Like(long userId, long commentId)
    {
        public long Id { get; set; }

        public Guid RowId { get; set; }

        public long UserId { get; set; } = userId;

        public long CommentId { get; set; } = commentId;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;

        public Comment Comment { get; set; } = null!;
    }
}
