/// <summary>
/// Information about students
/// </summary>
namespace CoursesAPI.Models.ViewModels {
    public class StudentViewModel {
        public int studentID { get; set;}
        // Example 2809952079
        public string ssn { get; set; }
        // Example Skúli Arnarsson
        public string name { get; set; }
    }
}