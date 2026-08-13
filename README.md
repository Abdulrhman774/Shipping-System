# 🚚 Shipping Management System

A comprehensive **Shipping Management System** built with **.NET 8**, following **N-Tier Architecture with Clean Architecture Principles**.

The system is designed to manage **shipments, users, carriers, payments, subscriptions, addresses, shipping rates, and logistics operations** through a scalable, secure, and maintainable architecture.

The project goes beyond basic CRUD operations by implementing real-world software engineering practices such as **Result Pattern, Repository & Unit of Work, JWT Authentication, Refresh Token Rotation, Dynamic Authorization, Soft Delete, Audit Trail, SQL Views, and centralized business rules**.

---

## 📋 Project Status

This project is **completed and fully functional**, with the core shipping, authentication, subscription, billing, and logistics management features implemented.

| Feature | Status |
|---|---|
| ✅ Authentication & Authorization | Complete |
| ✅ User Management | Complete |
| ✅ Shipment Management | Complete |
| ✅ Shipment Tracking | Complete |
| ✅ Distance Calculation | Complete |
| ✅ Shipping Rate Calculator | Complete |
| ✅ Subscription System | Complete |
| ✅ Payment Methods | Complete |
| ✅ Carrier Management | Complete |
| ✅ Address Management | Complete |
| ✅ Audit Trail | Complete |
| ✅ Role & Permission Management | Complete |
| ✅ Analytics & Dashboard Views | Complete |
| ✅ API Documentation | Complete |
---

# 🌟 Architectural Highlights

This project goes beyond basic CRUD by applying several software engineering patterns and practices.

### 🔹 Result Pattern

Business operations use a centralized **Result Pattern** instead of relying on exceptions for expected business failures.

This provides:

* Predictable business logic flow
* Structured error handling
* Consistent API responses
* Separation between business errors and unexpected system exceptions
* Reduced usage of `throw new Exception` for normal business rules

---

### 🔹 Unit of Work & Generic Repository

The DAL uses **Generic Repository** and **Unit of Work** patterns to abstract Entity Framework Core operations.

Benefits include:

* Centralized data access
* Reusable repository operations
* Transaction management
* Atomic operations
* Better separation between Business Logic and EF Core

Critical operations can therefore follow an **All-or-Nothing** transaction model.

---

### 🔹 Smart API Client

The MVC UI contains a custom **`GenericApiClient`** responsible for communicating with the Web API.

It handles:

* HTTP requests
* JWT attachment
* API response handling
* `401 Unauthorized` interception
* Automatic access-token renewal
* Refresh Token Rotation
* Retrying the original request after successful token refresh

This allows the UI to maintain authentication without unnecessarily interrupting the user's session.

---

### 🔹 Dynamic Permission Matrix

Authorization is not limited to simple role checks.

The system supports a **dynamic policy-based permission model** that can map roles to specific:

* MVC Controllers
* Actions
* Operations

An `AdminPolicyHandler` is used to evaluate whether the current user has permission to perform a specific operation.

This makes the authorization system more flexible than traditional role-only authorization.

---

### 🔹 Soft Delete & Audit Trail

Entities inherit from a shared `BaseEntity`, allowing centralized management of common fields and lifecycle information.

The system supports:

* Soft Delete
* Created By
* Created Date
* Updated By
* Updated Date
* Entity status
* Status history

EF Core **Global Query Filters** can be used to automatically exclude soft-deleted records from normal queries.

---

### 🔹 SQL Views & Analytics

For reporting and dashboard operations, the system uses **SQL Views** mapped to **EF Core Keyless Entities**.

This allows frequently used analytical queries to be optimized at the database level.

Examples include:

* Shipment statistics
* Financial information
* Operational analytics
* Dashboard summaries

---

# ✨ Core Features

## 🛡️ Security & Identity

