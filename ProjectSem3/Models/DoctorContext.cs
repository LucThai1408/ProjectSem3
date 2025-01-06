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
        public DbSet<Position> Position { get; set; }
        public DbSet<Expertise> Expertise { get; set; }
        public DbSet<Level> Level { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure restrict delete for Question-Answer relationship
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answer)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure restrict delete for Account-Answer relationship
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Account)
                .WithMany()
                .HasForeignKey(a => a.AccountId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        }
    }
}
