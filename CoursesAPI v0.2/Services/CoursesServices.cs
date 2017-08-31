using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.EntityModels;
using CoursesAPI.Models.ViewModels;
using CoursesAPI.Repositories;
using System.ComponentModel.DataAnnotations;

namespace CoursesAPI.Services {
    public class CoursesServices : ICoursesService {
        private readonly ICoursesRepository _repo;
        public CoursesServices (ICoursesRepository repo) {
            _repo = repo;
        }
        public IEnumerable<CoursesDTO> GetCourses (string semester) {
            string givenSemester;
            if(semester == null){
                givenSemester = "20173";
            }
            else{
                givenSemester = semester;
            }
            var courses = _repo.GetCourses (givenSemester);
            return courses;
        }

        public CoursesDTO GetCourseByID (int ID) {
            var courses = _repo.GetCourseByID (ID);
            return courses;
        }
        public CoursesDTO UpdateCourse (int courseID, CourseViewModel updatedCourse) {
            //check if modelstate is valid
            var course = _repo.UpdateCourse (courseID, updatedCourse);
            if(course == null){
                return null;
            }
            return course;
        }

        public bool DeleteCourse(int courseID)
        {
             if(_repo.DeleteCourse(courseID)){
                return true;
             }
             return false;
        }

        public List<StudentsDTO> GetStudentsInCourse(int courseID)
        {
            var students = _repo.GetStudentsInCourse(courseID);
            if(students == null){
                return null;
            }
            return students;
        }
    }
}