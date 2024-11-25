using System;
using TechTalk.SpecFlow;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class RemoveParticipantFromCourseStepDefinitions
    {
        [BeforeFeature]
        public static void Before()
        {
            
        }

        [AfterFeature]
        public static void After() 
        { 

        }

        [Given(@"\[Participant is enrolled to course]")]
        public void GivenParticipantIsEnrolledToCourse()
        {
            throw new PendingStepException();
        }

        [When(@"\[When user removes participant from course]")]
        public void WhenWhenUserRemovesParticipantFromCourse()
        {
            throw new PendingStepException();
        }

        [Then(@"\[Participant should be removed from the list in course]")]
        public void ThenParticipantShouldBeRemovedFromTheListInCourse()
        {
            throw new PendingStepException();
        }
    }
}