* 🔐 JWT-based Authentication
* 🔄 Refresh Token Rotation
* 🔑 Access Token renewal
* 🚪 Logout support
* 👥 Role-Based Access Control
* 🎯 Dynamic Controller/Action permissions
* 🛡️ ASP.NET Core Identity
* 🔒 Policy-based authorization

### Supported Roles

The system supports multiple operational roles:

* `Admin`
* `OpManager`
* `Reviewer`
* `Op`
* `User`

Each role can have different permissions depending on the operation being performed.

---

# 📦 Shipment Lifecycle Management

The shipment module contains business rules for managing the complete shipment lifecycle.

### Shipment State Machine

Shipments follow controlled state transitions:

```text
Created
   ↓
Approved
   ↓
ReadyForShip
   ↓
Shipped
   ↓
Delivered
```

The state machine prevents invalid status transitions and keeps the shipment lifecycle predictable.

### Shipment Features

* Create shipment
* Update shipment
* Retrieve shipment
* Delete shipment
* Unique tracking number generation
* Shipment status management
* Status history
* Sender information
* Receiver information
* Address management
* Carrier assignment
* Shipping type
* Packaging type
* Rate calculation

---

# 💰 Dynamic Shipping Rate Calculator

The system contains a business-driven shipping rate calculator.

The shipping cost can be calculated using factors such as:

* Actual weight
* Package dimensions
* Volumetric weight
* Distance
* Shipping type
* Shipping-specific factors

### Volumetric Weight

The system can compare actual weight against volumetric weight to determine the chargeable weight.

This allows the system to handle real-world shipping scenarios where package size can be more important than physical weight.

---

# 💎 Subscription System

Users can purchase subscription packages that provide predefined quotas.

Packages can include limits for:

* 📦 Shipment count
* ⚖️ Total weight
* 📍 Total distance

When a shipment is created, the system validates the user's available subscription usage and consumes the required quota.

This creates a business rule layer around shipment creation rather than treating shipments as simple database records.

---

# 💳 Payments & Billing

The system supports payment-related entities and operations.

### Payment Features

* Payment method management
* Payment method CRUD
* Commission tracking
* Payment information associated with business operations
* Subscription-related payment handling

---

# 🚚 Logistics Management

The system provides management for several logistics entities:

* 🚛 Carriers
* 🌍 Countries
* 🏙️ Cities
* 📦 Shipping Types
* 📦 Packaging Types
* 💳 Payment Methods
* 📍 Addresses

These entities are exposed through dedicated API endpoints and are used by the shipment and billing modules.

---

# 📊 Usage Analytics

Subscription usage is tracked based on business metrics such as:

* Number of shipments
* Total shipment weight
* Total shipping distance

The system can use these values to determine whether a user still has available subscription capacity.

---

# 🧾 Audit & Transaction Logging

The system maintains business and operational history through centralized logging and audit information.

This includes:

* Entity creation
* Entity updates
* Entity deletion
* User responsible for changes
* Shipment status history
* Transaction-related information

Logging is implemented using **Serilog**, with support for SQL Server logging.

---

# 🏗️ Project Architecture

The solution follows an **N-Tier Architecture inspired by Clean Architecture principles**.

```text
Shipping-System/
│
├── Domain/
│   ├── Entities/
│   ├── Enums/
│   └── Shared/
│
├── DAL/
│   ├── Configurations/
│   ├── Context/
│   ├── Contracts/
│   ├── Repositories/
│   ├── Migrations/
│   └── Seeding/
│
├── BL/
│   ├── Services/
│   ├── Services/Shipment/
│   ├── DTOs/
│   ├── Common/
│   ├── Contract/
│   ├── Mapping/
│   └── Validators/
│
├── WebApi/
│   ├── Controllers/
│   ├── Extensions/
│   ├── Services/
│   └── Program.cs
│
├── UI/
│   ├── Areas/
│   ├── Controllers/
│   ├── Services/
│   ├── Models/
│   ├── Views/
│   └── wwwroot/
│
└── Shipping-System.sln
```

