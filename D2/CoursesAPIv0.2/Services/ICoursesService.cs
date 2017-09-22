﻿using System;
using System.Collections.Generic;
using CoursesAPI.Models.DTOModels;
using CoursesAPI.Models.ViewModels;
/// <summary>
/// Interface for courseService, each function in courseService must also be here.
/// </summary>
namespace CoursesAPI.Services {
    public interface ICoursesService {
        IEnumerable<CoursesDTO> GetCourses(string semester);
        CoursesDTO GetCourseByID(int courseID);
        CoursesDTO UpdateCourse(int courseID, CourseViewModel updatedCourse);
        bool CreateCourse(CourseViewModel newCourse);
        bool DeleteCourse(int courseID);
        StudentsDTO AddStudentToCourse(StudentViewModel newStudent, int courseID);
        bool AddStudentToWaitingList(StudentViewModel newStudent, int courseId);
        bool CheckIfStudentExists(int studentID);
        List<StudentsDTO> GetStudentsInCourse(int courseID);
        List<StudentsDTO> GetWaitingList(int courseID);
        bool canAddToCourse(int courseID);
        bool removeFromWaitingList(int studentID, int courseID);
        bool removeStudentFromCourse(int courseID, string ssn);
        bool CourseExists(int courseID);
        int getStudentID(string ssn);
        bool checkIfAlreadyRegistered(int studentID, int courseID);
    }
}