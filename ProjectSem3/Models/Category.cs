﻿using System.ComponentModel.DataAnnotations;

namespace ProjectSem3.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string? CategoryName { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Topic>? Topic { get; set; } // Mối quan hệ với bảng Topic
    }
}
