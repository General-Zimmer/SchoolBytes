Feature: Tilmelding

A short summary of the feature

@tag1
Scenario: Oprettelse af tilmelding
	Given Participant Bob og et CourseModule cm1
	When Bob bliver tilmeldt cm1
	Then Er Bob tilmeldt og relationerne findes i DB

Scenario: En participant kan kun tilmeldes et CourseModule en gang
	Given Participant Bob der er tilmeldt CourseModule cm1
	When Bob bliver tilmeldt cm1 igen
	Then Er der kun en registration i DB

Scenario: En participant kan kun tilmeldes fremtidige kurser
	Given Participant Bob og et CourseModule med forbigået dato
	When Bob bliver tilmeldt A
	Then Er der ikke en ny registration i DB A


Scenario: En participant med 5 aktive tilmeldte kurser, kan ikke tilmeldes
	Given Participant Bob med 5 aktive tilmeldte kurser
	When Bob bliver tilmeldt B
	Then Er der ikke en ny registration i DB B


Scenario: En participant med fejl formateret data, tilmeldes ikke
	Given Particpant Bobby med forkert format på properties
	When Bobby bliver tilmeldt
	Then Er der ikk en ny registration i DB C

Scenario: En participant operettes ikke, hvis den allerede findes
	Given Bob2 med samme telefonnummer som Bob
	When Bob2 bliver tilmeldt
	Then Er der hverken en ny registration eller ny participant
