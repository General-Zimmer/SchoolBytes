Feature: Tilmelding

A short summary of the feature

@tag1
Scenario: [Oprettelse af tilmelding]
	Given [Participant bob tilmeldes modul tilhørende rigtige course]
	When [Oprettes en instans i databasen]
	Then [At bob er tilmeldt modul]
Scenario: [en participant kan kun tilmeldes en gang]
	Given [Participant kan ikke oprettes to gange]
	Then [At bob kun kan gave 5 tilmeldinger]
	Then [At datoer er i dag eller efter]
