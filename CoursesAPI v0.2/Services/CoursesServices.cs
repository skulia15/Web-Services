using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.EntityModels;
using CoursesAPI.Repositories;

namespace CoursesAPI.Services {
    public class CoursesServices : ICoursesService {
        private readonly ICoursesRepository _repo;
        public CoursesServices (ICoursesRepository repo) {
            _repo = repo;
        }
        public IEnumerable<CoursesDTO> GetCourses (string semester) {
            var courses = _repo.GetCourses(semester);
            return courses;
        }

        public CoursesDTO GetCourseByID (int ID){
            var courses = _repo.GetCourseByID(ID);
            return courses;
        }
        public CoursesDTO UpdateCourse(int courseID, CourseTemplate updatedCourse)
        {
            var course = _repo.UpdateCourse(courseID, updatedCourse);
            return course;
        }
    }
}