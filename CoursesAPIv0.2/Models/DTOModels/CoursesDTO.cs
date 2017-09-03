
/// <summary>
/// Transfers course information
/// </summary>
namespace CoursesAPI.Models.DTOModels {
    public class CoursesDTO {
        public int ID { get; set; }

        public string templateID { get; set; }
        public string name { get; set; }

        public string semester { get; set; }
    }
}