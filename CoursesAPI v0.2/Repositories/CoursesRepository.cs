using System;
using System.Collections.Generic;
using System.Linq;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.ViewModels;

namespace CoursesAPI.Repositories {
    public class CoursesRepository : ICoursesRepository {
        private readonly AppDataContext _db;

        public CoursesRepository (AppDataContext db) {
            _db = db;
        }
        // If no semester is provided in the query (i.e. /api/courses), the current semester should be used
        public IEnumerable<CoursesDTO> GetCourses (string semester) {
            var courses = (from c in _db.Courses 
                            where c.semester == semester
                            select new CoursesDTO {
                            ID = c.ID,
                            name = c.name,
                            templateID = c.templateID,
                            semester = c.semester
                            }).ToList ();
            return courses;
        }
        public CoursesDTO GetCourseByID(int ID){
            var courseDTO = (from c in _db.Courses 
                            where c.ID == ID
                            select new CoursesDTO {
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
            if(course == null){
                return null;
            }

            course.startDate = updatedCourse.startDate;
            course.endDate = updatedCourse.endDate;

            _db.SaveChanges();

            CoursesDTO DTO = new CoursesDTO(){
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
            if(delCourse == null){
                return false;
            }
            
            _db.Courses.Remove(delCourse);
            _db.SaveChanges();
            return true;
        }
    }
}