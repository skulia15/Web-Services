using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesAPI.Services;
using Microsoft.AspNetCore.Mvc;
/// <summary>
/// This class handles request and sends them to its service class
/// </summary>
namespace API.Controllers {
    [Route ("api/students")]
    public class StudentsController : Controller {
        private readonly IStudentsService _StudentsService;
        public StudentsController(IStudentsService studentsService){
            _StudentsService = studentsService;
        }
        // GET api/students
        [HttpGet]
        public IActionResult GetStudents () {
            var student = _StudentsService.GetStudents();
            return Ok(student);
        }
    }
}