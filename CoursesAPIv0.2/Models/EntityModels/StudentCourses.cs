/// <summary>
/// Connection between courses and students, stores information about both
/// </summary>
namespace CoursesAPI.Models.EntityModels {
    public class StudentCourses {
        public int ID { get; set; }
        public int studentID { get; set; }
        public int courseID { get; set; }
        public bool deleted { get; set; }
    }
}