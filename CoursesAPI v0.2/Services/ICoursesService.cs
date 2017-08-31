using System;
using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.ViewModels;

namespace CoursesAPI.Services {
    public interface ICoursesService {
        IEnumerable<CoursesDTO> GetCourses (string semester);
        CoursesDTO GetCourseByID(int courseID);
        CoursesDTO UpdateCourse(int courseID, CourseViewModel updatedCourse);
        bool DeleteCourse(int courseID);
    }
}