using System.Net;
using MyBlog.Data;
using MyBlog.Models.DataModels;
using MyBlog.Models.Enums.Subject;
using MyBlog.Models.ViewModels;
using MyBlog.Models.ViewModels.Post;
using MyBlog.Plugins.Exceptions;

namespace MyBlog.Services
{
    public class PostService
    {
        public PostService(
            BlogDbContext dbContext,
            ImageService imageService,
            SubjectService subjectService)
        {
            this.DbContext = dbContext;
            this.ImageService = imageService;
            this.SubjectService = subjectService;
        }

        private BlogDbContext DbContext { get; }

        private ImageService ImageService { get; }

        private SubjectService SubjectService { get; }

        public PostMiniViewModel[] GetAll()
        {
            PostMiniViewModel[] posts = this.FindPosts();

            return posts;
        }

        public async Task<ResponseMessageViewModel> Register(PostViewModel postModel, long userId)
        {
            Dictionary<string, string> errors = new();

            Subject? majorSubject = this.SubjectService.FindSubject(postModel.MajorSubjectId!.Value, SubjectType.MajorSubject);
            if (majorSubject == null)
            {
                errors.Add(nameof(PostViewModel.MajorSubjectId), $"هیچ دسته بندی موضوعات با شناسه {postModel.MajorSubjectId} پیدا نشد");
            }

            Subject? forumSubjectId = this.SubjectService.FindSubject(postModel.ForumSubjectId!.Value, SubjectType.ForumSubject);
            if (forumSubjectId == null)
            {
                errors.Add(nameof(PostViewModel.ForumSubjectId), $"هیچ موضوعی با شناسه {postModel.ForumSubjectId} پیدا نشد");
            }

            if (errors.Count > 0)
            {
                throw new HttpException(errors, HttpStatusCode.NotFound);
            }

            if (forumSubjectId!.ParentId != majorSubject!.Id)
            {
                throw new HttpException($"موضوع {forumSubjectId.Name} را نمی توانید با دسته بندی موضوع {majorSubject.Name} ثبت کنید", $"{nameof(PostViewModel.MajorSubjectId)}, {nameof(PostViewModel.ForumSubjectId)}", HttpStatusCode.NotAcceptable);
            }

            bool title = this.IsExistTitle(postModel.Title);
            if (title)
            {
                throw new HttpException($"عنوان {postModel.Title} قبلا برای یک پست ثبت شده است", nameof(PostViewModel.Title), HttpStatusCode.Conflict);
            }

            // Check format and fix size Image
            try
            {
                postModel.Avatar = this.ImageService.FixImageSize(postModel.Avatar, 900);
            }
            catch (ArgumentException)
            {
                throw new HttpException("فرمت عکس صحیح نیست", nameof(PostViewModel.Avatar), HttpStatusCode.BadRequest);
            }
            catch (FormatException)
            {
                throw new HttpException("فرمت عکس صحیح نیست", nameof(PostViewModel.Avatar), HttpStatusCode.BadRequest);
            }

            Post post = new(postModel.Title, postModel.Text, postModel.Avatar, userId, forumSubjectId.Id);
            await this.DbContext.AddAsync(post);
            await this.DbContext.SaveChangesAsync();

            return new ResponseMessageViewModel(null, "ثبت پست با موفقیت انجام شد");
        }

        // Database Methods
        public PostMiniViewModel[] FindPosts()
        {
            return this.DbContext.Posts.Select(x => new PostMiniViewModel { Id = x.Id, UserName = x.User.Name, Title = x.Title, Text = x.Text, SubjectName = x.Subject.Name, RegisterDateTime = x.RegisterDateTime, NumberOfVisits = x.NumberOfVisits == null ? 0 : x.NumberOfVisits, Image = x.Avatar, UserAvatar = x.User.Avatar }).ToArray();
        }

        public bool IsExistTitle(string title)
        {
            return this.DbContext.Posts.Any(x => x.Title == title);
        }
    }
}
