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
            if (!_coursesService.CourseExists(courseID)) {
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
            if (!_coursesService.CourseExists(courseID)) {
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
            if (!_coursesService.CourseExists(courseID)) {
                return NotFound();
            }
            if (_coursesService.DeleteCourse(courseID)) {
                return NoContent();
            }
            return NotFound();
        }

        // POST api/courses/1/students
        [HttpPost("{courseID:int}/students")]
        public IActionResult AddStudentToCourse([FromBody] StudentViewModel newStudent, int courseID) {
            // Check if the current number of registered students has reached limit
            if (!_coursesService.CourseExists(courseID)) {
                return NotFound();
            }
            // Get the id of the student
            int studentID = _coursesService.getStudentID(newStudent.ssn);
            // Check if the student exists
            if (!_coursesService.CheckIfStudentExists(studentID)) {
                return NotFound();
            }
            bool canAddToCourse = _coursesService.canAddToCourse(courseID);
            if (!canAddToCourse) {
                // Student cannot be added to the course
                // Return Precondition Failed code
                return StatusCode(412);
            }
            // Return DTOmodel of student
            var studentCourse = _coursesService.AddStudentToCourse(newStudent, courseID);
            if (studentCourse == null) {
                // Error adding the student to the course
                return NotFound();
            }
            return StatusCode(201, studentCourse);
        }

        // /api/courses/1/waitinglist
        [HttpGet("{courseID:int}/waitingList")]
        public IActionResult GetWaitingList(int courseID) {
            if (!_coursesService.CourseExists(courseID)) {
                return NotFound();
            }

            List<StudentsDTO> waitingList = _coursesService.GetWaitingList(courseID);
            // Check if waiting list exists
            if (waitingList == null) {
                return NotFound();
            }
            return Ok(waitingList);
        }

        [HttpPost("{courseID:int}/waitinglist")]
        public IActionResult AddStudentToWaitingList([FromBody] StudentViewModel newStudent, int courseID) {
            // Check if the course exists
            if (!_coursesService.CourseExists(courseID)) {
                return NotFound();
            }
            // Get the ID of the student
            int studentID = _coursesService.getStudentID(newStudent.ssn);
            // Check if the student exists
            if (!_coursesService.CheckIfStudentExists(studentID)) {
                return NotFound();
            }
            bool addedToWaitingList = _coursesService.AddStudentToWaitingList(newStudent, courseID);
            if(!addedToWaitingList){
                return StatusCode(412);
            }
            return Ok();
        }

        [HttpDelete("{courseId:int}/students/{ssn}")]
        public IActionResult RemoveStudentFromCourse(int courseID, string ssn) {
            // Check if the course exists
            if (!_coursesService.CourseExists(courseID)) {
                return NotFound();
            }
            // Get the ID of the student
            int studentID = _coursesService.getStudentID(ssn);
            // Check if the student exists
            if (!_coursesService.CheckIfStudentExists(studentID)) {
                return NotFound();
            }
            bool addedToWaitingList = _coursesService.removeStudentFromCourse(courseID, ssn);
            return Ok();
        }
    }
}