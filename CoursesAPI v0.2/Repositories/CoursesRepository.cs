using System;
using System.Collections.Generic;
using System.Linq;
using CoursesAPI.Models.DTOModels;

namespace CoursesAPI.Repositories {
    public class CoursesRepository : ICoursesRepository {
        private readonly AppDataContext _db;

        public CoursesRepository (AppDataContext db) {
            _db = db;
        }
        // If no semester is provided in the query (i.e. /api/courses), the current semester should be used
        public IEnumerable<CoursesDTO> GetCourses () {
            var courses = (from c in _db.Courses 
                            where c.semester.EndsWith("3")
                            select new CoursesDTO {
                            ID = c.ID,
                            name = c.name,
                            templateID = c.templateID
                            }).ToList ();
            return courses;
        }
        public CoursesDTO GetCourseByID(int ID){
            var courseDTO = (from c in _db.Courses 
                            where c.ID == ID
                            select new CoursesDTO {
                            ID = c.ID,
                            name = c.name,
                            templateID = c.templateID
                            }).SingleOrDefault();
            
            // If not found???
            return courseDTO;
        }
    }
}