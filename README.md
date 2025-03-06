# RapidPay

## Overview

RapidPay is a fast-growing payment provider. This project implements the Authorization System for RapidPay, which consists of three main modules: Card Management, Card Authorization, and Payment Fees.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop)

### Setting Up the Project

1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/RapidPay.git
   cd RapidPay
   ```

2. Build and run the Docker containers:
   ```sh
   docker-compose build
   docker-compose up
   ```

   This will start the application and the SQL Server database in separate containers. The API will be accessible at `http://localhost:5000`.

### API Documentation

The API documentation is available via Swagger UI. To access the Swagger UI, navigate to `http://localhost:5000` in your browser.

#### Card Management Module

1. **Create Card**
   - `POST /api/cards`
   - Request Body: `{ "creditLimit": 1000 }`
   - Response: `200 OK` with card details

2. **Authorize Card**
   - `POST /api/cards/{cardId}/authorize`
   - Response: `200 OK` if authorized

3. **Pay with Card**
   - `POST /api/cards/{cardId}/pay`
   - Request Body: `{ "amount": 100 }`
   - Response: `200 OK` with transaction details

4. **Get Card Balance**
   - `GET /api/cards/{cardId}/balance`
   - Response: `200 OK` with card balance

5. **Update Card Details**
   - `PUT /api/cards/{cardId}`
   - Request Body: `{ "balance": 500, "creditLimit": 1000, "isActive": true }`
   - Response: `200 OK` with updated card details

#### Card Authorization Module

- **Authorize Card**
  - `POST /api/authorization/{cardId}`
  - Response: `200 OK` if authorized

#### Payment Fees Module

1. **Get Current Fee**
   - `GET /api/fees`
   - Response: `200 OK` with current fee

2. **Update Fee**
   - `POST /api/fees/update`
   - Response: `200 OK` if fee updated successfully

### Additional Information

- Secure authentication is implemented using JWT.
- The database schema and ORM usage are optimized for high efficiency.
- Thread safety considerations are applied for concurrent transactions.

### Unit Tests

Unit tests are included in the `Tests` project. To run the tests, use the following command:

```sh
dotnet test
```

### Architectural Choices

- The project is structured to separate concerns into different services and controllers.
- Dependency injection is used to manage service lifetimes and dependencies.
- Entity Framework Core is used for data access.

### Thread Safety and Security Improvements

- Thread safety is ensured by using appropriate locking mechanisms and ensuring that DbContext instances are not shared across threads.
- Secure authentication is implemented using JWT, which provides a more secure method than basic authentication.

We hope you find this implementation useful. Happy coding!