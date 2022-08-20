using Microsoft.AspNetCore.Identity;
using MyBlog.Models.ViewModels.User;

namespace MyBlog.Models.DataModels
{
    public class User : IdentityUser<long>
    {
        public User()
        {
            this.ConfirmCodes = new HashSet<ConfirmCode>();
            this.Articles = new HashSet<Article>();
        }

        public User(
            string? title,
            string name,
            string? surname,
            string? userName,
            string? email,
            string phoneNumber,
            string? passwordHash)
            : this()
        {
            // Inputs user
            this.Title = title;
            this.Name = name;
            this.Surname = surname;
            this.UserName = userName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.PasswordHash = passwordHash;

            // Inputs constructore
            this.EmailConfirmed = false;
            this.PhoneNumberConfirmed = false;
            this.AccessFailedCount = 0;
            this.TwoFactorEnabled = false;
            this.RegisterDateTime = DateTime.Now;
            this.LastUpdateDateTime = DateTime.Now;
        }

        internal static void Copy(ProfileUserViewModel userModel, User user)
        {
            user.Title = userModel.Title;
            user.Name = userModel.Name;
            user.Surname = userModel.Surname;
            user.UserName = !string.IsNullOrEmpty(userModel.UserName) ? userModel.UserName : userModel.PhoneNumber;
            user.Email = userModel.Email;
            user.PhoneNumber = userModel.PhoneNumber;
            user.Avatar = userModel.Avatar;
            user.LastUpdateDateTime = DateTime.Now;
        }

        public Guid RowId { get; set; }

        public string? Title { get; set; }

        public string Name { get; set; } = null!;

        public string? Surname { get; set; }

        public string? FullName { get; set; }

        public string? Avatar { get; set; }

        public DateTime RegisterDateTime { get; set; }

        public DateTime LastUpdateDateTime { get; set; }

        public DateTime? LoginDateTime { get; set; }

        public DateTime? LastLoginDateTime { get; set; }

        public ICollection<ConfirmCode> ConfirmCodes { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
