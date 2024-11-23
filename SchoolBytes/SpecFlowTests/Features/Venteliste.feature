Feature: Venteliste

A short summary of the feature

@tag1
Scenario: A Ventelisten opretholder en korrekt rækkefølge
	Given Participant Bob, et CourseModule cm og venteliste på cm med anden participant
	When Bob bliver tilmeldt ventelisten
	Then Bob bliver tilmeldt ventelisten og relationerne findes i DB

Scenario: B Man må ikke være på ventelisten flere gangeg
	Given Participant Bob der er tilmeldt ventelisten
	When Bob vil tilmelde sig ventelisten igen
	Then Bliver den nye registration ikke oprettet i DB

Scenario: C Når cm er fuld booket kommer man på ventelisten
	Given Participant Bob og et cm der er fuld booket
	When Bob vil blive tilmeldt cm
	Then Bob bliver tilmeldt ventelisten og står som den første

Scenario: D Når cm ikke er fuld booket kommer man ikke på ventelisten
	Given Participant Bob og cm med ledige pladser
	When Bob vil gerne tilmelde sig cm
	Then Bob bliver ikke tilmeldt ventelisten men cm

Scenario: E Man bliver fjernet fra ventelisten når man kommer på cm
	Given Participant Bob, der allerede er på ventelisten
	When Der er en ledig plads
	Then Bob er tilmeldt cm og står ikke længere på ventelisten
