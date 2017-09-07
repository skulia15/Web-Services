using System;
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
        public CoursesServices(ICoursesRepository repo) {
            _repo = repo;
        }
        //Gets the courses taught in the given semester.
        //If invalid or no parameters are given returns the courses in the given semester
        public IEnumerable<CoursesDTO> GetCourses(string semester) {
            string givenSemester;
            if (semester == null) {
                givenSemester = "20173";
            } else {
                givenSemester = semester;
            }
            var courses = _repo.GetCourses(givenSemester);
            return courses;
        }

        // Returns a courseDTO with the corresponding courseID
        public CoursesDTO GetCourseByID(int ID) {
            var courses = _repo.GetCourseByID(ID);
            return courses;
        }
        // Finds the course with the given courseID and 
        // modifies the date values for that course
        public CoursesDTO UpdateCourse(int courseID, CourseViewModel updatedCourse) {
            //check if modelstate is valid
            var course = _repo.UpdateCourse(courseID, updatedCourse);
            if (course == null) {
                return null;
            }
            return course;
        }

        public bool CreateCourse(CourseViewModel newCourse) {
            return _repo.CreateCourse(newCourse);

        }

        public bool DeleteCourse(int courseID) {
            if (_repo.DeleteCourse(courseID)) {
                return true;
            }
            return false;
        }
        // Returns a list of studentsDTO's of all the students
        // registered in that course. 
        public List<StudentsDTO> GetStudentsInCourse(int courseID) {
            var students = _repo.GetStudentsInCourse(courseID);
            if (students == null) {
                return null;
            }
            return students;
        }
        // Checks if the course is valid, if so adds the students to
        // that course by creating a new entry in the relational table studentCourses
        public bool AddStudentToCourse(StudentViewModel newStudent, int courseID) {
            // Check if the student exists
            if (!CheckIfStudentExists(newStudent.studentID)) {
                return false;
            }
            // Check if the student is already registered in the course
            bool isRegistered = checkIfAlreadyRegistered(newStudent.studentID, courseID);

            bool isWaiting = IsOnWaitingList(newStudent.studentID, courseID);
            // If student is already registered you cannot add him again
            if (isRegistered) {
                return false;
            }
            //check if the course id is indeed valid
            if (GetCourseByID(courseID) == null) {
                return false;
            }
            if (isWaiting) {
                bool removed = removeFromWaitingList(newStudent.studentID, courseID);
                if (!removed) {
                    return false;
                }
            }

            // Add the student to the course
            bool addStudent = _repo.AddStudentToCourse(newStudent, courseID);
            if (addStudent) {
                return true;
            }
            // Error adding student to the course
            return false;
        }

        public bool removeFromWaitingList(int studentID, int courseID) {
            bool isGone = _repo.removeFromWaitingList(studentID, courseID);
            if (isGone) {
                return true;
            }
            return false;
        }

        public bool CheckIfStudentExists(int studentID) {
            bool studentExists = _repo.CheckIfStudentExists(studentID);
            if (studentExists) {
                return true;
            }
            return false;
        }

        public List<StudentsDTO> GetWaitingList(int courseID) {
            // If course is note found
            var course = GetCourseByID(courseID);
            if (course == null) {
                return null;
            }
            // Get the waiting list for the course
            List<StudentsDTO> waitingList = _repo.GetWaitingList(courseID);
            return waitingList;
        }
        public bool IsOnWaitingList(int studentID, int courseID) {
            bool isWaiting = _repo.checkIfAlreadyOnWaitingList(studentID, courseID);
            if (isWaiting) {
                return true;
            }
            return false;

        }
        public bool AddStudentToWaitingList(StudentViewModel newStudent, int courseId) {
            // Check if the course exists
            if (GetCourseByID(courseId) == null) {
                return false;
            }
            // Check if the student exists
            if (!CheckIfStudentExists(newStudent.studentID)) {
                return false;
            }
            // Check if the student is already on the waiting list
            if (IsOnWaitingList(newStudent.studentID, courseId)) {
                return false;
            }
            // Add the student to the course
            var added = _repo.AddStudentToWaitingList(newStudent, courseId);
            // Student addes successfully
            return true;
        }

        public bool canAddToCourse(int courseID) {
            // Check if course exists
            if (GetCourseByID(courseID) == null) {
                return false;
            }
            // Get how many are registered
            int registered = _repo.checkRegistered(courseID);
            // Get how many can register
            int maxInCourse = _repo.getMaxInCourse(courseID);
            if (registered < maxInCourse) {
                // Maximum is not reached, can add to course
                return true;
            }
            // Maximum is reached
            return false;
        }

        public bool checkIfAlreadyRegistered(int studentID, int courseID) {
            bool isRegistered = _repo.checkIfAlreadyRegistered(studentID, courseID);
            if (isRegistered) {
                return true;
            }
            return false;
        }

        public bool removeStudentFromCourse(int courseID, string ssn) {
            bool removed = _repo.removeStudentFromCourse(courseID, ssn);

            return removed;
        }
    }
}