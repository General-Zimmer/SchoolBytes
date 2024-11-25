using NUnit.Framework;
using SchoolBytes.Controllers;
using SchoolBytes.Models;
using SchoolBytes.util;
using System;
using System.Data.Common;
using System.Net;
using System.Reflection;
using System.Web.Mvc;
using TechTalk.SpecFlow;



namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class UnsubscribingAParticipantFromACourseModuleStepDefinitions
    {
        private static DBConnection _context = DBConnection.getDBContext();

        private static Participant bob = new Participant("Bob", "69695512");
        private static Teacher teacher1 = new Teacher() { Name = "teacher1", Id = 55435 };
        private static Course course1 = new Course() { Name = "Course1", Id = 123, Teacher = teacher1 };
        private static Course course2 = new Course() { Name = "Course2", Id = 1234, Teacher = teacher1 };
        private static CourseModule cm1 = new CourseModule() { Name = "cm1", Id = 888, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1), Capacity = 5, MaxCapacity = 5 };
        private static CourseModule cm2 = new CourseModule() { Name = "cm2", Id = 889, Teacher = teacher1, Course = course2, StartTime = DateTime.Now.AddDays(1), Capacity = 5, MaxCapacity = 5 };

        [BeforeFeature]
        public static void UnsubFeature(FeatureContext featureContext)
        {
            _context.Add(bob);
            _context.Add(course1);
            _context.Add(course2);
            _context.Add(cm1);
            _context.Add(cm2);
            _context.Add(teacher1);
            

            _context.SaveChanges();

        }



        [Given(@"a course module exists with id (.*) that has a participant with phone number ""([^""]*)""")]
        public void GivenACourseModuleExistsWithIdThatHasAParticipantWithPhoneNumber(int moduleId, string phoneNumber)
        {

            //COURSE
            Course course = _context.courses.Find(123);

            //participant
            Participant bob1 = new Participant("Bob", phoneNumber);
            _context.Add(bob1);

            _context.SaveChanges();

            //CM
            CourseModule cm = course.CoursesModules.Find(m => m.Id == moduleId);

            //PARTICIPANT REGISTRATION
            Registration reg = new Registration(bob, cm);

            cm.Registrations.Add(reg);

        }

        [Given(@"a course module exists with id (.*) that does not have a participant with phone number ""([^""]*)""")]
        public void GivenACourseModuleExistsWithIdThatDoesNotHaveAParticipantWithPhoneNumber(int moduleId, string phoneNumber)
        {
            Course course = _context.courses.Find(1234);

            CourseModule cm = course.CoursesModules.Find(m => m.Id == moduleId);

            Registration registration = cm.Registrations.FirstOrDefault(reg => reg.participant.PhoneNumber == phoneNumber);

            Participant participant = registration.participant;

        }

        [When(@"I unsubscribe the participant with phone number ""([^""]*)"" from course id (.*) and module id (.*)")]
        public void WhenIUnsubscribeTheParticipantWithPhoneNumberFromCourseIdAndModuleId(string phoneNumber, int courseId, int moduleId)
        {

            DatabaseUtils.Unsub(courseId, moduleId, phoneNumber);

        }

        [Then(@"the participant with phone number ""([^""]*)"" should no longer be registered in the course module")]
        public void ThenTheParticipantWithPhoneNumberShouldNoLongerBeRegisteredInTheCourseModule(string phoneNumber)
        {
            Participant participant = _context.participants.FirstOrDefault(p => p.PhoneNumber == phoneNumber);

            Assert.IsNull(participant);
        }

        [Then(@"no error should be returned")]
        public void ThenNoErrorShouldBeReturned()
        {
            //?? 
            Assert.Catch(null);
            
        }

        [Then(@"an error with status code (.*) and message ""([^""]*)"" should be returned")]
        public void ThenAnErrorWithStatusCodeAndMessageShouldBeReturned(int statusCode, string errorMessage)
        {

            //HttpStatusCodeResult status = HttpStatusCodeResult(HttpStatusCode.BadRequest, "No registration found for the couse with the phonenumber.");
            var status = new HttpStatusCodeResult((int)HttpStatusCode.BadRequest, "No registration found for the course with the phonenumber.");


            Assert.Equals(status.StatusCode, statusCode);
            Assert.Equals(status.StatusDescription, errorMessage);
 
        }

        [Given(@"a course module exists with id (.*) with a waiting list participant")]
        public void GivenACourseModuleExistsWithIdWithAWaitingListParticipant(int moduleId)
        {

            

            CourseModule cm4 = new CourseModule() { Name = "cm4", Id = moduleId, Teacher = teacher1, Course = course2, StartTime = DateTime.Now.AddDays(1), Capacity = 5, MaxCapacity = 5 };
            
            _context.Add(cm4);

            WaitRegistration waitReg = new WaitRegistration(bob, cm4);

            _context.SaveChanges();

        }

        [Given(@"the module has a participant with phone number ""([^""]*)""")]
        public void GivenTheModuleHasAParticipantWithPhoneNumber(string phoneNumber)
        {

            Participant bob2 = new Participant("Bobski", phoneNumber);
            _context.Add(bob2);

            CourseModule cm3 = new CourseModule() { Name = "cm3", Id = 236, Teacher = teacher1, Course = course2, StartTime = DateTime.Now.AddDays(1), Capacity = 5, MaxCapacity = 5 };
            _context.Add(cm3);


            Registration regi = new Registration(bob2, cm3);


        }

        [Then(@"the participant from the waiting list should be added to the course module")]
        public void ThenTheParticipantFromTheWaitingListShouldBeAddedToTheCourseModule()
        {
            
            
        }
    }
}
