using Microsoft.AspNetCore.Identity;

namespace MyBlog.Models.DataModels
{
    public class User : IdentityUser<long>
    {
        public User(
            string? title,
            string name,
            string? surname,
            string? userName,
            string? email,
            string phoneNumber,
            string? passwordHash)
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

        public Guid RowId { get; set; }

        public string? Title { get; set; }

        public string Name { get; set; } = null!;

        public string? Surname { get; set; }

        public string? FullName { get; set; }

        public string? Avatar { get; set; }

        public DateTime RegisterDateTime { get; set; }

        public DateTime LastUpdateDateTime { get; set; }
    }
}
