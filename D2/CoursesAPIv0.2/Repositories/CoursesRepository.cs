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
                    semester = c.semester,
                    maxStudents = c.maxStudents
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
            // Check if the course is already active that semester
            var courseExists = (from c in _db.Courses
                                where c.templateID == newCourse.templateID && newCourse.semester == c.semester 
                                select c).SingleOrDefault();
            // Cannot create course if already active
            if(courseExists != null){
                return false;
            }

            // Check if template course exists
            var template = (from ct in _db.CourseTemplate where newCourse.templateID == ct.CourseID select ct).SingleOrDefault();
            if (template == null) {
                return false;
            }
            // Add the course to the DB
            _db.Courses.Add(new Course {
                templateID = newCourse.templateID,
                    semester = newCourse.semester,
                    startDate = newCourse.startDate,
                    endDate = newCourse.endDate,
                    maxStudents = newCourse.maxStudents,
                    name = template.Name
            });
            try {
                _db.SaveChanges();
                return true;
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
            try {
                _db.Courses.Remove(delCourse);
                _db.SaveChanges();
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return true;
        }

        public List<StudentsDTO> GetStudentsInCourse(int courseID) {
            var students = (from s in _db.Students join sc in _db.StudentCourses on s.ID equals sc.studentID where sc.courseID == courseID && sc.deleted == false select new StudentsDTO {
                ssn = s.ssn,
                    name = s.name,
            }).ToList();
            return students;
        }
        public StudentsDTO AddStudentToCourse(int studentID, int courseID) {
            var students = _db.Students.First(x => x.ID == studentID);

            if (students == null) {
                return null;
            } 
            else {
                StudentCourses studentCourse = new StudentCourses {
                    courseID = courseID,
                    studentID = studentID,
                    deleted = false
                };
                // Creata a new ScooberDTO
                StudentsDTO scoober = new StudentsDTO{
                    ssn = students.ssn,
                    name = students.name
                };

                try {
                    _db.StudentCourses.Add(studentCourse);
                    _db.SaveChanges();
                    return scoober;
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
                return null;
            }
        }
        public List<StudentsDTO> GetWaitingList(int courseID) {
            var students = (from s in _db.Students join wl in _db.WaitingList on s.ID equals wl.studentID where wl.courseID == courseID select new StudentsDTO {
                ssn = s.ssn,
                name = s.name
            }).ToList();
            return students;
        }
        public bool AddStudentToWaitingList(int studentID, int courseID) {
            WaitingList waitingList = new WaitingList {
                courseID = courseID,
                studentID = studentID
            };
            
            try {
                _db.WaitingList.Add(waitingList);
                _db.SaveChanges();
                return true;
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
            int registeredInCourse = (from sc in _db.StudentCourses where sc.courseID == courseID && sc.deleted == false select sc).Count();
            return registeredInCourse;
        }

        public int getMaxInCourse(int courseID) {
            // Check for maximum number of students in course
            int maxStudentsInCourse = (from course in _db.Courses where course.ID == courseID select course).SingleOrDefault().maxStudents;
            return maxStudentsInCourse;
        }

        public bool checkIfAlreadyRegistered(int studentID, int courseID) {
            // Find the student in the relational table and if the student is enrolled
            var isRegistered = (from sc in _db.StudentCourses where sc.studentID == studentID &&
                sc.courseID == courseID && (sc.deleted == false) select sc).SingleOrDefault();
            // Not found in the relational table
            if (isRegistered == null) {
                return false;
            }
            return true;
        }
        public bool checkIfAlreadyOnWaitingList(int studentID, int courseID) {
            var isWaiting = (from wl in _db.WaitingList where wl.studentID == studentID &&
                wl.courseID == courseID select wl).SingleOrDefault();
            if (isWaiting == null) {
                return false;
            }
            return true;
        }

        public bool removeFromWaitingList(int studentID, int courseID) {
            var remove = (from s in _db.WaitingList where s.studentID == studentID &&
                s.courseID == courseID select s).SingleOrDefault();
            if (remove == null) {
                return false;
            }

            try {
                _db.WaitingList.Remove(remove);
                _db.SaveChanges();
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return false;

        }

        public bool removeStudentFromCourse(int courseID, string ssn) {
            // Get the student by ssn
            var student = (from s in _db.Students where s.ssn == ssn select s).SingleOrDefault();
            var remove = (from s in _db.StudentCourses where s.studentID == student.ID &&
                s.courseID == courseID select s).SingleOrDefault();
            if (remove == null) {
                // Student not found
                return false;
            }
            remove.deleted = true;
            try {
                _db.SaveChanges();
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public bool CheckIfStudentExistsBySSN(string ssn)
        {
            var students = _db.Students.FirstOrDefault(x => x.ssn == ssn);
            if (students == null) {
                return false;
            }
            return true;
        }

        public StudentsDTO getStudentBySSN(string ssn)
        {
            var student = _db.Students.SingleOrDefault(x => x.ssn == ssn);
            if (student == null) {
                return null;
            }
            StudentsDTO studentDTO = new StudentsDTO{
                ssn = student.ssn,
                name = student.name
            };
            return studentDTO;
        }

        public int getStudentID(string ssn)
        {
            var student = (from s in _db.Students where s.ssn == ssn select s).SingleOrDefault();
            // If student is not found
            if(student == null){
                return -1;
            }
            return student.ID;
        }
    }
}