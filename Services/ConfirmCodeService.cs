using MyBlog.Data;
using MyBlog.Models.DataModels;
using MyBlog.Models.Enums;

namespace MyBlog.Services
{
    public class ConfirmCodeService
    {
        public ConfirmCodeService(BlogDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        private BlogDbContext DbContext { get; }

        public DateTime? FindLastConfirmCodeCreateDate(long userId, CodeType codeType)
        {
            return this.DbContext.ConfirmCodes.Where(x => x.UserId == userId && x.CodeType == codeType)
                .OrderByDescending(x => x.Id).Select(x => x.CreateDate).FirstOrDefault();
        }

        public ConfirmCode? FindConfirmCode(long? userId, int code, CodeType codeType)
        {
            return this.DbContext.ConfirmCodes.FirstOrDefault(x => (userId == null || x.UserId == userId) && x.Code == code && x.CodeType == codeType);
        }
    }
}
