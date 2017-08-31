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