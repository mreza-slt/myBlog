using System.Net;
using MyBlog.Data;
using MyBlog.Models.DataModels;
using MyBlog.Models.Enums.Category;
using MyBlog.Models.ViewModels.Post;
using MyBlog.Plugins.Exceptions;

namespace MyBlog.Services
{
    public class PostService(
    BlogDbContext dbContext,
    ImageService imageService,
    CategoryService categoryService)
    {
        private BlogDbContext DbContext { get; } = dbContext;

        private ImageService ImageService { get; } = imageService;

        private CategoryService CategoryService { get; } = categoryService;

        public PostMiniViewModel[] GetAll() => this.FindPosts();

        public PostMiniViewModel Get(long id) =>
            this.FindPost(id) ?? throw new HttpException($"هیچ پستی با شناسه {id} پیدا نشد", nameof(id), HttpStatusCode.NotFound);

        public async Task<long> Register(PostViewModel postModel, long userId)
        {
            Dictionary<string, string> errors = [];

            Category? majorCategory = this.CategoryService.FindCategory(postModel.CategoryId!.Value, CategoryType.MajorCategory);
            if (majorCategory == null)
            {
                errors.Add(nameof(PostViewModel.CategoryId), $"هیچ دسته بندی موضوعات با شناسه {postModel.CategoryId} پیدا نشد");
            }

            Category? childCategory = this.CategoryService.FindCategory(postModel.ChildCategoryId!.Value, CategoryType.ForumCategory);
            if (childCategory == null)
            {
                errors.Add(nameof(PostViewModel.ChildCategoryId), $"هیچ موضوعی با شناسه {postModel.ChildCategoryId} پیدا نشد");
            }

            if (errors.Count > 0)
            {
                throw new HttpException(errors, HttpStatusCode.NotFound);
            }

            if (childCategory!.ParentId != majorCategory!.Id)
            {
                throw new HttpException($"موضوع {childCategory.Name} نمی‌تواند با دسته‌بندی {majorCategory.Name} مرتبط شود", $"{nameof(PostViewModel.CategoryId)}, {nameof(PostViewModel.ChildCategoryId)}", HttpStatusCode.NotAcceptable);
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

            Post post = new(postModel.Title, postModel.Text, postModel.Image, userId, childCategory.Id);
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
                 CategoryName = x.Category.Name,
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
                CategoryName = x.Category.Name,
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
