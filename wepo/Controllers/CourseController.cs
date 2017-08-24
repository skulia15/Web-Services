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
                        endDate = DateTime.Now.AddMonths (3),
                        studentList = new List<Student> {
                            new Student {
                                name = "Glúteinn Frímann",
                                ssn = "2809952079"
                            },
                            new Student {
                                name = "Theodóra Hanna Halldórsdóttir",
                                ssn = "0501942059"
                            },
                            new Student {
                                name = "Snorri Sturluson",
                                ssn = "0505092249"
                            }
                        }
                    },
                    new Course {
                        ID = 2,
                        name = "Computer Networks",
                        templateID = "T-514-TSAM",
                        startDate = DateTime.Now,
                        endDate = DateTime.Now.AddMonths (3),
                        studentList = new List<Student> {
                            new Student {
                                name = "DABS",
                                ssn = "0909725949"
                            },
                            new Student {
                                name = "Person Pearson",
                                ssn = "0101992099"
                            },
                            new Student {
                                name = "Skúli Arnarsson",
                                ssn = "0102039249"
                            }
                        }
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

            // GET api/courses/2/studentList
            [HttpGet("{courseID:int}/studentList", Name = "GetStudentsInCourse")]
            public IActionResult GetStudentsInCourse (int courseID) {
                var result = _courses.Where(x => x.ID == courseID).SingleOrDefault();
                // If the course does not exist 
                if(result == null){
                    return NotFound();
                }
                //If the list of students is empty
                if(result.studentList == null){
                    return NoContent();
                }
                return Ok(result.studentList);
            }

            // POST api/courses
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
                if (courseExists != null){
                    //Duplicate exists, Conflict statuscode
                    return StatusCode(409);
                }
                _courses.Add(newCourse);
                int courseID = newCourse.ID;
                return CreatedAtRoute("GetCourseById", new {courseID = newCourse.ID}, newCourse);
            }

            // POST api/courses/5/studentList
            [HttpPost("{courseID:int}/studentList", Name = "AddStudentToCourse")]
            public IActionResult AddStudentToCourse (int courseID, [FromBody] Student addStudent) {
                // Check if course exists
                var course = _courses.Where(x => x.ID == courseID).SingleOrDefault();
                if(course == null){
                    return BadRequest();
                }
                // Validate the new course to add
                if(!ModelState.IsValid){
                    return StatusCode(412);
                }
                // Check for duplicates in the Studentlist
                var studentExists = course.studentList.SingleOrDefault(x => x.ssn == addStudent.ssn);
                if (studentExists != null){
                    //Duplicate exists
                    return StatusCode(412);
                }
                
                //Add the student to the student list of the course
                course.studentList.Add(addStudent);
                return CreatedAtRoute("GetStudentsInCourse", new {ssn = addStudent.ssn}, addStudent);
            }

            // Update a course
            // PUT api/courses/2
            [HttpPut ("{CourseID}", Name = "UpdateCourse")]
            public IActionResult UpdateCourse (int courseID, [FromBody] Course updatedCourse) {
                //Find the course to update
                var oldCourse = _courses.Where(x => x.ID == courseID).SingleOrDefault();
                if(oldCourse == null){
                    return NotFound();
                }
                //Validate updated course
                if(!ModelState.IsValid){
                    return StatusCode(412);
                }
                _courses.Remove(oldCourse);
                _courses.Add(updatedCourse);
                return CreatedAtRoute("GetCourseById", new {courseID = updatedCourse.ID}, updatedCourse);
            }

            // DELETE api/courses/5
            [HttpDelete ("{courseID}", Name = "DeleteCourse")]
            public IActionResult DeleteCourse (int courseID) { 
                var removeCourse = _courses.Where(x => x.ID == courseID).SingleOrDefault();
                // Check if the course to be deleted exists
                if(removeCourse == null){
                    return NotFound();
                }
                _courses.Remove(removeCourse);
                return NoContent();
            }
        }
    }