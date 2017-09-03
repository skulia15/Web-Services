using System;
using System.Collections.Generic;
using System.Linq;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.EntityModels;
using CoursesAPI.Models.ViewModels;

/// <summary>
/// This class querys the database as requested by its service class
/// </summary>
namespace CoursesAPI.Repositories
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly AppDataContext _db;

        public CoursesRepository(AppDataContext db)
        {
            _db = db;
        }
        // If no semester is provided in the query (i.e. /api/courses), the current semester should be used
        public IEnumerable<CoursesDTO> GetCourses(string semester)
        {
            var courses = (from c in _db.Courses
                           where c.semester == semester
                           select new CoursesDTO
                           {
                               ID = c.ID,
                               name = c.name,
                               templateID = c.templateID,
                               semester = c.semester
                           }).ToList();
            return courses;
        }
        public CoursesDTO GetCourseByID(int ID)
        {
            var courseDTO = (from c in _db.Courses
                             where c.ID == ID
                             select new CoursesDTO
                             {
                                 ID = c.ID,
                                 name = c.name,
                                 templateID = c.templateID,
                                 semester = c.semester
                             }).SingleOrDefault();
            return courseDTO;
        }

        public CoursesDTO UpdateCourse(int courseID, CourseViewModel updatedCourse)
        {
            var course = (from c in _db.Courses
                          where c.ID == courseID
                          select c).SingleOrDefault();
            if (course == null)
            {
                return null;
            }

            course.startDate = updatedCourse.startDate;
            course.endDate = updatedCourse.endDate;

            _db.SaveChanges();

            CoursesDTO DTO = new CoursesDTO()
            {
                ID = course.ID,
                templateID = course.templateID,
                name = course.name,
                semester = course.semester
            };
            // If not found???
            return DTO;
        }

        public bool DeleteCourse(int courseID)
        {
            var delCourse = (from c in _db.Courses
                             where c.ID == courseID
                             select c).SingleOrDefault();
            if (delCourse == null)
            {
                return false;
            }

            _db.Courses.Remove(delCourse);
            _db.SaveChanges();
            return true;
        }

        public List<StudentsDTO> GetStudentsInCourse(int courseID)
        {
            var students = (from s in _db.Students
                            join sc in _db.StudentCourses on s.ID equals sc.studentID
                            where sc.courseID == courseID
                            select new StudentsDTO
                            {
                                ssn = s.ssn,
                                name = s.name
                            }).ToList();
            return students;
        }
        public bool AddStudentToCourse(int courseID, int studentID)
        {
            var students = _db.Students.First(x => x.ID == studentID);

            if (students == null)
            {
                return false;
            }
            else
            {
                StudentCourses studentCourse = new StudentCourses
                {
                    courseID = courseID,
                    studentID = studentID
                };

                _db.StudentCourses.Add(studentCourse);
                _db.SaveChanges();
                return true;
            }

        }

    }
}