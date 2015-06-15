Feature: Formatting
	In order to support CI/CD with GitHub
	As an API
	I want to accept Push Events from request body

@normal
Scenario: Call push endpoint when push event is in request body
	Given I have received a HttpRequestMessage with payload for the push event
	Then I am routed to the correct controller method
