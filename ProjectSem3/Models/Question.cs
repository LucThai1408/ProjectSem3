using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectSem3.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public int AccountId { get; set; }
        public Account? Account { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Answer>? Answer { get; set; }
    }
}
