using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.ViewModels;
/// <summary>
/// Course repo interface, each function in the CourseRepo must also be here.
/// </summary>
namespace CoursesAPI.Repositories {
    public interface ICoursesRepository {
        IEnumerable<CoursesDTO> GetCourses(string semester);
        CoursesDTO GetCourseByID(int ID);
        CoursesDTO UpdateCourse(int courseID, CourseViewModel updatedCourse);
        bool AddStudentToCourse(int courseID,int studentID);
        bool DeleteCourse(int courseID);
        List<StudentsDTO> GetStudentsInCourse(int courseID);
    }
}