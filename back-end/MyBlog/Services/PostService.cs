using System.Net;
using MyBlog.Data;
using MyBlog.Models.DataModels;
using MyBlog.Models.Enums.Subject;
using MyBlog.Models.ViewModels.Post;
using MyBlog.Plugins.Exceptions;

namespace MyBlog.Services
{
    public class PostService(
    BlogDbContext dbContext,
    ImageService imageService,
    SubjectService subjectService)
    {
        private BlogDbContext DbContext { get; } = dbContext;

        private ImageService ImageService { get; } = imageService;

        private SubjectService SubjectService { get; } = subjectService;

        public PostMiniViewModel[] GetAll() => this.FindPosts();

        public PostMiniViewModel Get(long id) =>
            this.FindPost(id) ?? throw new HttpException($"هیچ پستی با شناسه {id} پیدا نشد", nameof(id), HttpStatusCode.NotFound);

        public async Task<long> Register(PostViewModel postModel, long userId)
        {
            Dictionary<string, string> errors = [];

            Subject? majorSubject = this.SubjectService.FindSubject(postModel.SubjectId!.Value, SubjectType.MajorSubject);
            if (majorSubject == null)
            {
                errors.Add(nameof(PostViewModel.SubjectId), $"هیچ دسته بندی موضوعات با شناسه {postModel.SubjectId} پیدا نشد");
            }

            Subject? childSubject = this.SubjectService.FindSubject(postModel.ChildSubjectId!.Value, SubjectType.ForumSubject);
            if (childSubject == null)
            {
                errors.Add(nameof(PostViewModel.ChildSubjectId), $"هیچ موضوعی با شناسه {postModel.ChildSubjectId} پیدا نشد");
            }

            if (errors.Count > 0)
            {
                throw new HttpException(errors, HttpStatusCode.NotFound);
            }

            if (childSubject!.ParentId != majorSubject!.Id)
            {
                throw new HttpException($"موضوع {childSubject.Name} نمی‌تواند با دسته‌بندی {majorSubject.Name} مرتبط شود", $"{nameof(PostViewModel.SubjectId)}, {nameof(PostViewModel.ChildSubjectId)}", HttpStatusCode.NotAcceptable);
            }

            bool title = this.IsExistTitle(postModel.Title);
            if (title)
            {
                throw new HttpException($"عنوان {postModel.Title} قبلا برای یک پست ثبت شده است", nameof(PostViewModel.Title), HttpStatusCode.Conflict);
            }

            // Check format and fix size Image
            try
            {
                this.ImageService.FixImageSize(postModel.Image, 900);
            }
            catch (ArgumentException)
            {
                throw new HttpException("فرمت عکس صحیح نیست", nameof(PostViewModel.Image), HttpStatusCode.BadRequest);
            }
            catch (FormatException)
            {
                throw new HttpException("فرمت عکس صحیح نیست", nameof(PostViewModel.Image), HttpStatusCode.BadRequest);
            }

            Post post = new(postModel.Title, postModel.Text, postModel.Image, userId, childSubject.Id);
            await this.DbContext.AddAsync(post);
            await this.DbContext.SaveChangesAsync();

            return post.Id;
        }

        // Database Methods
        private PostMiniViewModel[] FindPosts() =>
             [.. this.DbContext.Posts.Select(x => new PostMiniViewModel
             {
                 Id = x.Id,
                 UserName = x.User.Name,
                 Title = x.Title,
                 Text = x.Text,
                 SubjectName = x.Subject.Name,
                 RegisterDateTime = x.RegisterDateTime,
                 NumberOfVisits = x.NumberOfVisits == null ? 0 : x.NumberOfVisits,
                 Image = x.Image,
                 UserAvatar = x.User.Avatar,
             })];

        private PostMiniViewModel? FindPost(long id) =>
             this.DbContext.Posts
            .Where(x => x.Id == id)
            .Select(x => new PostMiniViewModel
            {
                Id = x.Id,
                UserName = x.User.Name,
                Title = x.Title,
                Text = x.Text,
                SubjectName = x.Subject.Name,
                RegisterDateTime = x.RegisterDateTime,
                NumberOfVisits = x.NumberOfVisits == null ? 0 : x.NumberOfVisits,
                Image = x.Image,
                UserAvatar = x.User.Avatar,
            })
            .FirstOrDefault();

        private bool IsExistTitle(string title) =>
             this.DbContext.Posts.Any(x => x.Title == title);
    }
}
