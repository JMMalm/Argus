# Argus
An application monitor dashboard R&D project for my employer using .NET Core MVC and Bootstrap.

Argus' main page scrolls through a listing of the company's applications and would ideally be displayed on a communal monitor (e.g. in a department or lobby). Users can then receive near-real-time status updates of applications relevant to the team and hide irrelevant applications. For my employer specifically, each application's "card" contains a link to the internal issue-reporting suite, which is only "implied" in this public version.

## Features
* Responsive design for both desktop and mobile.
* Ability to hide unwanted application elements from display and the option to show them again.
* Auto-scroll functionality.
* (Coming soon) Auto-refresh functionality to fetch application updates at a pre-defined interval. (currently 60 seconds)

## Built With
* ASP.NET Core MVC (.NET Core 2.2)
* Bootstrap 4
* C#
* Dapper 1.60.6
* jQuery 3.3.1
* Moq 4.11.0

## Dependencies
* Microsoft.AspNetCore.Mvc.Core 2.2.5
* Microsoft.Extensions.Configuration 2.2.0
* Microsoft.Extensions.Configuration.Binder 2.2.4
* Microsoft.NET.Test.Sdk 15.9.0
* MSTest.TestAdapter 1.3.2
* MSTest.TestFramework 1.3.2

## Tools
* Visual Studio 2017
* SQL Server 13.0.4001 (localdb)

## Notes
* Since this is for my employer some elements of the application will be renamed or redacted entirely.
* You must install the .NET Core 2.2.107 SDK to work with this application in Visual Studio 2017.