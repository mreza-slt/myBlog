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

        public PostMiniViewModel Get(long id)
        {
            PostMiniViewModel? post = this.FindPost(id);

            if(post == null)
            {
                throw new HttpException($"هیچ پستی با شناسه {id} پیدا نشد", nameof(id), HttpStatusCode.NotFound);
            }

            return post;
        }

        public async Task<long> Register(PostViewModel postModel, long userId)
        {
            Dictionary<string, string> errors = new();

            Subject? majorSubject = this.SubjectService.FindSubject(postModel.SubjectId!.Value, SubjectType.MajorSubject);
            if (majorSubject == null)
            {
                errors.Add(nameof(PostViewModel.SubjectId), $"هیچ دسته بندی موضوعات با شناسه {postModel.SubjectId} پیدا نشد");
            }

            Subject? childSubjectId = this.SubjectService.FindSubject(postModel.ChildSubjectId!.Value, SubjectType.ForumSubject);
            if (childSubjectId == null)
            {
                errors.Add(nameof(PostViewModel.ChildSubjectId), $"هیچ موضوعی با شناسه {postModel.ChildSubjectId} پیدا نشد");
            }

            if (errors.Count > 0)
            {
                throw new HttpException(errors, HttpStatusCode.NotFound);
            }

            if (childSubjectId!.ParentId != majorSubject!.Id)
            {
                throw new HttpException($"موضوع {childSubjectId.Name} را نمی توانید با دسته بندی موضوع {majorSubject.Name} ثبت کنید", $"{nameof(PostViewModel.SubjectId)}, {nameof(PostViewModel.ChildSubjectId)}", HttpStatusCode.NotAcceptable);
            }

            bool title = this.IsExistTitle(postModel.Title);
            if (title)
            {
                throw new HttpException($"عنوان {postModel.Title} قبلا برای یک پست ثبت شده است", nameof(PostViewModel.Title), HttpStatusCode.Conflict);
            }

            // Check format and fix size Image
            string? image;
            try
            {
                image = this.ImageService.FixImageSize(postModel.Image, 900);
            }
            catch (ArgumentException)
            {
                throw new HttpException("فرمت عکس صحیح نیست", nameof(PostViewModel.Image), HttpStatusCode.BadRequest);
            }
            catch (FormatException)
            {
                throw new HttpException("فرمت عکس صحیح نیست", nameof(PostViewModel.Image), HttpStatusCode.BadRequest);
            }

            Post post = new(postModel.Title, postModel.Text, image, userId, childSubjectId.Id);
            await this.DbContext.AddAsync(post);
            await this.DbContext.SaveChangesAsync();

            return post.Id;
        }

        // Database Methods
        public PostMiniViewModel[] FindPosts()
        {
            return this.DbContext.Posts.Select(x => new PostMiniViewModel { Id = x.Id, UserName = x.User.Name, Title = x.Title, Text = x.Text, SubjectName = x.Subject.Name, RegisterDateTime = x.RegisterDateTime, NumberOfVisits = x.NumberOfVisits == null ? 0 : x.NumberOfVisits, Image = x.Image, UserAvatar = x.User.Avatar }).ToArray();
        }

        public PostMiniViewModel? FindPost(long id)
        {
            return this.DbContext.Posts.Where(x => x.Id == id).Select(x => new PostMiniViewModel { Id = x.Id, UserName = x.User.Name, Title = x.Title, Text = x.Text, SubjectName = x.Subject.Name, RegisterDateTime = x.RegisterDateTime, NumberOfVisits = x.NumberOfVisits == null ? 0 : x.NumberOfVisits, Image = x.Image, UserAvatar = x.User.Avatar }).FirstOrDefault();
        }

        public bool IsExistTitle(string title)
        {
            return this.DbContext.Posts.Any(x => x.Title == title);
        }
    }
}
