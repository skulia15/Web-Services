using System;
using System.Collections.Generic;
using System.Linq;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.EntityModels;

namespace CoursesAPI.Repositories {
    public class CoursesRepository : ICoursesRepository {
        private readonly AppDataContext _db;

        public CoursesRepository (AppDataContext db) {
            _db = db;
        }
        // If no semester is provided in the query (i.e. /api/courses), the current semester should be used
        public IEnumerable<CoursesDTO> GetCourses (string semester) {
            string givenSemester;
            if(semester == null){
                givenSemester = "20173";
            }
            else{
                givenSemester = semester;
            }

            var courses = (from c in _db.Courses 
                            where c.semester == givenSemester
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
            
            // If not found???
            return courseDTO;
        }

        public CoursesDTO UpdateCourse(int courseID, CourseTemplate updatedCourse)
        {
            var course = (from c in _db.Courses 
                            where c.templateID == updatedCourse.courseID
                            && c.semester == updatedCourse.semester
                            select c).SingleOrDefault();

            course.startDate = updatedCourse.startDate;
            course.endDate = updatedCourse.endDate;
            
            if(course == null){
                return null;
            }

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
    }
}