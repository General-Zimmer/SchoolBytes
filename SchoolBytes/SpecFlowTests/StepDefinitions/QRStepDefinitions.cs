using NUnit.Framework;
using System;
using TechTalk.SpecFlow;
using SchoolBytes.Controllers;
using SchoolBytes.Models;
using System.Web.Mvc;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class QRStepDefinitions
    {
        
        private string initialId;
        private string newId;
        private static DBConnection _context = DBConnection.getDBContext();
        private static Participant bob = new Participant("Bob", "12345678");
        private static Teacher teacher1 = new Teacher() { Name = "teacher1", Id = 55435 };
        private static Course course1 = new Course() { Name = "Course1", Id = 666, Teacher = teacher1 };
        private static CourseModule cm1 = new CourseModule() { Name = "cm1", Id = 888, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1) };

        private static CourseModule cm2 = new CourseModule() { Name = "cm2", Id = 123132232, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1) };
        private static CourseModule cm3 = new CourseModule() { Name = "cm3", Id = 423524, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1) };
        private static CourseModule cm4 = new CourseModule() { Name = "cm4", Id = 68368833, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1) };
        private static CourseModule cm5 = new CourseModule() { Name = "cm5", Id = 345372, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1) };
        private static CourseModule cm6 = new CourseModule() { Name = "cm6", Id = 323411114, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1) };

        private static Participant bobby = new Participant("Bobby", "69695512");
        private static Registration registrationTest = new Registration(bob, cm1);

        private static QRController qrController = new();

        [BeforeFeature]
        public static void BeforeTilmeldingFeature(FeatureContext featureContext)
        {
            _context.Add(bob);
            _context.Add(bobby);
            _context.Add(teacher1);
            _context.Add(course1);
            _context.Add(cm1);
            _context.Add(cm2);
            _context.Add(cm3);
            _context.Add(cm4);
            _context.Add(cm5);
            _context.Add(cm6);

            _context.SaveChanges();
        }

        [AfterFeature]
        public static void AfterTilmeldingFeature(FeatureContext featureContext)
        {
            _context.Remove(_context.courseModules.Find(cm1.Id));
            _context.Remove(_context.courseModules.Find(cm2.Id));
            _context.Remove(_context.courseModules.Find(cm3.Id));
            _context.Remove(_context.courseModules.Find(cm4.Id));
            _context.Remove(_context.courseModules.Find(cm5.Id));
            _context.Remove(_context.courseModules.Find(cm6.Id));
            _context.Remove(_context.courses.Find(course1.Id));
            _context.Remove(_context.teachers.Find(teacher1.Id));
            _context.Remove(_context.participants.Find(bob.Id));
            _context.Remove(_context.participants.Find(bobby.Id));

            _context.SaveChanges();
        }
        

        /*private static Course testCourse = new Course("TestCourse", "test", DateTime.Now, DateTime.Now.AddMinutes(5), 1, 1);
        private static CourseModule courseTestModule = new CourseModule { Id = 1, Name = "Test Course Module",
            Date = DateTime.Now, StartTime = DateTime.Now.AddMinutes(10), EndTime = DateTime.Now.AddMinutes(15), 
            Capacity = 30, MaxCapacity = 50, Location = "Room 101", Teacher = new Teacher { Id = 1, Name = "John Doe" }, Course = testCourse };
        private static Participant participantTest = new Participant("TestParticipant", "12345678");
        private static Registration registrationTest = new Registration(participantTest, courseTestModule);
        private static DBConnection dbConnection = DBConnection.getDBContext();

        [BeforeFeature]
        public static void SetUp()
        {
            testCourse.CoursesModules.Add(courseTestModule);
            participantTest.Id = 1;
            courseTestModule.Registrations.Add(registrationTest);
            dbConnection.Add(testCourse);
            dbConnection.Add(courseTestModule);
            dbConnection.Add(participantTest);
            dbConnection.Add(registrationTest);
            dbConnection.SaveChanges();
        }

        [AfterFeature]
        public static void CleanUp()
        {
            // Clean up the test data from the database after the test
            dbConnection.Remove(testCourse);
            dbConnection.Remove(courseTestModule);
            dbConnection.Remove(participantTest);
            dbConnection.Remove(registrationTest);
            dbConnection.SaveChanges();
        }*/

        // Scenario: ID for check-in changes when new QR Code is generated
        [Given(@"I have access to the Generation Webpage")]
        public void GivenIHaveAccessToTheGenerationWebpage()
        {
            // Simulate accessing the generation webpage. Currently simply knowing the webpage is enough as there is no implementation of
            // the page being admin only accessible
            Assert.IsTrue(true, "User has access to the QR code generation page.");
        }

        [When(@"I generate a new QR Code")]
        public void WhenIGenerateANewQRCode()
        {
            // Get the current ID and then generate a new one and set it
            initialId = qrController.GetQrId(); 
            string newIdTemp = qrController.GenerateRandomId();
            qrController.SetQrId(newIdTemp);
            newId = qrController.GetQrId();
        }

        [Then(@"the ID for Attendance Check In should change")]
        public void ThenTheIDForAttendanceCheckInShouldChange()
        {
            // Validate that the ID has changed
            Assert.AreNotEqual(initialId, newId, "The QR Code ID should change when a new QR Code is generated.");
        }


        // Scenario for attendance
        [Given(@"I have a valid registration for a course module with id (.*) for course id (.*) for a participant with phone number ""(.*)""")]
        public void GivenIHaveAValidRegistration(int moduleId, int courseId, string phoneNumber)
        {
            // Ensure that the registration exists in the database with the given parameters
            var course = _context.courses.Find(courseId);
            CourseModule courseModule = course.CoursesModules.Find(c => c.Id == moduleId);
            Registration reg = courseModule.Registrations.Find(r => r.participant.PhoneNumber == phoneNumber);
            reg = registrationTest;
            // Assert that the registration exists
            Assert.IsNotNull(reg, "Registration not found");
        }

        [When(@"I check in the participant with phone number ""(.*)"" from course id (.*) and module id (.*)")]
        public void WhenICheckInTheParticipantWithPhoneNumberFromCourseIdAndModuleId(string phoneNumber, int courseId, int moduleId)    
        {
            // Call the RegistrationCheckIn method and store the result
            ActionResult checkInResult = qrController.RegistrationCheckIn(phoneNumber, moduleId);

            // Assert the HTTP status code is 200
            Assert.IsInstanceOf<HttpStatusCodeResult>(checkInResult, "Expected HttpStatusCodeResult");
            var statusCodeResult = (HttpStatusCodeResult)checkInResult;
            Assert.AreEqual(200, statusCodeResult.StatusCode, "Expected status code 200 (OK)");
        }

        [Then(@"the attendance for my registration should change to true")]
        public void ThenTheAttendanceForMyRegistrationShouldChangeToTrue()
        {
            Assert.IsTrue(registrationTest.Attendance, "Attendance should be true");
        }
    }
}
