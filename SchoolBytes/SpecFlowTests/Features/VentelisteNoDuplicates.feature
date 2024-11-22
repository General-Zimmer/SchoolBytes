Feature: VentelisteNoDuplicates

A short summary of the feature

@waitlist
Scenario: Add an already existing participant to the waitlist
	Given the waitlist already contains the same participant
	When the participant tries to sign up to the waitlist again
	Then they should not be added to the waitlist