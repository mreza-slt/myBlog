namespace MyBlog.Models.DataModels
{
    public class Comment(long userId, long postId, string content, long? parentId)
    {
        public long Id { get; set; }

        public Guid RowId { get; set; }

        public long UserId { get; set; } = userId;

        public long PostId { get; set; } = postId;

        public string Content { get; set; } = content;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public long? ParentId { get; set; } = parentId;

        public ICollection<Comment> Children { get; set; } = [];

        public User User { get; set; } = null!;

        public Comment CommentParent { get; set; } = null!;

        public Post Post { get; set; } = null!;

        public ICollection<Like> Likes { get; set; } = [];
    }
}
