using System;
using System.Collections.Generic;
using System.Linq;
using CoursesAPI.Models;
using CoursesAPI.Services.Exceptions;
using CoursesAPI.Services.Models.Entities;
using CoursesAPI.Services.CoursesServices;
using CoursesAPI.Tests.MockObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoursesAPI.Tests.Services
{
	[TestClass]
    public class CourseServicesTests
	{
		private MockUnitOfWork<MockDataContext> _mockUnitOfWork;
		private CoursesServiceProvider _service;
		private List<TeacherRegistration> _teacherRegistrations;

		private const string SSN_DABS    = "1203735289";
		private const string SSN_GUNNA   = "1234567890";

		private const string SSN_SKEBOR = "0101010101";
		private const string INVALID_SSN = "9876543210";

		private const string NAME_GUNNA  = "Guðrún Guðmundsdóttir";
		private const string NAME_SKEBOR  = "Guðrún Guðmundsdóttir";

		private const int COURSEID_VEFT_20153 = 1337;
		private const int COURSEID_VEFT_20163 = 1338;
		private const int INVALID_COURSEID    = 9999;

		private const String NonValidSemester = "00000";

		[TestInitialize]
        public void CourseServicesTestsSetup()
		{
			_mockUnitOfWork = new MockUnitOfWork<MockDataContext>();

			#region Persons
			var persons = new List<Person>
			{
				// Of course I'm the first person,
				// did you expect anything else?
				new Person
				{
					ID    = 1,
					Name  = "Daníel B. Sigurgeirsson",
					SSN   = SSN_DABS,
					Email = "dabs@ru.is"
				},
				new Person
				{
					ID    = 2,
					Name  = NAME_GUNNA,
					SSN   = SSN_GUNNA,
					Email = "gunna@ru.is"
				},
				new Person
				{
					ID    = 3,
					Name  = NAME_SKEBOR,
					SSN   = SSN_SKEBOR,
					Email = "skebor@ru.is"
				}
			};
			#endregion

			#region Course templates

			var courseTemplates = new List<CourseTemplate>
			{
				new CourseTemplate
				{
					CourseID    = "T-514-VEFT",
					Description = "Í þessum áfanga verður fjallað um vefþj...",
					Name        = "Vefþjónustur"
				}
			};
			#endregion

			#region Courses
			var courses = new List<CourseInstance>
			{
				new CourseInstance
				{
					ID         = COURSEID_VEFT_20153,
					CourseID   = "T-514-VEFT",
					SemesterID = "20153"
				},
				new CourseInstance
				{
					ID         = COURSEID_VEFT_20163,
					CourseID   = "T-514-VEFT",
					SemesterID = "20163"
				}
			};
			#endregion

			#region Teacher registrations
			_teacherRegistrations = new List<TeacherRegistration>
			{
				new TeacherRegistration
				{
					ID               = 101,
					CourseInstanceID = COURSEID_VEFT_20153,
					SSN              = SSN_DABS,
					Type             = TeacherType.MainTeacher
				}
			};
			#endregion

			_mockUnitOfWork.SetRepositoryData(persons);
			_mockUnitOfWork.SetRepositoryData(courseTemplates);
			_mockUnitOfWork.SetRepositoryData(courses);
			_mockUnitOfWork.SetRepositoryData(_teacherRegistrations);

			// TODO: this would be the correct place to add 
			// more mock data to the mockUnitOfWork!

			_service = new CoursesServiceProvider(_mockUnitOfWork);
		}

		#region GetCoursesBySemester
		/// <summary>
		/// TODO: implement this test, and several others!
		/// </summary>
		[TestMethod]
		public void GetCoursesBySemester_ReturnsEmptyListWhenNoDataDefined()
		{
			// Arrange:
			
			// Act:
			var CoursesThisSemester = _service.GetCourseInstancesBySemester(NonValidSemester);
			// Assert:
			// Should return an empty list instead of null
			Assert.IsNotNull(CoursesThisSemester);
			// The list should be empty
			Assert.IsTrue(CoursesThisSemester.Count() == 0);
		}

		[TestMethod]
		public void GetCoursesBySemester_ReturnsCoursesWhereSemesterIsNotGiven()
		{
			// Arrange:
			
			// Act:
			// Get courses in the semester 20153 because no semester is specified
			var CoursesThisSemester = _service.GetCourseInstancesBySemester();
			// Assert:
			// Should return a course
			Assert.IsNotNull(CoursesThisSemester);
			// Should return a list of courses with one course
			Assert.IsTrue(CoursesThisSemester.Count() == 1);
		}

		[TestMethod]
		public void GetCoursesBySemester_AssertThatTeachersNameIsIncluded()
		{
			// Arrange:
			
			// Act:
			var CoursesThisSemester = _service.GetCourseInstancesBySemester("20153");

			// Assert:
			// Should return a list of courses with one course
			Assert.IsNotNull(CoursesThisSemester);
			foreach(CourseInstanceDTO c in CoursesThisSemester){
			// Check if the name of the main teacher is included
			Assert.IsNotNull(c.MainTeacher);
			}
		}

		[TestMethod]
		public void GetCoursesBySemester_GetCoursesInValidSemester()
		{
			// Arrange:
			String validSemester = "20163";

			// Act:
			var CoursesThisSemester = _service.GetCourseInstancesBySemester(validSemester);

			// Assert:
			// The number of courses should be 1
			 Assert.AreEqual(1, CoursesThisSemester.Count());
			 // The list cannot be empty
			 Assert.IsNotNull(CoursesThisSemester);
		}

		[TestMethod]
		public void GetCoursesBySemester_ReturnsAnEmptyStringWhenCourseHasNoMainTeacher()
		{
			// Arrange:
			var model = new AddTeacherViewModel
			{
				SSN  = SSN_SKEBOR,
				Type = TeacherType.AssistantTeacher
			};
			var dto = _service.AddTeacherToCourse(COURSEID_VEFT_20163, model);
			// Act:
			var CoursesThisSemester = _service.GetCourseInstancesBySemester("20163");

			// Assert:
			// For all courses in this semester
			foreach(CourseInstanceDTO c in CoursesThisSemester){
			// Check if the name of the main teacher is 
			// If there is no main teacher registered it should return an empty string
				Assert.IsNotNull(c.MainTeacher);
				Assert.AreEqual(c.MainTeacher, "");
			}
		}

		#endregion

		#region AddTeacher

		/// <summary>
		/// Adds a main teacher to a course which doesn't have a
		/// main teacher defined already (see test data defined above).
		/// </summary>
		[TestMethod]
		public void AddTeacher_WithValidTeacherAndCourse()
		{
			// Arrange:
			var model = new AddTeacherViewModel
			{
				SSN  = SSN_GUNNA,
				Type = TeacherType.MainTeacher
			};
			var prevCount = _teacherRegistrations.Count;

			// Act:
			var dto = _service.AddTeacherToCourse(COURSEID_VEFT_20163, model);
			// Assert:

			// Check that the dto object is correctly populated:
			Assert.AreEqual(SSN_GUNNA, dto.SSN);
			Assert.AreEqual(NAME_GUNNA, dto.Name);

			// Ensure that a new entity object has been created:
			var currentCount = _teacherRegistrations.Count;
			Assert.AreEqual(prevCount + 1, currentCount);

			// Get access to the entity object and assert that
			// the properties have been set:
			var newEntity = _teacherRegistrations.Last();
			Assert.AreEqual(COURSEID_VEFT_20163, newEntity.CourseInstanceID);
			Assert.AreEqual(SSN_GUNNA, newEntity.SSN);
			Assert.AreEqual(TeacherType.MainTeacher, newEntity.Type);

			// Ensure that the Unit Of Work object has been instructed
			// to save the new entity object:
			Assert.IsTrue(_mockUnitOfWork.GetSaveCallCount() > 0);
		}

        [TestMethod]
		[ExpectedException(typeof(AppObjectNotFoundException))]
		public void AddTeacher_InvalidCourse()
		{
			// Arrange:
			var model = new AddTeacherViewModel
			{
				SSN  = SSN_GUNNA,
				Type = TeacherType.AssistantTeacher
			};

			// Act:
			var dto = _service.AddTeacherToCourse(INVALID_COURSEID, model);
			// Assert:
			Assert.Fail("No exception was thrown");
		}

		[TestMethod]
		public void AddTeacher_CheckIfMainTeacherNameIsReturnedCorrectly()
		{
			// Arrange:
			// Should have one course and one main teacher
			var CoursesIn20153 = _service.GetCourseInstancesBySemester("20153");
			// Contain one course and no teachers
			var CoursesIn20163 = _service.GetCourseInstancesBySemester("20163");

			// Act:
			// Assert:			
			foreach(CourseInstanceDTO c in CoursesIn20153){
				Assert.AreNotEqual("", c.MainTeacher);
				Assert.AreEqual("Daníel B. Sigurgeirsson", c.MainTeacher);
			}
			foreach(CourseInstanceDTO c in CoursesIn20163){
				Assert.AreEqual("", c.MainTeacher);
			}

			// Arrange pt2:
			// Add an assistant teacher to a course with no teachers
			var model = new AddTeacherViewModel
			{
				SSN  = SSN_SKEBOR,
				Type = TeacherType.AssistantTeacher
			};
			// Act pt2:
			// Add an assistant teacher to the course
			_service.AddTeacherToCourse(COURSEID_VEFT_20163, model);
			// Assert pt3:
			// Still no main teacher, check if returns empty string
			foreach(CourseInstanceDTO c in CoursesIn20163){
				Assert.AreEqual("", c.MainTeacher);
			}
		}

		/// <summary>
		/// Ensure it is not possible to add a person as a teacher
		/// when that person is not registered in the system.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof (AppObjectNotFoundException))]
		public void AddTeacher_InvalidTeacher()
		{
			// Arrange:
			var model = new AddTeacherViewModel
			{
				SSN  = INVALID_SSN,
				Type = TeacherType.MainTeacher
			};
			int prevCount = _teacherRegistrations.Count();

			// Act: 
			_service.AddTeacherToCourse(COURSEID_VEFT_20163, model);

			// Assert:
			// Ensure that the number of teachers in the course is the same, that is no teacher was added
			Assert.AreEqual(prevCount, _teacherRegistrations.Count());
			// Assert that an exception has been thrown.
			Assert.Fail("No exception was thrown");
		}

		/// <summary>
		/// In this test, we test that it is not possible to
		/// add another main teacher to a course, if one is already
		/// defined.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof (AppValidationException))]
		public void AddTeacher_AlreadyWithMainTeacher()
		{
			// Arrange:
			var model = new AddTeacherViewModel
			{
				SSN  = SSN_GUNNA,
				Type = TeacherType.MainTeacher
			};
			// Act:
			// add a main teacher to a course that already has a main teacher
			_service.AddTeacherToCourse(COURSEID_VEFT_20153, model);

			// Assert:
			// check how many main teachers are in the course
				//TODO
			// Check if the exception was thrown
			Assert.Fail("No exception was thrown");
		}

		/// <summary>
		/// In this test, we ensure that a person cannot be added as a
		/// teacher in a course, if that person is already registered
		/// as a teacher in the given course.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(AppValidationException))]
		public void AddTeacher_PersonAlreadyRegisteredAsTeacherInCourse()
		{
			// Arrange:
			var model = new AddTeacherViewModel
			{
				SSN  = SSN_DABS,
				Type = TeacherType.AssistantTeacher
			};

			// Act:
			// Add teacher to a course he is already teaching
			_service.AddTeacherToCourse(COURSEID_VEFT_20153, model);

			// Assert:
			// Check if the exception was thrown
			Assert.Fail("No exception was thrown");
		}

		#endregion
	}
}
