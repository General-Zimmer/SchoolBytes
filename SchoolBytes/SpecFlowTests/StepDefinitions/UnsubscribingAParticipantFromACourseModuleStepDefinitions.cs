using NUnit.Framework;
using SchoolBytes.Controllers;
using SchoolBytes.Models;
using System;
using System.Reflection;
using TechTalk.SpecFlow;


namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class UnsubscribingAParticipantFromACourseModuleStepDefinitions
    {
        private readonly List<CourseModule> _courseModules = new(); 
        private readonly List<Participant> _participants = new(); 
        private ModuleController _controller;
        private Course _course; 
        private CourseModule _courseModule; 
        private Participant _participant; 
        private string _errorMessage; 
        private int _statusCode;



        [Given(@"a course module exists with id (.*) that has a participant with phone number ""([^""]*)""")]
        public void GivenACourseModuleExistsWithIdThatHasAParticipantWithPhoneNumber(int moduleId, string phoneNumber)
        {
            
            _participant = new Participant("Test Participant", phoneNumber);

            
            _courseModule = new CourseModule
            {
                Id = moduleId,
                Registrations = new System.Collections.Generic.List<Registration>
                {
                    new Registration(_participant, null)
                }
            };


            _course = new Course("test1", "desc", DateTime.Now, DateTime.UtcNow, 30, 1);
            _course.CoursesModules.Add(_courseModule);
        }

        [Given(@"a course module exists with id (.*) that does not have a participant with phone number ""([^""]*)""")]
        public void GivenACourseModuleExistsWithIdThatDoesNotHaveAParticipantWithPhoneNumber(int moduleId, string phoneNumber)
        {
            
            _courseModule = new CourseModule
            {
                Id = moduleId,
                Registrations = new System.Collections.Generic.List<Registration>()
            };


            _course = new Course("test1", "desc", DateTime.Now, DateTime.UtcNow, 30, 1);
            _course.CoursesModules.Add(_courseModule);
        }

        [When(@"I unsubscribe the participant with phone number ""([^""]*)"" from course id (.*) and module id (.*)")]
        public void WhenIUnsubscribeTheParticipantWithPhoneNumberFromCourseIdAndModuleId(string phoneNumber, int courseId, int moduleId)
        {

            _controller.unsub(courseId, moduleId, phoneNumber);

        }

        [Then(@"the participant with phone number ""([^""]*)"" should no longer be registered in the course module")]
        public void ThenTheParticipantWithPhoneNumberShouldNoLongerBeRegisteredInTheCourseModule(string phoneNumber)
        {
            var isStillRegistered = _courseModule.Registrations
                .Any(reg => reg.participant.PhoneNumber == phoneNumber);

            Assert.IsFalse(isStillRegistered, $"Deltager med tlf nr: {phoneNumber} er stadig registreret");
        }

        [Then(@"no error should be returned")]
        public void ThenNoErrorShouldBeReturned()
        {
            Assert.IsNull(_errorMessage, $"An unexpected error occurred: {_errorMessage}");
        }

        [Then(@"an error with status code (.*) and message ""([^""]*)"" should be returned")]
        public void ThenAnErrorWithStatusCodeAndMessageShouldBeReturned(int statusCode, string errorMessage)
        {
            Assert.AreEqual(statusCode, _statusCode, "Status code does not match.");
            Assert.AreEqual(errorMessage, _errorMessage, "Error message does not match.");
        }

        [Given(@"a course module exists with id (.*) with a waiting list participant")]
        public void GivenACourseModuleExistsWithIdWithAWaitingListParticipant(int moduleId)
        {

            var waitlistParticipant = new Participant("Waitlist Participant", "12345678");

            
            _courseModule = new CourseModule
            {
                Id = moduleId,
                Waitlist = new LinkedList<WaitRegistration>
                {
                    new WaitRegistration(waitlistParticipant, _courseModule)
                }
            };


            _course = new Course("test1", "desc", DateTime.Now, DateTime.UtcNow, 30, 1);
            _course.CoursesModules.Add(_courseModule);
        }

        [Given(@"the module has a participant with phone number ""([^""]*)""")]
        public void GivenTheModuleHasAParticipantWithPhoneNumber(string phoneNumber)
        {
            var participant = new Participant("Testtttt", phoneNumber);
            _courseModule.Registrations.Add(new Registration(participant, _courseModule));
        }

        [Then(@"the participant from the waiting list should be added to the course module")]
        public void ThenTheParticipantFromTheWaitingListShouldBeAddedToTheCourseModule()
        {
            var waitlistParticipant = _courseModule.Waitlist.First().participant;
            Assert.IsTrue(_courseModule.Registrations
                .Any(r => r.participant.PhoneNumber == waitlistParticipant.PhoneNumber),
                "The waiting list participant was not added to the course module.");
        }
    }
}
