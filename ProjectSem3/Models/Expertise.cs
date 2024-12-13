using System.ComponentModel.DataAnnotations;

namespace ProjectSem3.Models
{
    public class Expertise
    {
        [Key]
        public int ExpertiseId { get; set; }
        public string? ExpertiseName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Account>? Account { get; set; }
    }
}
