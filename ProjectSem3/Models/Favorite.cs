using System.ComponentModel.DataAnnotations;

namespace ProjectSem3.Models
{
    public class Favorite
    {
        [Key]
        public int FavoriteId { get; set; }

        public int AccountId { get; set; }
        public Account? Account { get; set; } // Liên kết với bảng Account

        public int? PostId { get; set; }
        public Post? Post { get; set; } // Liên kết với bảng Post (nếu yêu thích bài viết)

        public int? QuestionId { get; set; }
        public Question? Question { get; set; } // Liên kết với bảng Question (nếu yêu thích câu hỏi)
        public int? AnswerId { get; set; }
        public Answer? Answer { get; set; } // Liên kết với bảng Question (nếu yêu thích câu hỏi)

        public DateTime CreatedAt { get; set; }
    }
}
