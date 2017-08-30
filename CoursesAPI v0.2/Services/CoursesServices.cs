using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Repositories;

namespace CoursesAPI.Services {
    public class CoursesServices : ICoursesService {
        private readonly ICoursesRepository _repo;
        public CoursesServices (ICoursesRepository repo) {
            _repo = repo;
        }
        public IEnumerable<CoursesDTO> GetCourses () {
            var courses = _repo.GetCourses();
            return courses;
        }
    }
}