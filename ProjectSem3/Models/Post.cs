using System.ComponentModel.DataAnnotations;

namespace ProjectSem3.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        [Required]

        public string? Title { get; set; }
        [Required]
        public string? Content { get; set; }
        
        public string? Image { get; set; }

        public int AccountId { get; set; }
        public Account? Accounts { get; set; }

        public int TopicId { get; set; }
        public Topic? Topic { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Favorite>? Favorite { get; set; } // Mối quan hệ với bảng Favorite
    }
}
