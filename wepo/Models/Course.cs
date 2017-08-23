using System.ComponentModel.DataAnnotations;
namespace wepo.Models {
    public class Course {
        //Example Web Services
        public string name { get; set; }
        [Required]
        //Example 1
        public int ID { get; set; }
        [Required]
        //Example T-514-VEFT
        public string templateID { get; set; }
        [Required]
        //Example: 2016-08-17
        public System.DateTime startDate { get; set; }
        //Example: 2016-11-08)
        public System.DateTime endDate { get; set; }
        //public List<Student> Students { get; set; }
    }
}