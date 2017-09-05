/// <summary>
/// Handles display of informations
/// </summary>
namespace CoursesAPI.Models.ViewModels
{
    public class CourseViewModel
    {
        public string courseID { get; set; }
        // Example: 2016-08-17
        public System.DateTime startDate { get; set; }
        // Example: 2016-11-08)
        public System.DateTime endDate { get; set; }

        public string semester { get; set; }

        public int maxStudents { get; set; }
    }
}