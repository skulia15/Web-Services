using System;
using System.Collections.Generic;
using System.Linq;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Exceptions;
using CoursesAPI.Services.Models.Entities;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CoursesAPI.Services.CoursesServices
{
    public class CoursesServiceProvider
    {
        private readonly IUnitOfWork _uow;

        private readonly IRepository<CourseInstance> _courseInstances;
        private readonly IRepository<TeacherRegistration> _teacherRegistrations;
        private readonly IRepository<CourseTemplate> _courseTemplates;
        private readonly IRepository<Person> _persons;

        public CoursesServiceProvider(IUnitOfWork uow)
        {
            _uow = uow;

            _courseInstances = _uow.GetRepository<CourseInstance>();
            _courseTemplates = _uow.GetRepository<CourseTemplate>();
            _teacherRegistrations = _uow.GetRepository<TeacherRegistration>();
            _persons = _uow.GetRepository<Person>();
        }
       
    

        /// <summary>
        /// You should implement this function, such that all tests will pass.
        /// </summary>
        /// <param name="courseInstanceID">The ID of the course instance which the teacher will be registered to.</param>
        /// <param name="model">The data which indicates which person should be added as a teacher, and in what role.</param>
        /// <returns>Should return basic information about the person.</returns>
        public PersonDTO AddTeacherToCourse(int courseInstanceID, AddTeacherViewModel model)
        {
            // TODO: implement this logic!
            return null;
        }
        public string parseHeader(String header)
        {
            var split = header.Split(',').Select(StringWithQualityHeaderValue.Parse)
            .OrderByDescending(x => x.Quality);
            var lang = split.ElementAt(0).ToString();
            var finalSlipt = lang.Split(';');

            return finalSlipt[0].ToString();
        }
        /// <summary>
        /// You should write tests for this function. You will also need to
        /// modify it, such that it will correctly return the name of the main
        /// teacher of each course.
        /// </summary>
        /// <param name="semester"></param>
        /// <returns></returns>
        public Envelope<CourseInstanceDTO> GetCourseInstancesBySemester(String languageHeader,string semester,int pageNumber)
        {
            int pageSize = 2;
            var templates = _courseTemplates.All().ToList();
            
            if (string.IsNullOrEmpty(semester))
            {
                semester = "20153";
            }
            List<CourseInstanceDTO> courses = null;
            var lang = parseHeader(languageHeader);
            if(lang == "en"){
                courses = (from c in _courseInstances.All()
                            join ct in _courseTemplates.All() on c.CourseID equals ct.CourseID orderby c.ID
                            where c.SemesterID == semester
                            select new CourseInstanceDTO
                            {
                                Name = ct.NameEN,
                                TemplateID = ct.CourseID,
                                CourseInstanceID = c.ID,
                                MainTeacher = "" // Hint: it should not always return an empty string!
                            }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            else{
                courses = (from c in _courseInstances.All()
                           join ct in _courseTemplates.All() on c.CourseID equals ct.CourseID orderby c.ID
                           where c.SemesterID == semester
                           select new CourseInstanceDTO
                           {
                               Name = ct.Name,
                               TemplateID = ct.CourseID,
                               CourseInstanceID = c.ID,
                               MainTeacher = "" // Hint: it should not always return an empty string!
                           }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            

            int TotalNumberOfItems = courses.Count;
            int pageCount = (int)Math.Ceiling(TotalNumberOfItems / (double)pageSize);

            return new Envelope<CourseInstanceDTO>
            {
                Items = courses,
                TotalPages = pageCount,
                PageSize = pageSize,
                CurrentPage = pageNumber
            };

        }
    }
}
