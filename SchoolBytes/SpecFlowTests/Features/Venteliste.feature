Feature: Venteliste

A short summary of the feature

@tag1
Scenario: A Ventelisten opretholder en korrekt rækkefølge
	Given Participant Bob, et CourseModule cm1 og venteliste på cm1 med 1 anden participant
	When Bob bliver tilmeldt ventelisten på cm1
	Then Er Bob tilmeldt ventelisten og relationerne findes i DB

Scenario: B Man må ikke stå på ventelisten flere gange
	Given Participant Bob der er tilmeldt CourseModule cm1s venteliste
	When Bob bliver tilmeldt ventelisten på cm1 igen
	Then Er der kun en registration i DB

Scenario: C Man kommer på ventelisten når courseModule er fuld booket
	Given Participant Bob og et CourseModule med maxed out kapacitet
	When Bob bliver tilmeldt cm1
	Then Bob bliver tilmeldt ventelisten på cm1 som nr.1

Scenario: D Man kommer ikke på ventelisten hvis cm1 ikke er fuld booket
	Given Participant Bob og et CourseModule med ledige pladser
	When Bob bliver tilmeldt cm1
	Then Bob bliver tilmeldt cm1 og ikke ventelisten

Scenario: E Man bliver fjernet fra ventelisten når man kommer på cm1
	Given Participant Bob der allerede står på cm1s venteliste
	When Der kommer en ledig plads på cm1
	Then Bob bliver tilmeldt cm1 og fjernet fra ventelisten
