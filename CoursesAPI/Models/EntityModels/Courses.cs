namespace CoursesAPI.Models.EntityModels
{
    public class Courses
    {
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
    }
}