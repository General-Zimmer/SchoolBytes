using SchoolBytes.Models;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class VentelisteStepDefinitions
    {
        DBConnection db = DBConnection.getDBContext();
        CourseModule courseModule;
        LinkedList<WaitRegistration> waitRegistrations;
        Participant newParticipant;
        WaitRegistration newWaitRegistration;

        [Given(@"the waitlist already contains at least one participant")]
        public void GivenTheWaitlistAlreadyContainsAtLeastOneParticipant()
        {
            Participant participant = db.participants.First();
            courseModule = db.courseModules.First();
            waitRegistrations = courseModule.Waitlist;
            waitRegistrations.AddLast(new WaitRegistration(participant, courseModule));
        }

        [When(@"the participant signs up")]
        public void WhenTheParticipantSignsUp()
        {
            newParticipant = new Participant("Jimbob", "99887766");
            newWaitRegistration = new WaitRegistration(newParticipant, courseModule);
            waitRegistrations.AddLast(newWaitRegistration);
        }

        [Then(@"they should be last on the waitlist")]
        public void ThenTheyShouldBeLastOnTheWaitlist()
        {
            WaitRegistration waitRegistration = waitRegistrations.Last();
            waitRegistration.Should().BeEquivalentTo(newWaitRegistration);
        }
    }
}
