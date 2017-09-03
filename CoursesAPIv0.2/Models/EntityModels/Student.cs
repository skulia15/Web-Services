/// <summary>
/// Information about students
/// </summary>
namespace CoursesAPI.Models.EntityModels {
    public class Student {
        public int ID { get; set;}
        // Example 2809952079
        public string ssn { get; set; }
        // Example Skúli Arnarsson
        public string name { get; set; }
    }
}