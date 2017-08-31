using CoursesAPI.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace CoursesAPI.Repositories {
    public class AppDataContext : DbContext {
        public AppDataContext (DbContextOptions<AppDataContext> options) : base (options) {}

        public DbSet<Course> Courses { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentCourses> StudentCourses { get; set; }
    }
}