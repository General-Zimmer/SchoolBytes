using SchoolBytes.Models;
using System;
using TechTalk.SpecFlow;
using static SchoolBytes.util.VentelisteUtil;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class VentelisteNoDuplicatesStepDefinitions
    {
        DBConnection db = DBConnection.getDBContext();
        CourseModule courseModule;
        LinkedList<WaitRegistration> waitRegistrations;
        Participant participant;

        [Given(@"the waitlist already contains the same participant")]
        public void GivenTheWaitlistAlreadyContainsTheSameParticipant()
        {
            participant = db.participants.First();
            courseModule = db.courseModules.First();
            waitRegistrations = courseModule.Waitlist;
            waitRegistrations.AddLast(new WaitRegistration(participant, courseModule));
        }

        [When(@"the participant tries to sign up to the waitlist again")]
        public void WhenTheParticipantTriesToSignUpToTheWaitlistAgain()
        {
            AddToWaitlist(courseModule, participant);
        }

        [Then(@"they should not be added to the waitlist")]
        public void ThenTheyShouldNotBeAddedToTheWaitlist()
        {
            int counter = 0;

            foreach (var wait in courseModule.Waitlist) 
            {
                if (wait.participant.PhoneNumber.Equals(participant.PhoneNumber)) 
                { 
                    counter++;
                }
            }

            counter.Should().Be(1);

            // courseModule dummy gets deleted here
        }
    }
}
