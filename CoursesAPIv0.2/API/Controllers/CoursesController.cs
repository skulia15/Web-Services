using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.EntityModels;
using CoursesAPI.Models.ViewModels;
using CoursesAPI.Services;
using Microsoft.AspNetCore.Mvc;
/// <summary>
/// CoursesController handles requests and sends them to its the service class
/// </summary>
namespace API.Controllers {
    [Route ("api/courses")]
    public class CoursesController : Controller {
        private readonly ICoursesService _coursesService;
        public CoursesController(ICoursesService coursesService){
            _coursesService = coursesService;
        }
        // GET /api/courses?semester=20173
        [HttpGet ("{semester?}")]
        public IActionResult GetCourses (string semester) {
            var courses = _coursesService.GetCourses(semester);
            return Ok(courses);
        }

        // GET api/courses/1
        [HttpGet ("{courseID:int}", Name = "GetCourseByID")]
        public IActionResult GetCourseByID (int courseID) {
            var course = _coursesService.GetCourseByID(courseID);
            if(course == null){
                return NotFound();
            }
            return Ok(course);
        }

        // GET api/courses/1/students
         [HttpGet ("{courseID:int}/students", Name = "GetStudentsInCourse")]
        public IActionResult GetStudentsInCourse (int courseID) {
            var students = _coursesService.GetStudentsInCourse(courseID);
            if(students == null){
                return NotFound();
            }
            return Ok(students);
        }

         // PUT api/courses/1
        [HttpPut ("{courseID:int}", Name = "UpdateCourse")]
        public IActionResult UpdateCourse (int courseID, [FromBody] CourseViewModel updatedCourse) {
            if(!ModelState.IsValid){
                return StatusCode(412);
            }
            var course = _coursesService.UpdateCourse(courseID, updatedCourse);
            if(course == null){
                return NotFound();
            }
            //error check here
            return Ok(course);
        }

         // DELETE api/courses/1
        [HttpDelete ("{courseID:int}", Name = "UpdateCourse")]
        public IActionResult DeleteCourse (int courseID) {
            if(_coursesService.DeleteCourse(courseID)){
                return NoContent();
            }
            // Success?
            return NotFound();
        }
        [HttpPost ("{courseID:int}/{studentID:int}")]
        public IActionResult AddStudentToCourse(int courseID,int studentID)
        {
            
            bool studentCourse = _coursesService.AddStudentToCourse(courseID,studentID);
            if(!studentCourse)
            {
                return NotFound();
            }
            return Ok("Student added to the course");
        }
    }
}