using System.ComponentModel.DataAnnotations;

namespace ProjectSem3.Models
{
    public class Level
    {
        [Key]
        public int LevelId { get; set; }
        public string? LevelName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Account>? Account { get; set; }
    }
}
