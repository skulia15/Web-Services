/// <summary>
/// Information about each course
/// </summary>
namespace CoursesAPI.Models.EntityModels {
    public class Course {
        // Example Web Services
        public string name { get; set; }
        // Example 1
        public int ID { get; set; }
        // Example T-514-VEFT
        public string templateID { get; set; }
        // Example: 2016-08-17
        public System.DateTime startDate { get; set; }
        // Example: 2016-11-08)
        public System.DateTime endDate { get; set; }
        // List of students registered in a particular course
        //public List<Student> studentList { get; set; }
        
        // example: "20171"represents spring 2017, "20172"
        // represents summer 2017 and "20173" represents fall 2017
        public string semester { get; set; }
        // Indicates the maximum number of students that can be enrolled in a course
        public int maxStudents{ get; set; }
    }
}