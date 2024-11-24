using System;
using TechTalk.SpecFlow;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class UnsubscribingAParticipantFromACourseModuleStepDefinitions
    {
        [Given(@"a course module exists with id (.*) that has a participant with phone number ""([^""]*)""")]
        public void GivenACourseModuleExistsWithIdThatHasAParticipantWithPhoneNumber(int p0, string p1)
        {
            throw new PendingStepException();
        }

        [When(@"I unsubscribe the participant with phone number ""([^""]*)"" from course id (.*) and module id (.*)")]
        public void WhenIUnsubscribeTheParticipantWithPhoneNumberFromCourseIdAndModuleId(string p0, int p1, int p2)
        {
            throw new PendingStepException();
        }

        [Then(@"the participant with phone number ""([^""]*)"" should no longer be registered in the course module")]
        public void ThenTheParticipantWithPhoneNumberShouldNoLongerBeRegisteredInTheCourseModule(string p0)
        {
            throw new PendingStepException();
        }

        [Then(@"no error should be returned")]
        public void ThenNoErrorShouldBeReturned()
        {
            throw new PendingStepException();
        }

        [Given(@"a course module exists with id (.*) that does not have a participant with phone number ""([^""]*)""")]
        public void GivenACourseModuleExistsWithIdThatDoesNotHaveAParticipantWithPhoneNumber(int p0, string p1)
        {
            throw new PendingStepException();
        }

        [Then(@"an error with status code (.*) and message ""([^""]*)"" should be returned")]
        public void ThenAnErrorWithStatusCodeAndMessageShouldBeReturned(int p0, string p1)
        {
            throw new PendingStepException();
        }

        [Given(@"a course module exists with id (.*) with a waiting list participant")]
        public void GivenACourseModuleExistsWithIdWithAWaitingListParticipant(int p0)
        {
            throw new PendingStepException();
        }

        [Given(@"the module has a participant with phone number ""([^""]*)""")]
        public void GivenTheModuleHasAParticipantWithPhoneNumber(string p0)
        {
            throw new PendingStepException();
        }

        [Then(@"the participant from the waiting list should be added to the course module")]
        public void ThenTheParticipantFromTheWaitingListShouldBeAddedToTheCourseModule()
        {
            throw new PendingStepException();
        }
    }
}
