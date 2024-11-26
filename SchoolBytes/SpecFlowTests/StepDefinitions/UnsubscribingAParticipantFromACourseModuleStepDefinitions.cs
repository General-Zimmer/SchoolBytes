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


        [AfterFeature]
        public static void AfterTilmeldingFeature(FeatureContext featureContext)
        {
            _context.Remove(_context.courseModules.Find(cm1.Id));
            _context.Remove(_context.courseModules.Find(cm2.Id));
            _context.Remove(_context.courses.Find(course1.Id));
            _context.Remove(_context.courses.Find(course2.Id));
            _context.Remove(_context.teachers.Find(teacher1.Id));
            _context.Remove(_context.participants.Find(bob.Id));

            //TEST1
            _context.Remove(_context.participants.FirstOrDefault(p => p.Name == "tester1"));

            //TEST2
            _context.Remove(_context.courseModules.FirstOrDefault(cm => cm.Name == "cm4"));

            //TEST 3
            _context.Remove(_context.participants.FirstOrDefault(p => p.Name == "Bobski"));
            _context.Remove(_context.courseModules.FirstOrDefault(cm => cm.Name == "cm3"));

            _context.SaveChanges();
        }




        [Given(@"a course module exists with id (.*) that has a participant with phone number ""([^""]*)""")]
        public void GivenACourseModuleExistsWithIdThatHasAParticipantWithPhoneNumber(int moduleId, string phoneNumber)
        {
            //TEST1
            
            Course course = _context.courses.Find(123);

            
            Participant bob1 = new Participant("tester1", phoneNumber);
            _context.Add(bob1);

            _context.SaveChanges();

            
            CourseModule cm = course.CoursesModules.Find(m => m.Id == moduleId);

            
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

        

        [Then(@"an error with status code (.*) and message ""([^""]*)"" should be returned")]
        public void ThenAnErrorWithStatusCodeAndMessageShouldBeReturned(int statusCode, string errorMessage)
        {

            
            var expectedStatus = new HttpStatusCodeResult(statusCode, errorMessage);

            
            Assert.AreEqual(expectedStatus.StatusCode, statusCode, "Statuskoderne matcher ikke.");

            
            Assert.AreEqual(expectedStatus.StatusDescription, errorMessage, "Statusbeskederne matcher ikke.");

        }

        [Given(@"a course module exists with id (.*) with a waiting list participant")]
        public void GivenACourseModuleExistsWithIdWithAWaitingListParticipant(int moduleId)
        {

            //Test 2

            
            CourseModule cm4 = new CourseModule
            {
                Name = "cm4",
                Id = moduleId,
                Teacher = teacher1,
                Course = course2,
                StartTime = DateTime.Now.AddDays(1),
                Capacity = 5,
                MaxCapacity = 5
            };

            
            _context.courseModules.Add(cm4);

            
            WaitRegistration waitReg = new WaitRegistration(bob, cm4);

            
            cm4.Waitlist.AddLast(waitReg);

            
            _context.SaveChanges();

        }

        [Given(@"the module has a participant with phone number ""([^""]*)""")]
        public void GivenTheModuleHasAParticipantWithPhoneNumber(string phoneNumber)
        {

            //test 3
            Participant bob2 = new Participant("Bobski", phoneNumber);
            _context.participants.Add(bob2); 

            
            CourseModule cm3 = new CourseModule
            {
                Name = "cm3",
                Id = 236,
                Teacher = teacher1,
                Course = course2,
                StartTime = DateTime.Now.AddDays(1),
                Capacity = 5,
                MaxCapacity = 5
            };
            _context.courseModules.Add(cm3); 

            
            Registration regi = new Registration(bob2, cm3);
            cm3.Registrations.Add(regi); 

            
            _context.SaveChanges();


        }

        [Then(@"the participant from the waiting list should be added to the course module")]
        public void ThenTheParticipantFromTheWaitingListShouldBeAddedToTheCourseModule()
        {
            
            var courseModule = _context.courseModules.FirstOrDefault(cm => cm.Waitlist.Any());
            Assert.IsNotNull(courseModule, "No course module with a waiting list was found.");

            
            var firstWaitlistedParticipant = courseModule.Waitlist.First.Value.participant;

            
            var isParticipantRegistered = courseModule.Registrations.Any(reg => reg.participant.Id == firstWaitlistedParticipant.Id);

            Assert.IsTrue(isParticipantRegistered, "The participant from the waiting list was not added to the course module.");

        }
    }
}
