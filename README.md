# Welcome MPService 👋 
MPService is a .NET Core microservice designed for managing Malayan Prints System for employee authentication, shift logs, and payroll computation. Built with a Domain-Driven Design (DDD) architecture, it separates concerns and models core business logic explicitly, making the system scalable, maintainable, and aligned with real-world business processes.

🧭 Domain-Driven Design (DDD)
MPService is structured around DDD building blocks to reflect core business domains:

Domain Layer – Contains rich domain models, aggregates, value objects, and business rules.

Application Layer – Coordinates tasks and delegates domain work. Implements use cases.

Infrastructure Layer – Manages external concerns like database access and identity providers.

Presentation/API Layer – Exposes HTTP endpoints for client communication.

This structure helps keep business logic isolated from infrastructure concerns, resulting in a more maintainable codebase.


🚀 Features
🔐 Authentication – Secure login system using JWT

🕒 Shift Logs – Accurate tracking of employee working days.

💰 Payroll Computation – Calculates wages based on day logs and configurable pay rules.

🧩 Modular DDD Architecture – Loosely coupled components for better testability and scalability.

🛡️ Role-Based Access Control (RBAC) – Fine-grained authorization using roles.

🛠️ Tech Stack
Language: C# (.NET Core 9)

Architecture: Domain-Driven Design (DDD)

Auth: JWT

ORM: Entity Framework Core

Database: MS SQL Server

API Docs: Scalar (OpenAPI)

## Get a fresh project

📦 Getting Started
1. Clone the Repo
   ```bash
   git clone https://github.com/jamesxddev/MPService.git
   cd MPService
   ```

2. Setup Database
Update appsettings.json with your connection string.
   ```json
   "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=MPServiceDb;..."
    }
   ```

Migrations will automatically apply when the application starts.

3. Run the App
```bash
dotnet run
```

Scalar UI will be available at https://localhost:44305/scalar/v1

📈 Roadmap
 Leave management module

 Time-off approvals

 Notification system

 Admin dashboard UI

