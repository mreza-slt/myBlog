using MyBlog.Data;
using MyBlog.Models.DataModels;
using MyBlog.Models.Enums;

namespace MyBlog.Services
{
    public class ConfirmCodeService(BlogDbContext dbContext)
    {
        private BlogDbContext DbContext { get; } = dbContext;

        public DateTime? FindLastConfirmCodeCreateDate(long userId, CodeType codeType)
        {
            return this.DbContext.ConfirmCodes.Where(x => x.UserId == userId && x.CodeType == codeType)
                .OrderByDescending(x => x.Id).Select(x => x.CreateDate).FirstOrDefault();
        }

        public ConfirmCode? FindConfirmCode(long? userId, int code, CodeType codeType)
            => this.DbContext.ConfirmCodes.FirstOrDefault(x => (userId == null || x.UserId == userId) && x.Code == code && x.CodeType == codeType);
    }
}
