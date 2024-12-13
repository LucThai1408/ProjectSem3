using Microsoft.EntityFrameworkCore;

namespace ProjectSem3.Models
{
    public class DoctorContext : DbContext
    {
        public DoctorContext(DbContextOptions<DoctorContext> options) : base(options)
        {

        }

        public DbSet<Account> Account { get; set; }
        public DbSet<Category> Categorie { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Favorite> Favorite { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Expertise> Expertise { get; set; }
        public DbSet<Level> Level { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mối quan hệ nhiều-một cho Favorite với Account, Post, Question
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Account)
                .WithMany(a => a.Favorite)
                .HasForeignKey(f => f.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Post)
                .WithMany(p => p.Favorite)
                .HasForeignKey(f => f.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Question)
                .WithMany(q => q.Favorite)
                .HasForeignKey(f => f.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answer)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
