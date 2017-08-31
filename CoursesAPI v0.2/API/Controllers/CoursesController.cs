using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesAPI.Models.EntityModels;
using CoursesAPI.Services;
using Microsoft.AspNetCore.Mvc;

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

        // GET api/Courses/1
        [HttpGet ("{courseID:int}", Name = "GetCourseByID")]
        public IActionResult GetCourseByID (int courseID) {
            var course = _coursesService.GetCourseByID(courseID);
            if(course == null){
                return NotFound();
            }
            return Ok(course);
        }

         // PUT api/Courses/1
        [HttpPut ("{courseID:int}", Name = "UpdateCourse")]
        public IActionResult UpdateCourse (int courseID, [FromBody] CourseTemplate updatedCourse) {
            var course = _coursesService.UpdateCourse(courseID, updatedCourse);
            //error check here
            return Ok(course);
        }


    }
}