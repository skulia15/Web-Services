using System;
using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;

namespace CoursesAPI.Services {
    public interface IStudentsService {
        IEnumerable<StudentsDTO> GetStudents ();
        StudentsDTO GetStudentBySSN(string ssn);
    }
}