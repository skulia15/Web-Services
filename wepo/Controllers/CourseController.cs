using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wepo.Models;
using System.ComponentModel.DataAnnotations;


namespace wepo.Controllers {
    [Route("api/courses")]
    public class CourseController : Controller {

        // Note: the variable is static such that the data will persist during
        // the execution of the web service. Data will be lost when the service
        // is restarted (and that is OK for now).

        private static List<Course> _courses;
        public CourseController () {
            if (_courses == null) {
                _courses = new List<Course> {
                    new Course {
                        ID = 1,
                        name = "Web services",
                        templateID = "T-514-VEFT",
                        startDate = DateTime.Now,
                        endDate = DateTime.Now.AddMonths (3)
                    },
                    new Course {
                        ID = 2,
                        name = "Computer Networks",
                        templateID = "T-514-TSAM",
                        startDate = DateTime.Now,
                        endDate = DateTime.Now.AddMonths (3)
                    }
                };
            }
        }

            // GET api/courses
            [HttpGet("", Name = "GetCourses")]
            public IActionResult GetCourses () {
                return Ok (_courses);
            }

            // GET api/courses/5
            [HttpGet("{courseID:int}", Name = "GetCourseByID")]
            public IActionResult GetCourseByID (int courseID) {
                var result = _courses.Where(x => x.ID == courseID).SingleOrDefault();
                if(result == null){
                    return NotFound();
                }
                return Ok(result);
            }

            // POST api/values
            [HttpPost("", Name = "CreateCourse")]
            public IActionResult CreateCourse ([FromBody] Course newCourse) {
                if(newCourse == null){
                    return BadRequest();
                }
                // Validate the new course to add
                if(!ModelState.IsValid){
                    return StatusCode(412);
                }
                // Check for duplicates in the list
                var courseExists = _courses.Where(x => x.ID == newCourse.ID);
                if (courseExists == null){
                    //Duplicate exists, Conflict statuscode
                    return StatusCode(422);
                }
                _courses.Add(newCourse);
                int courseID = newCourse.ID;
                return CreatedAtRoute("GetCourseById", new {courseID = newCourse.ID}, newCourse);
            }

            // Update a course
            // PUT api/values/5
            [HttpPut ("{CourseID}", Name = "UpdateCourse")]
            public IActionResult UpdateCourse (int courseID, [FromBody] Course updatedCourse) {
                //Find the course to update
                var oldCourse = _courses.Where(x => x.ID == courseID).SingleOrDefault();
                if(oldCourse == null){
                    return NotFound();
                }
                //Validate updated course here

                _courses.Remove(oldCourse);
                _courses.Add(updatedCourse);

                return Ok();
            }

            // DELETE api/values/5
            [HttpDelete ("{courseID}", Name = "DeleteCourse")]
            public IActionResult DeleteCourse (int courseID) { 
                var removeCourse = _courses.Where(x => x.ID == courseID).SingleOrDefault();
                if(removeCourse == null){
                    return NotFound();
                }
                _courses.Remove(removeCourse);
                return NoContent();
            }
        }
    }