using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.ViewModels;

namespace CoursesAPI.Repositories {
    public interface ICoursesRepository {
        IEnumerable<CoursesDTO> GetCourses(string semester);
        CoursesDTO GetCourseByID(int ID);
        CoursesDTO UpdateCourse(int courseID, CourseViewModel updatedCourse);
        bool DeleteCourse(int courseID);
        List<StudentsDTO> GetStudentsInCourse(int courseID);
    }
}