---

# 🧩 Architecture Layers

### Domain

Contains the core business entities and shared domain concepts.

Responsibilities:

* Entities
* Enums
* Base entities
* Domain-level results
* Core business concepts

---

### DAL — Data Access Layer

Responsible for persistence and database communication.

Includes:

* Entity Framework Core
* DbContext
* Entity configurations
* Generic repositories
* Unit of Work
* Migrations
* Database seeding
* SQL Views

---

### BL — Business Logic Layer

Contains the application's business rules and application services.

Includes:

* Business services
* DTOs
* AutoMapper profiles
* FluentValidation
* Result Pattern
* Shipment-specific business logic
* Subscription validation
* Rate calculation

---

### WebApi

Provides the RESTful API consumed by the frontend and potentially other clients.

Includes:

* API Controllers
* JWT Authentication
* Authorization
* Token services
* Global exception handling
* Rate limiting
* Swagger / OpenAPI
* Serilog integration

---

### UI

ASP.NET Core MVC frontend using Razor Views.

Includes:

* MVC Controllers
* Razor Views
* Admin Areas
* ViewModels
* API client services
* Session / Cookie authentication handling
* Bootstrap UI
* JavaScript

---

# 🛠️ Tech Stack

## Backend

| Technology                | Purpose                    |
| ------------------------- | -------------------------- |
| **.NET 8**                | Application framework      |
| **ASP.NET Core Web API**  | REST API                   |
| **ASP.NET Core MVC**      | Frontend                   |
| **Entity Framework Core** | ORM / Data Access          |
| **SQL Server**            | Database                   |
| **ASP.NET Core Identity** | User & Identity management |
| **JWT Bearer**            | Authentication             |
| **AutoMapper**            | Object mapping             |
| **FluentValidation**      | Request validation         |
| **Serilog**               | Logging                    |
| **Swagger / OpenAPI**     | API documentation          |

## Design Patterns & Practices

| Pattern / Practice                | Usage                                  |
| --------------------------------- | -------------------------------------- |
| **N-Tier Architecture**           | Application separation                 |
| **Clean Architecture Principles** | Dependency & responsibility separation |
| **Repository Pattern**            | Data access abstraction                |
| **Unit of Work**                  | Transaction management                 |
| **Result Pattern**                | Business error handling                |
| **Options Pattern**               | Configuration management               |
| **Dependency Injection**          | Service composition                    |
| **Policy-Based Authorization**    | Dynamic permissions                    |
| **Soft Delete**                   | Safe record deletion                   |
| **Global Query Filters**          | Automatic soft-delete filtering        |
| **SQL Views**                     | Analytics / reporting                  |
| **Keyless Entities**              | Mapping database views                 |

---

# 🎨 Frontend

The frontend is implemented using:

* **ASP.NET Core MVC**
* **Razor Views**
* **Bootstrap 5**
* **jQuery**
* **FontAwesome**
* **Session**
* **Cookies**
* Custom `GenericApiClient`

The UI communicates with the Web API rather than directly accessing the database.

---

# 🚀 Quick Start

## Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server Express
* Visual Studio 2022 or VS Code

---

## 1. Clone the Repository

```bash
git clone https://github.com/Abdulrhman774/Shipping-System.git
cd Shipping-System
```

---

## 2. Configure the Database

Update the connection string in:

