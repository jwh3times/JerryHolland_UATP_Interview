# RapidPay

## Overview

Welcome to RapidPay! We are excited to have you explore our system, which is designed to support a fast-growing payment provider. This project includes three fundamental modules: Card Management, Card Authorization, and Payment Fees. Each module is meticulously crafted to ensure efficient, secure, and reliable payment processing.

## Getting Started

Before you begin, there are a few essential tools and prerequisites you'll need to have installed on your system. Don't worry, we'll guide you through every step of the setup process.

### Prerequisites

To run this project, you'll need to have the following software installed:

- **.NET 8 SDK:** This is the software development kit you'll need to build and run our C# application. You can download it from the [.NET official website](https://dotnet.microsoft.com/download/dotnet/8.0).
- **Docker:** We use Docker to containerize the application and its dependencies, making it easier to manage and deploy. You can download Docker from the [Docker official website](https://www.docker.com/products/docker-desktop).

### Setting Up the Project

Now that you have the necessary prerequisites, let's get the project up and running. Follow these detailed steps:

1. **Clone the Repository:**
    First, you'll need to copy the repository to your local machine. Open your terminal or command prompt and run the following commands:
    ```sh
    git clone https://github.com/jwh3times/JerryHolland_UATP_Interview.git
    cd JerryHolland_UATP_Interview
    ```

2. **Generate a .pfx Certificate:**
    To run the application securely, you'll need to generate a `.pfx` certificate. You can do this using the `dotnet` CLI. Run the following command in your terminal from the root directory of the repo:
    ```sh
    dotnet dev-certs https -ep RapidPay/RapidPay.pfx -p <your-password> --trust
    ```
    Replace `<your-password>` with a secure password of your choice. This will generate a `RapidPay.pfx` file that you can use for secure communication.

3. **Build and Run Docker Containers:**
    Next, we'll use Docker to build and run the containers for both the application and the database. This is a two-step process:
    ```sh
    docker-compose build
    docker-compose up
    ```

    By running these commands, Docker will start the application and the SQL Server database in separate containers. You can then access the API at `https://localhost:5001`.

## API Documentation

We have comprehensive API documentation available via Swagger UI. This intuitive interface makes it easy to explore and test the different endpoints. To access the Swagger UI, simply navigate to `https://localhost:5001` in your browser.

### User Authorization Module

The User Authorization module manages the authentication of users for RapidPay. Here are the relevant endpoints:

1. **Register New User**
    - **Endpoint:** `POST /api/user/register`
    - **Description:** This endpoint registers a new user.
    - **Request Body:**
    ```json
    { "Username": "{username}", "Password": "{plain text password}" }
    ```
    - **Response:** `200 OK` if the user is successfully registered.

2. **Login User**
    - **Endpoint:** `POST /api/user/login`
    - **Description:** This endpoint is used to login an existing user.
    - **Request Body:**
    ```json
    { "Username": "{username}", "Password": "{plain text password}" }
    ```
    - **Response:** `200 OK` with the users authenticated JWT token allowing access to other endpoints of RapidPay when passed in the headers of the request. This token will expire after 1 hour and the user will need to login again.

### Card Management Module

Here are the key endpoints in the Card Management module, along with detailed descriptions of their usage:

1. **Create Card**
    - **Endpoint:** `POST /api/cards`
    - **Description:** This endpoint allows you to create a new card with a specified credit limit.
    - **Request Body:** 
      ```json
      { "creditLimit": 1000 }
      ```
    - **Response:** `200 OK` with the details of the newly created card with the Card Number encrypted for security.

2. **Authorize Card**
    - **Endpoint:** `POST /api/cards/{encryptedCardNumber}/authorize`
    - **Description:** This endpoint authorizes a card for use.
    - **Response:** `200 OK` if the card is successfully authorized.

3. **Pay with Card**
    - **Endpoint:** `POST /api/cards/{encryptedCardNumber}/pay`
    - **Description:** This endpoint processes a payment using the specified card.
    - **Request Body:** 
      ```json
      { "amount": 100 }
      ```
    - **Response:** `200 OK` with the details of the transaction.

4. **Get Card Balance**
    - **Endpoint:** `GET /api/cards/{encryptedCardNumber}/balance`
    - **Description:** This endpoint retrieves the current balance of the specified card.
    - **Response:** `200 OK` with the card balance.

5. **Update Card Details**
    - **Endpoint:** `PATCH /api/cards/{encryptedCardNumber}`
    - **Description:** This endpoint updates the details of a card, such as balance, credit limit, and activation status.
    - **Request Body:** 
      ```json
      { "balance": 500, "creditLimit": 1000, "isActive": true }
      ```
    - **Response:** `200 OK` with the updated card details.

### Card Authorization Module

The Card Authorization module ensures that cards are authorized before use. This module doesn't expose any API endpoints, but rather functions as the Service to handle the authorization for the previously documented Card Authorization endpoint.

### Payment Fees Module

The Payment Fees module manages the fees associated with card transactions. Here are the relevant endpoints:

1. **Get Current Fee**
    - **Endpoint:** `GET /api/fees`
    - **Description:** This endpoint retrieves the current transaction fee.
    - **Response:** `200 OK` with the current fee details.

## Architectural Choices

We have made several thoughtful architectural decisions to ensure that this project is robust, maintainable, and scalable. Here are some of the key choices:

- **Separation of Concerns:** The project is structured to separate different concerns into individual services and controllers. This modular approach makes the codebase easier to manage, test, and extend.
- **Dependency Injection:** We use dependency injection to manage service lifetimes and dependencies. This design pattern improves testability and modularity by decoupling the creation of an object from its usage.
- **Entity Framework Core:** For data access, we use Entity Framework Core. This powerful ORM (Object-Relational Mapper) allows us to interact with the database using .NET objects, making data access more intuitive and efficient.
- **Containerization:** By deploying the RapidPay app in a container we have laid the foundation for scaling the product up in the future to handle much greater load.

## Performance Optimizations and Security Improvements

We have implemented several measures to boost performance and enhance security in the application:

- **Performance Optimization:** To boost performance, the use of .NET's async await functionality has been used extensively to prevent wasting time by waiting on I/O operations such as database access. By using Docker we also have a foundation that would allow scaling the product up using Kubernetes to deploy multiple copies of the RapidPay container.
- **Secure Authentication:** We use JSON Web Tokens (JWT) for authentication. JWT provides a more secure method compared to basic authentication, allowing for secure token exchange and validation. This ensures that only authorized users can access the API.

## Unit Tests

Unit tests are an integral part of our development process. They help us ensure that the code is working as expected and make it easier to catch bugs early. We have included unit tests in the `Tests` project. To run the tests, use the following command:
```sh
dotnet test
```

Running the tests will execute all the unit tests and provide a report on their status.

## Conclusion

We hope that this detailed README file helps you get started with the RapidPay Authorization System. If you have any questions or run into any issues, please feel free to reach out!
