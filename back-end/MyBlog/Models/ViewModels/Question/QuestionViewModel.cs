using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.ViewModels.Question
{
    public class QuestionViewModel
    {
        [Required(ErrorMessage = "لطفا عنوان پرسش را وارد کنید")]
        public string Title { get; set; }

        [Required(ErrorMessage = "لطفا متن پرسش را وارد کنید")]
        public string Body { get; set; }
    }
}
