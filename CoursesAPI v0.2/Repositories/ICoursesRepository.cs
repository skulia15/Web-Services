using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.EntityModels;

namespace CoursesAPI.Repositories {
    public interface ICoursesRepository {
        IEnumerable<CoursesDTO> GetCourses(string semester);
        CoursesDTO GetCourseByID(int ID);
        CoursesDTO UpdateCourse(int courseID, CourseTemplate updatedCourse);
    }
}