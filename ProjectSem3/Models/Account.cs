using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectSem3.Models
{
   public class Account
{
    [Key]
    public int AccountId { get; set; }

    [Required]
    public string? FullName { get; set; }

        public string? Avatar { get; set; } = "/images/Avatar.jpg";

    public string? Role { get; set; } = "user";

    [Required]
    [MaxLength(15)]
    [RegularExpression(@"^(03|05|07|08|09|02)[0-9]{8}$" , ErrorMessage = "The phone number format is incorrect, it should follow the Vietnamese format")]
    public string? Phone { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    [NotMapped]
    [Compare("Password", ErrorMessage = "Confirmation password does not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public int? PositionId { get; set; }
    [ForeignKey("PositionId")]
    public Position? Position { get; set; } // Mối quan hệ với bảng Position

    public int? ExpertiseId { get; set; }
    [ForeignKey("ExpertiseId")]
    public Expertise? Expertise { get; set; } // Mối quan hệ với bảng Expertise

    public int? LevelId { get; set; }
    [ForeignKey("LevelId")]
    public Level? Level { get; set; } // Mối quan hệ với bảng Level

    [Range(0, 50, ErrorMessage = "The number of years of experience must be between 0 and 50")]
    [Required(ErrorMessage = "The Experience field is required.")]
    public int Experience { get; set; }

    public int? Status { get; set; }

    public int Online { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ICollection<Favorite>? Favorite { get; set; }
    }

}
