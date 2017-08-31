using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
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
    }
}