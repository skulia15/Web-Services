using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.EntityModels;
using CoursesAPI.Models.ViewModels;
using CoursesAPI.Repositories;

/// <summary>
/// This class sends request to the courseRepo and returns appropriate information
/// </summary>
namespace CoursesAPI.Services {
    public class CoursesServices : ICoursesService {
        private readonly ICoursesRepository _repo;
        public CoursesServices (ICoursesRepository repo) {
            _repo = repo;
        }
        //Gets the courses taught in the given semester.
        //If invalid or no parameters are given returns the courses in the given semester
        public IEnumerable<CoursesDTO> GetCourses (string semester) {
            string givenSemester;
            if (semester == null) {
                givenSemester = "20173";
            } else {
                givenSemester = semester;
            }
            var courses = _repo.GetCourses (givenSemester);
            return courses;
        }

        // Returns a courseDTO with the corresponding courseID
        public CoursesDTO GetCourseByID (int ID) {
            var courses = _repo.GetCourseByID (ID);
            return courses;
        }
        // Finds the course with the given courseID and 
        // modifies the date values for that course
        public CoursesDTO UpdateCourse (int courseID, CourseViewModel updatedCourse) {
            //check if modelstate is valid
            var course = _repo.UpdateCourse (courseID, updatedCourse);
            if (course == null) {
                return null;
            }
            return course;
        }

        public bool CreateCourse (CourseViewModel newCourse) {
            return _repo.CreateCourse (newCourse);

        }

        public bool DeleteCourse (int courseID) {
            if (_repo.DeleteCourse (courseID)) {
                return true;
            }
            return false;
        }
        // Returns a list of studentsDTO's of all the students
        // registered in that course. 
        public List<StudentsDTO> GetStudentsInCourse (int courseID) {
            var students = _repo.GetStudentsInCourse (courseID);
            if (students == null) {
                return null;
            }
            return students;
        }
        // Checks if the course is valid, if so adds the students to
        // that course by creating a new entry in the relational table studentCourses
        public bool AddStudentToCourse (int courseID, int studentID) {
            if (GetCourseByID (courseID) == null) {
                return false;
            }
            bool addStudent = _repo.AddStudentToCourse (courseID, studentID);
            if (addStudent) {
                return true;
            }
            return false;
        }

        public List<StudentsDTO> GetWaitingList (int courseID) {
            // If course is note found
            var course = GetCourseByID(courseID);
            if (course == null) {
                return null;
            }
            // Get the waiting list for the course
            List<StudentsDTO> waitingList = _repo.GetWaitingList(courseID);
            return waitingList;
        }
    }
}