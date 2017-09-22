using System;
using System.Collections.Generic;
using System.Linq;
using CoursesAPI.Models.DTOModels;
/// <summary>
/// Querys the student database as requested by its service class.
/// </summary>
namespace CoursesAPI.Repositories {
    public class StudentsRepository : IStudentsRepository {
        private readonly AppDataContext _db;

        public StudentsRepository (AppDataContext db) {
            _db = db;
        }
        // If no semester is provided in the query (i.e. /api/students), the current semester should be used
        public IEnumerable<StudentsDTO> GetStudents () {
            var students = (from s in _db.Students 
                            select new StudentsDTO {
                            ssn = s.ssn,
                            name = s.name,
                            }).ToList();
            return students;
        }
        public StudentsDTO GetStudentBySSN(string ssn){
            var student = (from s in _db.Students 
                            where s.ssn == ssn
                            select new StudentsDTO {
                            ssn = s.ssn,
                            name = s.name
                            }).SingleOrDefault();
            
            // If not found???
            return student;
        }
    }
}