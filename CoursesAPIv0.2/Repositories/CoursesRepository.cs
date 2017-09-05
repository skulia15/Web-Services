using System;
using System.Collections.Generic;
using System.Linq;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.EntityModels;
using CoursesAPI.Models.ViewModels;

/// <summary>
/// This class querys the database as requested by its service class
/// </summary>
namespace CoursesAPI.Repositories {
    public class CoursesRepository : ICoursesRepository {
        private readonly AppDataContext _db;

        public CoursesRepository(AppDataContext db) {
            _db = db;
        }
        // If no semester is provided in the query (i.e. /api/courses), the current semester should be used
        public IEnumerable<CoursesDTO> GetCourses(string semester) {
            var courses = (from c in _db.Courses where c.semester == semester select new CoursesDTO {
                ID = c.ID,
                    name = c.name,
                    templateID = c.templateID,
                    semester = c.semester
            }).ToList();
            return courses;
        }
        public CoursesDTO GetCourseByID(int ID) {
            var courseDTO = (from c in _db.Courses where c.ID == ID select new CoursesDTO {
                ID = c.ID,
                    name = c.name,
                    templateID = c.templateID,
                    semester = c.semester
            }).SingleOrDefault();
            return courseDTO;
        }

        public CoursesDTO UpdateCourse(int courseID, CourseViewModel updatedCourse) {
            var course = (from c in _db.Courses where c.ID == courseID select c).SingleOrDefault();
            if (course == null) {
                return null;
            }

            course.startDate = updatedCourse.startDate;
            course.endDate = updatedCourse.endDate;
            course.maxStudents = updatedCourse.maxStudents;

            _db.SaveChanges();

            CoursesDTO DTO = new CoursesDTO() {
                ID = course.ID,
                templateID = course.templateID,
                name = course.name,
                semester = course.semester
            };
            // If not found???
            return DTO;
        }

        public bool CreateCourse(CourseViewModel newCourse) {
            _db.Courses.Add(new Course {
                templateID = newCourse.courseID,
                    semester = newCourse.semester,
                    startDate = newCourse.startDate,
                    endDate = newCourse.endDate,
                    maxStudents = newCourse.maxStudents
            });
            try {
                _db.SaveChanges();
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public bool DeleteCourse(int courseID) {
            var delCourse = (from c in _db.Courses where c.ID == courseID select c).SingleOrDefault();
            if (delCourse == null) {
                return false;
            }

            _db.Courses.Remove(delCourse);
            _db.SaveChanges();
            return true;
        }

        public List<StudentsDTO> GetStudentsInCourse(int courseID) {
            var students = (from s in _db.Students join sc in _db.StudentCourses on s.ID equals sc.studentID where sc.courseID == courseID select new StudentsDTO {
                ssn = s.ssn,
                    name = s.name
            }).ToList();
            return students;
        }
        public bool AddStudentToCourse(StudentViewModel newStudent, int courseID) {
            var students = _db.Students.First(x => x.ID == newStudent.studentID);

            if (students == null) {
                return false;
            } else {
                StudentCourses studentCourse = new StudentCourses {
                    courseID = courseID,
                    studentID = newStudent.studentID
                };

                _db.StudentCourses.Add(studentCourse);
                _db.SaveChanges();
                return true;
            }
        }
        public List<StudentsDTO> GetWaitingList(int courseID) {
            var students = (from s in _db.Students join wl in _db.WaitingList on s.ID equals wl.studentID where wl.courseID == courseID select new StudentsDTO {
                ssn = s.ssn,
                    name = s.name
            }).ToList();
            return students;
        }
        public bool AddStudentToWaitingList(StudentViewModel newStudent, int courseId) {

            WaitingList waitingList = new WaitingList {
                courseID = courseId,
                studentID = newStudent.studentID
            };
            _db.WaitingList.Add(waitingList);
            try {
                _db.SaveChanges();
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return false;

        }
        public bool CheckIfStudentExists(int studentID) {
            var students = _db.Students.FirstOrDefault(x => x.ID == studentID);
            if (students == null) {
                return false;
            }
            return true;

        }

        public int checkRegistered(int courseID) {
            // Check how many students are registered in the course
            int registeredInCourse = (from sc in _db.StudentCourses 
                                        where sc.courseID == courseID 
                                        select sc).Count();
            return registeredInCourse;
        }

        public int getMaxInCourse(int courseID) {
            // Check for maximum number of students in course
            int maxStudentsInCourse = (from course in _db.Courses where course.ID == courseID select course).SingleOrDefault().maxStudents;
            return maxStudentsInCourse;
        }

        public bool checkIfAlreadyRegistered(int studentID, int courseID)
        {
            var isRegistered = (from sc in _db.StudentCourses
                                    where sc.studentID == studentID
                                    && sc.courseID == courseID
                                    select sc).SingleOrDefault();
            if(isRegistered == null){
                return false;
            }
            return true;
        }
    }
}