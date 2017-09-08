/// <summary>
/// A template for the courses
/// </summary>
namespace CoursesAPI.Models.EntityModels {
    public class CourseTemplate {
        public int ID { get; set; }
        // Example: "Vefþjónustur"
        public string Name { get; set; }
        // Example: "T-514-VEFT"
        public string CourseID { get; set; }
        // Example: 2016-08-17
    }
}