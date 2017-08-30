using System;
using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;

namespace CoursesAPI.Repositories {
    public class CoursesRepository : ICoursesRepository {
        public IEnumerable<CoursesDTO> GetCourses () {
            return new List<CoursesDTO> {
                new CoursesDTO { ID = 1, name = "Programming" },
                new CoursesDTO { ID = 2, name = "Web Services" }
            };
        }
    }
}