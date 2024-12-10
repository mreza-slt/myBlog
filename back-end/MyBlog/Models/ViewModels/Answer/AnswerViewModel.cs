using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.ViewModels.Answer
{
    public class AnswerViewModel
    {
        [Required(ErrorMessage = "لطفا شناسه پرسش را وارد کنید")]
        public long? QuestionId { get; set; }

        [Required(ErrorMessage = "لطفا متن پاسخ را وارد کنید")]
        public string Body { get; set; }
    }
}
