/// <summary>
/// Stores StudentID's in relation CourseID's to keep track of students in a waiting list for a course
/// </summary>
namespace CoursesAPI.Models.EntityModels {
    public class WaitingList {
        public int ID { get; set; }
        public int studentID { get; set; }
        public int courseID { get; set; }
    }
}