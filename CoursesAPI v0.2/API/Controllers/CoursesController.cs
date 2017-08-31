using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route ("api/courses")]
    public class CoursesController : Controller {
        private readonly ICoursesService _coursesService;
        public CoursesController(ICoursesService coursesService){
            _coursesService = coursesService;
        }
        // GET api/Courses
        [HttpGet]
        public IActionResult GetCourses () {
            var courses = _coursesService.GetCourses();
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


    }
}