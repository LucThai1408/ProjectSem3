using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectSem3.Models
{
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }
        [Required]
        public string? Content { get; set; }

        public string? Image { get; set; }
        [ForeignKey("QuestionId")]
        public int QuestionId { get; set; }
        public Question? Question { get; set; }

        public int AccountId { get; set; }
        public Account? Account { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
