# Teclyn
Teclyn is a multiplatform C#/Asp.Net production-ready toolkit using CQRS and Event-Sourcing.

## Work in progress
Teclyn is not fully useable yet.

The first official release should happen between August and Octobre 2016. The version number will change to 1.x at this point.

## Philosophy
Teclyn is:
- A toolkit: it is not a restrictive framework
- Designed to be useable as quick as possible: one configuration class, one init instruction, and you're set)
- Modular: use only the modules you need
- Extensible: develop your own modules and connectors

## History
After several years designing CQRS/DDD architectures for different companies, I decided it was time to release an entirely rewritten toolkit as open source.

I have used similar tools in the following contexts:
- Big companies
- Startups
- IT service company
- Banks
- Customer Relationship Management Systems (CRM)
- Content Management System (CMS)
- News websites
- Back office tools (automatic backup, massive emailing)

Teclyn is the result of more than 5 years of thinking about these types of architecture. 
Teclyn is the 4th entirely rewritten version of these tools, and the first open source one.

## Features
All the features have not been finalized yet, but you will soon find these:
- Easy to create CQRS Commands
- Simple Event-sourcing Event definitions
- Domain Driven Design oriented
- Datasource abstraction
- Dependency injection (through the Teclyn container or an external one like StructureMap)
- Target frameworks: .NET Core, .net 4.6, Asp.Net Core 1.0
- Can be run on Linux through dotnetcore / Asp.Net Core
- Native support of DBMS's :
  - MongoDB
  - SQL Server
  - Not persisted in-memory 
  - Text file
  - other DBMS's through the provided extensibility tools
- Can be used on Asp.Net MVC websites as well as on console apps or libraries
- Helpers allowing you to easily use Commands in the Asp.Net MVC/Javascript layer
- Embedded Asp.Net diagnostics/monitoring tools
- User account management