using System;
using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.EntityModels;

namespace CoursesAPI.Services {
    public interface ICoursesService {
        IEnumerable<CoursesDTO> GetCourses (string semester);
        CoursesDTO GetCourseByID(int courseID);
        CoursesDTO UpdateCourse(int courseID, CourseTemplate updatedCourse);
    }
}