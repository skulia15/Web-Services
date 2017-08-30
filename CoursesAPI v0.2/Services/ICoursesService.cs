using System;
using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;

namespace CoursesAPI.Services {
    public interface ICoursesService {
        IEnumerable<CoursesDTO> GetCourses ();
    }
}