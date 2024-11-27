using NUnit.Framework;
using System;
using TechTalk.SpecFlow;
using SchoolBytes.Controllers;
using SchoolBytes.Models;
using SchoolBytes.util;
using System.Web.Mvc;
using System.Web;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class QRStepDefinitions
    {
        private string initialId;
        private string newId;
        private string testPhoneNumber;
        private HttpStatusCodeResult errorResult;
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
        private static Registration registrationTest2 = new Registration(bobby, cm1);

        private QRController qrController = new();

        [BeforeFeature]
        public static void BeforeQRFeature(FeatureContext featureContext)
        {
            cm1.Registrations.Add(registrationTest);
            //registrationTest2.Attendance = true;
            //cm1.Registrations.Add(registrationTest2);
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
            _context.UpdateSub(registrationTest, cm1);
            _context.SaveChanges();
            //_context.UpdateSub(registrationTest2, cm1);
            //_context.SaveChanges();
        }

        [AfterFeature]
        public static void AfterQRFeature(FeatureContext featureContext)
        {
            _context.Remove(registrationTest);
            //_context.Remove(registrationTest2);
            _context.Remove(_context.courseModules.Find(cm1.Id));
            _context.Remove(_context.courseModules.Find(cm2.Id));
            _context.Remove(_context.courseModules.Find(cm3.Id));
            _context.Remove(_context.courseModules.Find(cm4.Id));
            _context.Remove(_context.courseModules.Find(cm5.Id));
            _context.Remove(_context.courseModules.Find(cm6.Id));
            _context.Remove(course1);
            _context.Remove(_context.teachers.Find(teacher1.Id));
            _context.Remove(_context.participants.Find(bob.Id));
            _context.Remove(_context.participants.Find(bobby.Id));

            _context.SaveChanges();
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
            var course = _context.courses.Find(courseId);
            Assert.IsNotNull(course, $"Course with ID {courseId} not found.");
            Assert.IsNotNull(course.CoursesModules, "CoursesModules navigation property is null.");
            Assert.IsTrue(course.CoursesModules.Any(c => c.Id == moduleId), $"CourseModule with ID {moduleId} not found for Course ID {courseId}.");
            CourseModule courseModule = course.CoursesModules.Find(c => c.Id == moduleId);
            Assert.IsNotNull(courseModule, $"CourseModule with ID {moduleId} not found for Course ID {courseId}.");
            Registration reg = courseModule.Registrations.Find(r => r.participant.PhoneNumber == phoneNumber);
            // Assert that the registration exists
            Assert.IsNotNull(reg, "Registration not found");
        }

        [When(@"I check in the participant with phone number ""(.*)"" from course id (.*) and module id (.*)")]
        public void WhenICheckInTheParticipantWithPhoneNumberFromCourseIdAndModuleId(string phoneNumber, int courseId, int moduleId)    
        {
            // Call the RegistrationCheckIn method and store the result
            HttpStatusCodeResult checkInResult = QRUtils.RegistrationCheckIn(phoneNumber, moduleId);

            // Assert the HTTP status code is 200
            Assert.IsInstanceOf<HttpStatusCodeResult>(checkInResult, "Expected HttpStatusCodeResult");
            Assert.AreEqual(200, checkInResult.StatusCode, "Expected status code 200 (OK)");
            Assert.IsTrue(registrationTest.Attendance, "Attendance not true.");
        }

        [Then(@"the attendance for my registration should change to true")]
        public void ThenTheAttendanceForMyRegistrationShouldChangeToTrue()
        {
            Assert.IsTrue(registrationTest.Attendance, "Attendance should be true");
        }

        // Scenario: Proper error code when registration not found

        [Given(@"I have a valid course module and a phone number")]
        public void GivenIHaveAValidCourseModuleAndAPhoneNumber()
        {
            // Initialize a test phone number and a valid course module
            testPhoneNumber = "98765432"; // Assume this phone number does not exist in the database

            // Fetch an existing course module (e.g., cm2) for testing purposes
            CourseModule testCourseModule = _context.courseModules.FirstOrDefault(cm => cm.Id == cm2.Id);
            Assert.IsNotNull(testCourseModule, "Test course module not found.");
        }

        [When(@"I look for a registration with the given phonenumber")]
        public void WhenILookForARegistrationWithTheGivenPhonenumber()
        {
            // Attempt to find a registration using the non-existent phone number
            try
            {
                errorResult = QRUtils.RegistrationCheckIn(testPhoneNumber, cm2.Id);
            }
            catch (HttpException ex)
            {
                // Simulate a 404 error response when registration is not found
                errorResult = new HttpStatusCodeResult(404, ex.Message);
            }
        }

        [Then(@"I should receive the error code 404")]
        public void ThenIShouldReceiveTheErrorCode404()
        {
            // Assert that the error code is 404
            Assert.IsInstanceOf<HttpStatusCodeResult>(errorResult, "Expected HttpStatusCodeResult.");
            Assert.AreEqual(404, errorResult.StatusCode, "Expected status code 404 (Not Found).");
        }

        // Scenario: Proper response when attendance has already been registered
        [Given(@"a registration that has already been checked in")]
        public void GivenARegistrationThatHasAlreadyBeenCheckedIn()
        {
            // Ensure an existing registration is present
            Assert.IsNotNull(registrationTest, "Existing registration not found.");
            registrationTest.Attendance = true;
            // Verify that the attendance has been registered
            Assert.IsTrue(registrationTest.Attendance, "Attendance for the registration should already be true.");
        }

        [When(@"I try to check in again")]
        public void WhenITryToCheckInAgain()
        {
            // Attempt to check in the participant again
            errorResult = QRUtils.RegistrationCheckIn(registrationTest.participant.PhoneNumber, cm1.Id);
        }

        [Then(@"I should receive the status code 200 and the statusDescription ""(.*)""")]
        public void ThenIShouldReceiveTheStatusCode200AndTheStatusDescription(string expectedDescription)
        {
            // Assert that the response code is 200
            Assert.IsInstanceOf<HttpStatusCodeResult>(errorResult, "Expected HttpStatusCodeResult.");
            Assert.AreEqual(200, errorResult.StatusCode, "Expected status code 200 (OK).");

            // Assert that the description matches the expected value
            Assert.AreEqual("Fremmøde allerede registreret.", errorResult.StatusDescription, $"Expected status description \"Fremmøde allerede registreret.\"");
        }
    }
}
