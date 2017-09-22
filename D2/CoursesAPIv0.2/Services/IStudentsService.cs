using System;
using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
/// <summary>
/// Interface for student service, each function in studentService must also be here.
/// </summary>
namespace CoursesAPI.Services {
    public interface IStudentsService {
        IEnumerable<StudentsDTO> GetStudents ();
        StudentsDTO GetStudentBySSN(string ssn);
    }
}