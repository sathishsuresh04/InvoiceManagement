# Invoice Management System

## Description

The Invoice Management System is a robust component designed to manage all invoices for a company. It allows for
efficient querying and management of invoice data, supporting functionalities such as fetching the total value of
specific invoices, calculating the total value of all unpaid invoices, and generating detailed item reports over
specified periods.

## What Is Implemented

### Endpoints

1. **Create Invoice**:

- **Endpoint**: `POST /`
- **Description**: Creates a new invoice with the provided details.

2. **Retrieve Total Amount of an Invoice by ID**:

- **Endpoint**: `GET /{invoiceId:guid}`
- **Description**: Retrieves the total amount for a specified invoice ID.

3. **Get Items Report**:

- **Endpoint**: `GET /items-report`
- **Description**: Retrieves a report of items within the specified date range.

4. **Get Total of Unpaid Invoices**:

- **Endpoint**: `GET /unpaid-total`
- **Description**: Retrieves the total amount of all unpaid invoices.

### Unit Tests

- Basic unit tests are included to cover primary functionalities, ensuring critical parts of the system work as
  expected.

### Code Quality

- The codebase has been refactored to follow best practices, such as:
  - Dependency Injection
  - Proper exception handling
  - Adherence to SOLID principles
  - Unit testing for essential functionality

## Tech Stack

- Clean Architecture
- Domain-Driven Design (DDD)
- Repository Pattern
- Result Object Pattern
- Command Query Responsibility Segregation (CQRS) Pattern with MediatR library
- Minimal APIs
- Fluent Validation with a Validation Pipeline Behaviour on top of MediatR
- Postgres DB
- Structured Logging with Serilog
- Unit Testing with NSubstitute
- .Net 8
- Centralized package management and build managment

## Third-Party NuGet Packages
- **Ardalis.GuardClauses**: Gaurd cluases
- **FluentAssertions**: For expressive assertions in unit tests.
- **NSubstitute**: For creating mock objects in tests.
- **Serilog**: For logging purposes.
- **Humanizer**: For humanizing strings and improving readability.
- **MediatR**: For handling application requests and commands.
- **Bogus**: Fake data generation
## Database

The project uses a portable version of Postgres as the default persistence layer, chosen for its ease of setup and
flexibility. However, alternative databases can be used based on specific requirements, with an explanation provided for
the choice made.

## Example Usage

### Creating a New Invoice

- **Endpoint**: `POST /`
- **Description**: Create a new invoice by providing necessary details like number, seller, buyer, description, and
  issued date.

### Retrieve Total Amount of an Invoice by ID

- **Endpoint**: `GET /{invoiceId:guid}`
- **Description**: Fetch the total amount for an invoice specified by its unique ID.

### Generate Items Report

- **Endpoint**: `GET /items-report`
- **Description**: Get a report of items within a specified date range.

### Retrieve Total of Unpaid Invoices

- **Endpoint**: `GET /unpaid-total`
- **Description**: Retrieve the total amount of all unpaid invoices within the system.

## License

This project is licensed under the MIT License.
