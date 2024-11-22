using System;
using SchoolBytes.Controllers;
using SchoolBytes.Models;
using TechTalk.SpecFlow;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class TilmeldingStepDefinitions
    {
        Participant bob = new Participant("Bob", "69695512");
        int erBobAdded;

        [Given(@"\[Participant bob tilmeldes modul tilhørende rigtige course]")]
        public void GivenParticipantBobTilmeldesModulTilhorendeRigtigeCourse()
        {
            //Participant bob = new Participant("Bob", "69695512" );

            DBConnection.SubscribeTest(666, 888, bob);

            
        }

        [When(@"\[Oprettes en instans i databasen]")]
        public void WhenOprettesEnInstansIDatabasen()
        {
            erBobAdded = DBConnection.GetSubscribeCount(bob);
            
        }

        [Then(@"\[At bob er tilmeldt modul]")]
        public void ThenAtBobErTilmeldtModul()
        {
            erBobAdded.Should().Be(1); 
        }

        [Given(@"\[Participant kan ikke oprettes to gange]")]
        public void GivenParticipantKanIkkeOprettesToGange()
        {
            DBConnection.getDBContext().courseModules.Find(666).Registrations.Count().Should().Be(1);
            DBConnection.IsParticipantSubscribed(bob).Should().Be(false);

            //DBConnection.SubscribeTest(666, 888, bob); // burde give fejl
        }


        [Then(@"\[At bob kun kan gave (.*) tilmeldinger]")]
        public void ThenAtBobKunKanGaveTilmeldinger(int p0)
        {
            throw new PendingStepException();
        }

        [Then(@"\[At datoer er i dag eller efter]")]
        public void ThenAtDatoerErIDagEllerEfter()
        {
            throw new PendingStepException();
        }

    }
}
