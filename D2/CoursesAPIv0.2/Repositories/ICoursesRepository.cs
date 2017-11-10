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
        bool AddStudentToWaitingList(int newStudent, int courseId);
        bool CreateCourse(CourseViewModel newCourse);
        CoursesDTO UpdateCourse(int courseID, CourseViewModel updatedCourse);
        StudentsDTO AddStudentToCourse(int studentID, int courseID);
        bool DeleteCourse(int courseID);
        List<StudentsDTO> GetStudentsInCourse(int courseID);
        bool CheckIfStudentExists(int studentID);
        List<StudentsDTO> GetWaitingList(int courseID);
        int checkRegistered(int courseID);
        int getMaxInCourse(int courseID);
        bool checkIfAlreadyRegistered(int studentID, int courseID);
        bool checkIfAlreadyOnWaitingList(int studentID, int courseID);
        bool removeFromWaitingList(int studentID, int courseID);
        bool removeStudentFromCourse(int courseID, string ssn);
        bool CheckIfStudentExistsBySSN(string ssn);
        StudentsDTO getStudentBySSN(string ssn);
        int getStudentID(string ssn);
    }
}