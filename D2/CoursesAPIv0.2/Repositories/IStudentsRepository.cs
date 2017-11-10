using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
/// <summary>
/// Student repo interface, each function in StudentRepo must also be here.
/// </summary>
namespace CoursesAPI.Repositories {
    public interface IStudentsRepository {
        IEnumerable<StudentsDTO> GetStudents();
        StudentsDTO GetStudentBySSN(string ssn);
    }
}