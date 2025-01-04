using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace ProjectSem3.Models
{
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }
        [Required]
        public string? Title { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Post>? Post { get; set; }
    }
}
