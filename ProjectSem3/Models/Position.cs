using System.ComponentModel.DataAnnotations;

namespace ProjectSem3.Models
{
    public class Position
    {
        [Key]
        public int PositionId { get; set; }
        public string? PositionName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Account>? Account { get; set; }
    }
}
