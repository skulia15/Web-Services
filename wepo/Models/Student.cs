using System;
using System.ComponentModel.DataAnnotations;

namespace wepo.Models {
    public class Student {
        [Required]
        // Example 2809952079
        public string ssn { get; set; }
        [Required]
        // Example Skúli Arnarsson
        public string name { get; set; }
    }
}