using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Repositories;
/// <summary>
/// Sends request to the student repo and returns appropriate information 
/// </summary>
namespace CoursesAPI.Services {
    public class StudentsService : IStudentsService {
        private readonly IStudentsRepository _repo;
        public StudentsService (IStudentsRepository repo) {
            _repo = repo;
        }
        public IEnumerable<StudentsDTO> GetStudents () {
            var students = _repo.GetStudents();
            return students;
        }

        public StudentsDTO GetStudentBySSN (string ssn){
            var students = _repo.GetStudentBySSN(ssn);
            return students;
        }
    }
}