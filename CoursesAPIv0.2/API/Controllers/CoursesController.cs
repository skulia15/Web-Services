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
    [Route("api/courses")]
    public class CoursesController : Controller {
        private readonly ICoursesService _coursesService;
        public CoursesController(ICoursesService coursesService) {
            _coursesService = coursesService;
        }
        // GET /api/courses?semester=20173
        [HttpGet("{semester?}")]
        public IActionResult GetCourses(string semester) {
            var courses = _coursesService.GetCourses(semester);
            return Ok(courses);
        }

        // GET api/courses/1
        [HttpGet("{courseID:int}", Name = "GetCourseByID")]
        public IActionResult GetCourseByID(int courseID) {
            var course = _coursesService.GetCourseByID(courseID);
            if (course == null) {
                return NotFound();
            }
            return Ok(course);
        }

        // GET api/courses/1/students
        [HttpGet("{courseID:int}/students", Name = "GetStudentsInCourse")]
        public IActionResult GetStudentsInCourse(int courseID) {
            // Check if the course exists
            if(!_coursesService.CourseExists(courseID)){
                return NotFound();
            }
            var students = _coursesService.GetStudentsInCourse(courseID);
            if (students == null) {
                return NotFound();
            }
            return Ok(students);
        }

        // PUT api/courses/1
        [HttpPut("{courseID:int}", Name = "UpdateCourse")]
        public IActionResult UpdateCourse(int courseID, [FromBody] CourseViewModel updatedCourse) {
            // Given json object is not valid
            if (!ModelState.IsValid) {
                return StatusCode(412);
            }
            // Check if the course exists
            if(!_coursesService.CourseExists(courseID)){
                return NotFound();
            }

            var course = _coursesService.UpdateCourse(courseID, updatedCourse);
            if (course == null) {
                return NotFound();
            }
            //error check here
            return Ok(course);
        }

        // PUT api/courses/1
        [HttpPost("", Name = "CreateCourse")]
        public IActionResult CreateCourse([FromBody] CourseViewModel newCourse) {
            if (!ModelState.IsValid) {
                return StatusCode(412);
            }
            // Check if succeded
            bool success = _coursesService.CreateCourse(newCourse);
            if (!success) {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE api/courses/1
        [HttpDelete("{courseID:int}", Name = "UpdateCourse")]
        public IActionResult DeleteCourse(int courseID) {
            if(!_coursesService.CourseExists(courseID)){
                return NotFound();
            }
            if (_coursesService.DeleteCourse(courseID)) {
                return NoContent();
            }
            // Success?
            return NotFound();
        }
            
        [HttpPost("{courseID:int}/students")]
        public IActionResult AddStudentToCourse([FromBody] StudentViewModel newStudent, int courseID) {
            // Check if the current number of registered students has reached limit
             if(!_coursesService.CourseExists(courseID)){
                return NotFound();
            }
            bool canAddToCourse = _coursesService.canAddToCourse(courseID);
            if (!canAddToCourse) {
                // Student cannot be added to the course
                // Return Precondition Failed code
                return StatusCode(412);
            }
            bool studentCourse = _coursesService.AddStudentToCourse(newStudent, courseID);
                if (!studentCourse) {
                    // Error adding the student to the course
                    return NotFound();
                }

            return Ok("Student added to the course");
        }

        // /api/courses/1/waitinglist
        [HttpGet("{courseID:int}/waitingList")]
        public IActionResult GetWaitingList(int courseID) {
            if(!_coursesService.CourseExists(courseID)){
                return NotFound();
            }

            List<StudentsDTO> waitingList = _coursesService.GetWaitingList(courseID);
            //Error handle
            //return 200
            return Ok();
        }

        [HttpPost("{courseID:int}/waitinglist")]
        public IActionResult AddStudentToWaitingList([FromBody] StudentViewModel newStudent, int courseID) {
             if(!_coursesService.CourseExists(courseID)){
                return NotFound();
            }
            bool addedToWaitingList = _coursesService.AddStudentToWaitingList(newStudent, courseID);
            return Ok();

        }

        [HttpDelete("{courseId:int}/students/{ssn}")]
        public IActionResult RemoveStudentFromCourse(int courseID, string ssn) {
             if(!_coursesService.CourseExists(courseID)){
                return NotFound();
            }
            bool addedToWaitingList = _coursesService.removeStudentFromCourse(courseID, ssn);
            return Ok();
        }
        

    }
}