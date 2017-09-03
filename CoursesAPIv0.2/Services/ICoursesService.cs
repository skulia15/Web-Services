using System;
using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.ViewModels;
/// <summary>
/// Interface for courseService, each function in courseService must also be here.
/// </summary>
namespace CoursesAPI.Services {
    public interface ICoursesService {
        IEnumerable<CoursesDTO> GetCourses (string semester);
        CoursesDTO GetCourseByID(int courseID);
        CoursesDTO UpdateCourse(int courseID, CourseViewModel updatedCourse);
        bool DeleteCourse(int courseID);
        bool AddStudentToCourse(int courseID,int studentID);
        List<StudentsDTO> GetStudentsInCourse(int courseID);
    }
}