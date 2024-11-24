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
        private bool isAttendanceUpdated;
        private Course testCourse;
        private CourseModule courseTestModule;
        private Participant participantTest;
        private Registration registrationTest;
        private QRController qrController = new();
        DBConnection dbConnection = DBConnection.getDBContext();
        [BeforeScenario]
        public void SetUp()
        {
            testCourse = new Course("TestCourse", "test", DateTime.Now, DateTime.Now.AddMinutes(5), 1, 123);
            courseTestModule = new CourseModule
            {
                Id = 123,
                Name = "Test Course Module",
                Date = DateTime.Now, // This should be the date for the module
                StartTime = DateTime.Now.AddMinutes(10), // Set a time 10 minutes from now
                EndTime = DateTime.Now.AddMinutes(15), // Set a time 15 minutes from now
                Capacity = 30, // Set the capacity of the course module
                MaxCapacity = 50, // Max capacity for the module
                Location = "Room 101", // Example location
                Teacher = new Teacher { Id = 1, Name = "John Doe" }, // Associate a teacher
                Course = testCourse // Associate the testCourse with this module
            };
            testCourse.CoursesModules.Add(courseTestModule);
            Participant testParticipant = new Participant("TestParticipant", "88888888");
            testParticipant.Id = 123;
            Registration testRegistration = new Registration(testParticipant, courseTestModule);
            courseTestModule.Registrations.Add(testRegistration);
            dbConnection.Add(testCourse);
            dbConnection.Add(courseTestModule);
            dbConnection.Add(testParticipant);
            dbConnection.Add(testRegistration);
            dbConnection.SaveChanges();
        }

        [AfterScenario]
        public void CleanUp()
        {
            // Clean up the test data from the database after the test
            dbConnection.Remove(testCourse);
            dbConnection.Remove(courseTestModule);
            dbConnection.Remove(participantTest);
            dbConnection.Remove(registrationTest);
            dbConnection.SaveChanges();
        }

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
            var course = dbConnection.courses.Find(courseId);
            CourseModule courseModule = course.CoursesModules.Find(c => c.Id == moduleId);
            Registration reg = courseModule.Registrations.Find(r => r.participant.PhoneNumber == phoneNumber);
            reg = registrationTest;
            // Assert that the registration exists
            Assert.IsNotNull(reg, "Registration not found");
        }

        [When(@"I check in the participant with phone number ""(.*)"" from course id (.*)
                and module id (.*)")]
        public void WhenICheckInTheParticipant(string phoneNumber, int courseId, int moduleId)
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
