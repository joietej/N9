# DotNet9WebApiSample

![License](https://img.shields.io/badge/license-MIT-blue.svg)

**DotNet9WebApiSample** is a .NET 9 Minimal Web API application emphasizing scalability, resilience, and security.

## 📖 Overview

This project demonstrates a structured approach to building a web API using .NET 9, incorporating:

- **Data Layer**: Utilizes Entity Framework Core with SQL Server for data management and migrations.
- **Service Layer**: Contains business logic interfacing between the API and data layers.
- **API Layer**: Exposes endpoints, integrates middleware, and manages authentication and authorization.

## 🛠️ Features

- **Entity Framework Core**: Database interactions and migrations with SQL Server.
- **OpenAPI (Scaler)**: API documentation and testing interface.
- **Polly**: Implements resilience strategies like retries and circuit breakers.
- **Middleware**:
    - HTTP request logging.
    - Global exception handling with standardized problem details.
- **Authentication & Authorization**:
    - Azure Active Directory integration.
    - JWT-based authentication.

## 📂 Project Structure

```plaintext
├── src/
│   ├── Data/               # Data access layer
│   ├── Services/           # Business logic layer
│   └── Api/                # API layer
├── tests/                  # Unit and integration tests
├── N9.sln                  # Solution file
└── README.md               # Project overview
```

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Docker (optional)
- Azure Active Directory setup for authentication

### Setup Instructions

1. **Clone the repository**:

   ```bash
   git clone https://github.com/joietej/DotNet9WebApiSample.git
   cd DotNet9WebApiSample
   ```

2. **Configure the database connection string**:

   Update the `ConnectionStrings:Sql` in `appsettings.json` with your SQL Server details.

3. **Apply Entity Framework Core migrations**:

   ```bash
   dotnet ef database update --project src/Data
   ```

4. **Run the application**:

   ```bash
   dotnet run --project src/N9.Api
   ```

5. **Access the API documentation**:

    Navigate to `http://localhost:5295/api-docs/v1` in your browser.


## 🐳 Run as a Docker Container

   To run the API as a Docker container, follow the steps below:

- **Build the Docker image**:

  In the root of your repository, run the following command to build the Docker image:

   ```bash
   docker build -t dotnet9webapisample .
   docker run -d -p 5295:8080 --name dotnet9webapisample dotnet9webapisample
   ```

## ⚙️ Configuration

### Authentication

Configure Azure AD settings in `appsettings.json`:

```json
"AzureAd": {
"Instance": "https://login.microsoftonline.com/",
"Domain": "yourdomain.com",
"TenantId": "your-tenant-id",
"ClientId": "your-client-id",
"CallbackPath": "/signin-oidc"
}
```

### Middleware

- **Polly**: Configured for transient fault handling.
- **HTTP Logging**: Enabled for request and response logging.
- **Exception Handling**: Standardized error responses using Problem Details.

## 🤝 Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## 📝 License

This project is licensed under the MIT License.
