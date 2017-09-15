using System.Collections.Generic;
using System.Linq;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Exceptions;
using CoursesAPI.Services.Models.Entities;

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

			_courseInstances      = _uow.GetRepository<CourseInstance>();
			_courseTemplates      = _uow.GetRepository<CourseTemplate>();
			_teacherRegistrations = _uow.GetRepository<TeacherRegistration>();
			_persons              = _uow.GetRepository<Person>();
		}

		/// <summary>
		/// You should implement this function, such that all tests will pass.
		/// </summary>
		/// <param name="courseInstanceID">The ID of the course instance which the teacher will be registered to.</param>
		/// <param name="model">The data which indicates which person should be added as a teacher, and in what role.</param>
		/// <returns>Should return basic information about the person.</returns>
		public PersonDTO AddTeacherToCourse(int courseInstanceID, AddTeacherViewModel model)
		{
			var course = (from c in _courseInstances.All() where courseInstanceID == c.ID select c).SingleOrDefault();
			if(course == null){
				throw new AppObjectNotFoundException();
			}
			var MainTeacher = TeacherType.MainTeacher;

			//Check if already registered as a teacher
			var teacher = (from t in _teacherRegistrations.All() 
						where t.CourseInstanceID == courseInstanceID 
						&& t.SSN == model.SSN select t).FirstOrDefault();	

			
			if(teacher != null){
				// Teacher already registered as teacher in this course
				throw new AppValidationException("Teacher already registered");				
			}
			
			// If trying to add a main teacher
			// Check number of main teachers
			if(model.Type == MainTeacher){
				int numberOfMainTeachers = (from t in _teacherRegistrations.All() 
											where t.CourseInstanceID == courseInstanceID 
											&& t.Type == MainTeacher select t).Count();
				if(numberOfMainTeachers >= 1){
					// Cannot add main teacher if there already is one
					throw new AppValidationException("Cannot add main teacher if there already is one");
				}
			}

			TeacherRegistration newTeacher = new TeacherRegistration{
				CourseInstanceID = courseInstanceID,
				SSN = model.SSN,
				Type = model.Type
			};

			_teacherRegistrations.Add(newTeacher);

			var getName = (from p in _persons.All() where p.SSN == model.SSN select p).SingleOrDefault();
			if(getName == null){
				// Person is not registered
				throw new AppObjectNotFoundException();			
			}

			PersonDTO newPerson = new PersonDTO{
				SSN = getName.SSN,
				Name = getName.Name
			};
			_uow.Save();
			
			return newPerson;
		}

		/// <summary>
		/// You should write tests for this function. You will also need to
		/// modify it, such that it will correctly return the name of the main
		/// teacher of each course.
		/// </summary>
		/// <param name="semester"></param>
		/// <returns></returns>
		public List<CourseInstanceDTO> GetCourseInstancesBySemester(string semester = null)
		{
			if (string.IsNullOrEmpty(semester))
			{
				semester = "20153";
			}

			var courses = (from c in _courseInstances.All()
				join ct in _courseTemplates.All() on c.CourseID equals ct.CourseID
				where c.SemesterID == semester
				select new CourseInstanceDTO
				{
					Name               = ct.Name,
					TemplateID         = ct.CourseID,
					CourseInstanceID   = c.ID,
					MainTeacher        = GetMainTeacherName(semester) // Hint: it should not always return an empty string!
				}).ToList();

			return courses;
		}
		private string GetMainTeacherName(string semester){
			// Get the course the specific course we are making when calling this function
			var course = (from c in _courseInstances.All()
						join ct in _courseTemplates.All() on c.CourseID equals ct.CourseID
						where c.SemesterID == semester select c).SingleOrDefault();
			// Get the teachers that is teaching that course
			var teacher = (from t in _teacherRegistrations.All() 
						where t.CourseInstanceID == course.ID 
						&& t.Type == TeacherType.MainTeacher
						select t).SingleOrDefault();
			if(teacher == null){
				return "";
			}
			
			var person = (from p in _persons.All() 
						where p.SSN == teacher.SSN
						select p).SingleOrDefault();
			if(person == null){
				return "";
			}
			return person.Name;
		}
	}
}
