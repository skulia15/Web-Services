using CoursesAPI.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
/// <summary>
/// provides appropriate database for each of the repository classes
/// </summary>
namespace CoursesAPI.Repositories {
    public class AppDataContext : DbContext {

        public AppDataContext (DbContextOptions<AppDataContext> options) : base (options) {}

        public DbSet<Course> Courses { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentCourses> StudentCourses { get; set; }

        public DbSet<WaitingList> WaitingList { get; set; }
        public DbSet<CourseTemplate> CourseTemplate { get; set; }
    }
}