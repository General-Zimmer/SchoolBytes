using System;
using TechTalk.SpecFlow;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class MinNyeTestStepDefinitions
    {

        [Given(@"a name '(.*)'")]
        public void GivenANameTestname(string name)
        {
            throw new PendingStepException();
        }

        [When(@"use as parameter in method")]
        public void WhenUseAsParameterInMethod()
        {
            throw new PendingStepException();
        }

        [Then(@"result should be")]
        public void ThenResultShouldBe()
        {
            throw new PendingStepException();
        }
    }
}
