using System.ComponentModel.DataAnnotations;

namespace ProjectSem3.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public string? Image { get; set; }

        public int AccountId { get; set; }
        public Account? Account { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Answer>? Answer { get; set; } // Mối quan hệ với bảng Answer
        public ICollection<Favorite>? Favorite { get; set; } // Mối quan hệ với bảng Favorite
    }
}