```text
WebApi/appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ShippingDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## 3. Apply Database Migrations

```bash
cd WebApi
dotnet ef database update
```

Alternatively, a database SQL script is available:

```text
Database/ShippingDB.sql
```

---

## 4. Run the Web API

```bash
cd WebApi
dotnet run
```

Swagger will be available under:

```text
/swagger
```

---

## 5. Run the MVC UI

Open another terminal:

```bash
cd UI
dotnet run
```

For development in Visual Studio, both **WebApi** and **UI** can be configured as multiple startup projects.

---

# 🔑 Default Admin Credentials

The database is seeded with a default administrator account.

| Username          | Password      | Role    |
| ----------------- | ------------- | ------- |
| `admin@gmail.com` | `Admin@12345` | `Admin` |

> ⚠️ Change or remove default credentials before using the system in a production environment.

---

# 📊 API Overview

The API is documented using Swagger / OpenAPI.

## 🔐 Authentication

| Method | Endpoint                        | Description                 |
| ------ | ------------------------------- | --------------------------- |
| POST   | `/Api/Auth/Register`            | Register a new user         |
| POST   | `/Api/Auth/Login`               | Authenticate user           |
| POST   | `/Api/Auth/RotateRefreshToken`  | Rotate refresh token        |
| POST   | `/Api/Auth/Refresh-AccessToken` | Generate a new access token |
| POST   | `/Api/Auth/Logout`              | Logout user                 |

---

## 📦 Shipment

| Method | Endpoint                       | Description        |
| ------ | ------------------------------ | ------------------ |
| GET    | `/Api/Shipment`                | Get shipments      |
| GET    | `/Api/Shipment/{id}`           | Get shipment by ID |
| POST   | `/Api/Shipment/CreateShipment` | Create shipment    |
| PUT    | `/Api/Shipment/{id}`           | Update shipment    |
| DELETE | `/Api/Shipment`                | Delete shipment    |

---

## 📋 Lookup APIs

| Method | Endpoint             | Description         |
| ------ | -------------------- | ------------------- |
| GET    | `/Api/City`          | Get cities          |
| GET    | `/Api/Country`       | Get countries       |
| GET    | `/Api/ShippingType`  | Get shipping types  |
| GET    | `/Api/PaymentMethod` | Get payment methods |
| GET    | `/Api/Carrier`       | Get carriers        |

Additional lookup and management endpoints are available through Swagger.

---

# 🧪 Testing

Run the available tests with:

```bash
dotnet test
```

API requests can also be tested using the provided HTTP files:

```text
WebApi/WebApi.http
WebApi/test.http
```

---

# 🚀 Future Enhancements

Although the current system is complete and fully functional, the following features can be added in future versions:

- [ ] Payment gateway integration
- [ ] Email / SMS notification system
- [ ] PDF / Excel advanced reporting
- [ ] Real-time tracking with SignalR
- [ ] Mobile application (React Native / Flutter)
- [ ] Multi-language support
- [ ] Advanced analytics and reporting
---

# 🤝 Contributing

Contributions, issues, and feature requests are welcome.

1. Fork the repository
2. Create your feature branch

```bash
git checkout -b feature/AmazingFeature
```

3. Commit your changes

```bash
git commit -m "Add AmazingFeature"
```

4. Push the branch

```bash
git push origin feature/AmazingFeature
```

5. Open a Pull Request

---

# 📧 Contact

**Developer:** Abdulrhman Azmy

**GitHub:** [@Abdulrhman774](https://github.com/Abdulrhman774)

**Project:** Shipping Management System

**Built With:** ❤️ + .NET 8

---

# 📄 License

This project is licensed under the **MIT License**.

---

## ⭐ Project Summary

The **Shipping Management System** is a .NET 8 logistics platform designed with scalability, maintainability, and real-world business rules in mind.

It combines:

* 🔐 Secure authentication
* 🎯 Dynamic authorization
* 📦 Shipment lifecycle management
* 💰 Dynamic rate calculation
* 💎 Subscription quotas
* 🚚 Carrier management
* 💳 Payment management
* 📊 Analytics and SQL Views
* 🧾 Audit trails
* 🔄 Refresh Token Rotation
* 🏗️ N-Tier / Clean Architecture principles

> **Shipping Management System — Built with .NET 8 and Clean Architecture Principles.** 🚀
