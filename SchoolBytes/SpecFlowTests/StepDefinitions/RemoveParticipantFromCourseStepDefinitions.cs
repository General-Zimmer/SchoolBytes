using System;
using System.IO;
using TechTalk.SpecFlow;
using SchoolBytes.Models;
using SchoolBytes.util;
using NUnit.Framework;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class RemoveParticipantFromCourseStepDefinitions
    {
        private static DBConnection context = DBConnection.getDBContext();
        private static Participant mulan = new Participant("Mulan", "69695512");
        private static Course course = new Course() { Name = "Course1", Id = 696 };


        [BeforeFeature]
        public static void Before()
        {
            context.Add(mulan);
            context.Add(course);
            course.Participants.Add(mulan);
            context.SaveChanges();
        }

        [AfterFeature]
        public static void After()
        {
            context.Remove(course);
            context.Remove(mulan);
            context.SaveChanges();
        }

        [Given(@"\[Participant is enrolled to course]")]
        public void GivenParticipantIsEnrolledToCourse()
        {
            course.Participants.Should().Contain(mulan);
        }

        [When(@"\[When user removes participant from course]")]
        public void WhenWhenUserRemovesParticipantFromCourse()
        {
            RemoveParticipantFromCourse.DeleteParticipant(mulan.Id, course.Id, context);
        }

        [Then(@"\[Participant should be removed from the list in course]")]
        public void ThenParticipantShouldBeRemovedFromTheListInCourse()
        {
            course.Participants.Should().NotContain(mulan);
        }

        [Then(@"\[Show error message that partcipant is not in course anymore]")]
        public void ThenShowErrorMessageThatPartcipantIsNotInCourseAnymore()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            RemoveParticipantFromCourse.DeleteParticipant(mulan.Id, course.Id, context);
            string consoleOutput = stringWriter.ToString(); 
            Assert.That(consoleOutput, Does.Contain("participant not associated with course"));

        }
    }
}

