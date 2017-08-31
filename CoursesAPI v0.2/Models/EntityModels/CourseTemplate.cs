namespace CoursesAPI.Models.EntityModels {
    public class CourseTemplate {
        // Example: "Vefþjónustur"
        public string name { get; set; }
        // Example: "T-514-VEFT"
        public string courseID { get; set; }
        // Example: 2016-08-17
        public System.DateTime startDate { get; set; }
        // Example: 2016-11-08)
        public System.DateTime endDate { get; set; }

        public string semester { get; set; }
    }
}