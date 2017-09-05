
/// <summary>
/// Transfers a simplified version of course information to display
/// </summary>
namespace CoursesAPI.Models.DTOModels {
    public class CoursesSimpleDTO {
        public string templateID { get; set; }

        public string name { get; set; }

        public string semester { get; set; }

        public int MaxStudents {get; set; }
    }
}