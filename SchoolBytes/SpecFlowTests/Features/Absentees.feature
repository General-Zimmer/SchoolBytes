Feature: Absentees

A short summary of the feature

@tag1
Scenario: Participant med 3 udeblivelser findes i notifikation
	Given Participant med 3 udeblivelser
	When A Notifikationer hentes fra HomeController
	Then Findes Participants navn i stringen

Scenario: Participant med 3 game udeblivelser findes ikke i notifikation
	Given Participant med 3 gamle udeblivelser
	When B Notifikationer hentes fra HomeController
	Then Findes Participants navn ikke i stringen
