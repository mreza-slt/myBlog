using System.Net;
using MyBlog.Data;
using MyBlog.Models.DataModels;
using MyBlog.Models.Enums.Subject;
using MyBlog.Models.ViewModels;
using MyBlog.Models.ViewModels.Article;
using MyBlog.Plugins.Exceptions;

namespace MyBlog.Services
{
    public class ArticleService
    {
        public ArticleService(
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

        public ArticleMiniViewModel[] GetAll()
        {
            Article[] articles = this.FindArticles();
            ArticleMiniViewModel[] articleMiniModels = articles.Select(x => new ArticleMiniViewModel { Id = x.Id, UserId = x.UserId, Title = x.Title, Text = x.Text, SubjectId = x.SubjectId, RegisterDateTime = x.RegisterDateTime, NumberOfVisits = x.NumberOfVisits, Avatar = x.Avatar }).ToArray();

            return articleMiniModels;
        }

        public async Task<ResponseMessageViewModel> Register(ArticleViewModel articleModel, long userId)
        {
            Dictionary<string, string> errors = new();

            Subject? majorSubject = this.SubjectService.FindSubject(articleModel.MajorSubjectId!.Value, SubjectType.MajorSubject);
            if (majorSubject == null)
            {
                errors.Add(nameof(ArticleViewModel.MajorSubjectId), $"هیچ دسته بندی موضوعات با شناسه {articleModel.MajorSubjectId} پیدا نشد");
            }

            Subject? forumSubjectId = this.SubjectService.FindSubject(articleModel.ForumSubjectId!.Value, SubjectType.ForumSubject);
            if (forumSubjectId == null)
            {
                errors.Add(nameof(ArticleViewModel.ForumSubjectId), $"هیچ موضوعی با شناسه {articleModel.ForumSubjectId} پیدا نشد");
            }

            if (errors.Count > 0)
            {
                throw new HttpException(errors, HttpStatusCode.NotFound);
            }

            if (forumSubjectId!.ParentId != majorSubject!.Id)
            {
                throw new HttpException($"موضوع {forumSubjectId.Name} را نمی توانید با دسته بندی موضوع {majorSubject.Name} ثبت کنید", $"{nameof(ArticleViewModel.MajorSubjectId)}, {nameof(ArticleViewModel.ForumSubjectId)}", HttpStatusCode.NotAcceptable);
            }

            bool title = this.IsExistTitle(articleModel.Title);
            if (title)
            {
                throw new HttpException($"عنوان {articleModel.Title} قبلا برای یک مقاله ثبت شده است", nameof(ArticleViewModel.Title), HttpStatusCode.Conflict);
            }

            // Check format and fix size Image
            try
            {
                articleModel.Avatar = this.ImageService.FixImageSize(articleModel.Avatar, 900);
            }
            catch (ArgumentException)
            {
                throw new HttpException("فرمت عکس صحیح نیست", nameof(ArticleViewModel.Avatar), HttpStatusCode.BadRequest);
            }
            catch (FormatException)
            {
                throw new HttpException("فرمت عکس صحیح نیست", nameof(ArticleViewModel.Avatar), HttpStatusCode.BadRequest);
            }

            Article article = new(articleModel.Title, articleModel.Text, articleModel.Avatar, userId, forumSubjectId.Id);
            await this.DbContext.AddAsync(article);
            await this.DbContext.SaveChangesAsync();

            return new ResponseMessageViewModel(null, "ثبت مقاله با موفقیت انجام شد");
        }

        // Database Methods
        public Article[] FindArticles()
        {
            return this.DbContext.Articles.ToArray();
        }

        public bool IsExistTitle(string title)
        {
            return this.DbContext.Articles.Any(x => x.Title == title);
        }
    }
}
