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
        public IEnumerable<CoursesDTO> GetCourses () {
            var courses = (from c in _db.Courses 
                            select new CoursesDTO {
                            ID = c.ID,
                            name = c.name
                            }).ToList ();
            return courses;
        }
        public CoursesDTO GetCourseByID(int ID){
            var courseDTO = (from c in _db.Courses 
                            where c.ID == ID
                            select new CoursesDTO {
                            ID = c.ID,
                            name = c.name
                            }).SingleOrDefault();
            
            // If not found???
            return courseDTO;
        }
    }
}