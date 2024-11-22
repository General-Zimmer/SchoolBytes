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

        [Given(@"\[Participant bob tilmeldes modul tilhørende rigtige course]")]
        public void GivenParticipantBobTilmeldesModulTilhorendeRigtigeCourse()
        {
            //Participant bob = new Participant("Bob", "69695512" );

            DBConnection.SubscribeTest(666, 888, bob);

            throw new PendingStepException();
        }

        [When(@"\[Oprettes en instans i databasen]")]
        public void WhenOprettesEnInstansIDatabasen()
        {
            int erBobAdded = DBConnection.GetSubscribeCount(bob);
            throw new PendingStepException();
        }

        [Then(@"\[At bob er tilmeldt modul]")]
        public void ThenAtBobErTilmeldtModul()
        {
            DBConnection.GetParticipantFromModul(bob);
            throw new PendingStepException();
        }
    }
}
