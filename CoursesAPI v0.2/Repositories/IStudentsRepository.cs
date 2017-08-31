using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;

namespace CoursesAPI.Repositories {
    public interface IStudentsRepository {
        IEnumerable<StudentsDTO> GetStudents();
        StudentsDTO GetStudentBySSN(string ssn);
    }
}