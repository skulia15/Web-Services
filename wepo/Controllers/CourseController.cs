using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wepo.Models;

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
                [HttpGet]
                [Route ("")]
                public IActionResult GetCourses () {
                    return Ok (_courses);
                }

                // GET api/courses/5
                [HttpGet]
                [Route("{courseID}")]
                public IActionResult GetCourseByID (int courseID) {
                    var result = _courses.Where(x => x.ID == courseID).SingleOrDefault();
                    if(result == null){
                        return NotFound();
                    }
                    return Ok(result);
                }

                // POST api/values
                [HttpPost]
                [Route("")]
                public void Post ([FromBody] string value) { }

                // PUT api/values/5
                [HttpPut ("{id}")]
                public void Put (int id, [FromBody] string value) { }

                // DELETE api/values/5
                [HttpDelete ("{id}")]
                public void Delete (int id) { }
            }
        }