# Contact Details Manager

An ASP.NET Core MVC application for managing contacts, built on Clean Architecture with three layers of automated tests.

## Features

- Create, view, edit and delete contacts
- Search and filter by name, email, country
- Sorting on any column
- Export to PDF / CSV / Excel
- Authentication and roles via ASP.NET Core Identity

## Architecture

The solution is split into three projects following Clean Architecture:

- **Core** — domain entities, DTOs, service interfaces and business logic,
  with no dependency on infrastructure or the web layer
- **Infrastructure** — Entity Framework Core, repositories and the SQL Server
  data context
- **UI** — ASP.NET Core MVC controllers and Razor views

Dependencies point inward: the UI depends on Core, Infrastructure implements Core's interfaces, and Core depends on nothing.

## Testing

Three test projects cover different layers:

- **ServiceTests** — unit tests for service-layer logic, using Moq to isolate repository dependencies
- **ControllerTests** — unit tests for MVC controller behaviour
- **IntegrationTests** — end-to-end tests against the running application

## Stack

ASP.NET Core MVC · Razor Views · Entity Framework Core · SQL Server · Clean Architecture · xUnit · Moq · FluentAssertions - AutoFixture

## Running locally
