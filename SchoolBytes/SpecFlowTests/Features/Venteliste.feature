Feature: Venteliste

Når et kursusgang/CourseModule er fuld booket, bliver man registreret
på en venteliste når man vil tilmelde sig modulet

@waitlist
Scenario: Add a new participant to the waitlist
	Given the waitlist already contains at least one participant
	When the participant signs up
	Then they should be last on the waitlist
