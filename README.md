# Teclyn
A multiplatform C#/Asp.Net production-ready Framework using CQRS and Event-Sourcing

## Cleaning in progress
The Teclyn framework currently uses proprietary elements, and cannot be uploaded on Github in its current form for legal reasons.

It is currently undergoing some cleaning in order to remove the proprietary part.

## Features
- Easy to create CQRS Commands
- Simple Event-sourcing Event definitions
- Domain Driven Design oriented
- Dependency injection (through the Teclyn container or an external one like Unity)
- Target frameworks: .NET Core, .net 4.6, Asp.Net Core 1.0
- Can be run on Linux through dotnetcore / Asp.Net Core
- Native support of DBMS's :
  - MongoDB
  - SQL Server
  - Not persisted in-memory 
  - Text file
  - other DBMS's through the provided extensibility tools
- can be used on Asp.Net MVC websites as well as on console apps or libraries
- Helpers allowing you to easily use call Commands in the Asp.Net MVC/Javascript layer
- Embedded Asp.Net diagnostics/monitoring tools
- Payment platform tools (Paypal / bank transfer)
- User account management
