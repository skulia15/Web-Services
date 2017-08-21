using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wepo.Models;

namespace wepo.Controllers {
        //[Route("api/students")]
        public class StudentController : Controller {

            // Note: the variable is static such that the data will persist during
            // the execution of the web service. Data will be lost when the service
            // is restarted (and that is OK for now).

            private static List<Student> _students;
            public StudentController () {
                if (_students == null) {
                    _students = new List<Student> {
                        new Student {
                            name = "Skúli Arnarsson",
                            ssn = "2809952079"
                        },
                        new Student {
                            name = "Theodóra Hanna Halldórsdóttir",
                            ssn = "0501942059"
                        }
                    };
                }
            }

                // GET api/students
                [HttpGet]
                [Route ("api/students")]
                public IActionResult GetStudents () {
                    return Ok (_students);
                }

                // POST api/values
                [HttpPost]
                public void Post ([FromBody] string value) { }

                // PUT api/values/5
                [HttpPut ("{id}")]
                public void Put (int id, [FromBody] string value) { }

                // DELETE api/values/5
                [HttpDelete ("{id}")]
                public void Delete (int id) { }
            }
        }