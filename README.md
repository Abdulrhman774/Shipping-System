# 🚚 Shipping Management System

> ⚠️ **Work in Progress** - This project is under active development. Some features may be incomplete or subject to change.

[![GitHub repo size](https://img.shields.io/github/repo-size/Abdulrhman774/Shipping-System)](https://github.com/Abdulrhman774/Shipping-System)
[![GitHub language count](https://img.shields.io/github/languages/count/Abdulrhman774/Shipping-System)](https://github.com/Abdulrhman774/Shipping-System)
[![GitHub top language](https://img.shields.io/github/languages/top/Abdulrhman774/Shipping-System)](https://github.com/Abdulrhman774/Shipping-System)
[![GitHub last commit](https://img.shields.io/github/last-commit/Abdulrhman774/Shipping-System)](https://github.com/Abdulrhman774/Shipping-System)
[![GitHub issues](https://img.shields.io/github/issues/Abdulrhman774/Shipping-System)](https://github.com/Abdulrhman774/Shipping-System)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)

---

A comprehensive **Shipping Management System** built with **.NET 8** and **N-Tier with Clean Architecture Principles. Designed to manage shipments, users, carriers, payments, subscriptions, and more with a robust **Result Pattern** for error handling.

---

## 📋 Project Status

| Feature | Status | Notes |
|---------|--------|-------|
| ✅ Authentication | Complete | JWT + Refresh Tokens |
| ✅ User Management | Complete | CRUD + Roles |
| ✅ Shipment Creation | Complete | With rate calculation |
| 🔄 Shipment Tracking | In Progress | Tracking by number |
| 🔄 Distance Calculation | In Progress | Fallback values used |
| ✅ Subscription System | Complete | Usage tracking |
| ✅ Payment Methods | Complete | CRUD operations |
| ✅ Carrier Management | Complete | CRUD operations |
| 🔄 Reporting | Planned | PDF/Excel export |
| 🔄 Notifications | Planned | Email/SMS alerts |

---

## ✨ Features

### 🛡️ Core Features
- ✅ **Authentication & Authorization** - JWT-based authentication with refresh tokens
- ✅ **User Management** - Full CRUD operations with role-based access (Admin/User)
- ✅ **Shipment Management** - Create, track, and manage shipments with unique tracking numbers
- ✅ **Rate Calculator** - Dynamic shipping rate calculation based on weight, dimensions, and distance
- ✅ **Subscription System** - Subscription packages with usage tracking (shipment count, weight, distance)
- ✅ **Payment Methods** - Multiple payment method support with commission tracking
- ✅ **Carrier Management** - Manage shipping carriers
- ✅ **Address Management** - Sender and receiver address management with default address support

### 🎯 Business Features
- 📦 **Shipment Tracking** - Track shipments via unique tracking numbers
- 💰 **Cost Calculation** - Volumetric weight calculation with distance-based pricing
- 📊 **Usage Analytics** - Track subscription usage (weight, distance, shipment count)
- 🔄 **Transaction Logging** - Full audit trail for all transactions
- 📱 **Multi-platform** - RESTful API with MVC frontend

---

## 🏗️ N-Tier Architecture (Clean Architecture Inspired)

```
Shipping-System/
│
├── Domain/                          # Core business entities and logic
│   ├── Entities/                    # Domain entities (ApplicationUser, TbShipment, etc.)
│   ├── Enums/                       # Enumerations (enEntityState, enGender)
│   └── Shared/                      # Shared domain components (BaseEntity, Results)
│
├── DAL/                             # Data Access Layer
│   ├── Configurations/              # Entity configurations (EF Core)
│   ├── Context/                     # Database context (ShippingDbContext)
│   ├── Contracts/                   # Repository contracts
│   ├── Repositories/                # Repository implementations
│   ├── Migrations/                  # EF Core migrations
│   └── Seeding/                     # Database seeding
│
├── BL/                              # Business Logic Layer
│   ├── Services/                    # Business services
│   ├── DTOs/                        # Data Transfer Objects
│   ├── Common/                      # Common utilities (ApiResponse, Results)
│   ├── Contract/                    # Service interfaces
│   ├── Mapping/                     # AutoMapper configurations
│   ├── Validators/                  # FluentValidation validators
│   └── Services/Shipment/           # Shipment-specific services
│
├── WebApi/                          # RESTful API Layer
│   ├── Controllers/                 # API controllers
│   ├── Extensions/                  # Extension methods
│   ├── Services/                    # API services (Auth, Token)
│   └── Program.cs                   # API entry point
│
├── UI/                              # Frontend Layer (MVC)
│   ├── Areas/                       # Area-based routing (Admin)
│   ├── Controllers/                 # MVC controllers
│   ├── Services/                    # Frontend services
│   ├── Models/                      # ViewModels
│   ├── Views/                       # Razor views
│   └── wwwroot/                     # Static files
│
└── README.md                        # This file
```

---

## 🛠️ Tech Stack

### Backend
| Technology | Purpose |
|------------|---------|
| **.NET 8** | Framework |
| **ASP.NET Core** | Web API & MVC |
| **Entity Framework Core** | ORM |
| **SQL Server** | Database |
| **Identity Framework** | Authentication |
| **JWT** | Token-based authentication |
| **AutoMapper** | Object mapping |
| **FluentValidation** | Input validation |
| **Serilog** | Logging |
| **Swagger** | API documentation |

### Frontend
| Technology | Purpose |
|------------|---------|
| **Razor Pages** | MVC views |
| **Bootstrap 5** | UI framework |
| **jQuery** | JavaScript library |
| **FontAwesome** | Icons |

---

## 🚀 Quick Start

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

### Step 1: Clone
```bash
git clone https://github.com/Abdulrhman774/Shipping-System.git
cd Shipping-System
```

### Step 2: Database Setup
Update connection string in `WebApi/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ShippingDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Run migrations:
```bash
cd WebApi
dotnet ef database update
```

Or use: `Database/ShippingDB.sql`

### Step 3: Run
#### Web API
```bash
cd WebApi
dotnet run
```

#### UI
```bash
cd UI
dotnet run
```

### Default Credentials
| Username | Password | Role |
|----------|----------|------|
| admin@gmail.com | Admin@12345 | Admin |

*Add more users via the system*

---

## 📊 API Endpoints

### 🔐 Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/Api/Auth/Register` | Register new user |
| POST | `/Api/Auth/Login` | Login user |
| POST | `/Api/Auth/RotateRefreshToken` | Rotate refresh token |
| POST | `/Api/Auth/Refresh-AccessToken` | Get new access token |
| POST | `/Api/Auth/Logout` | Logout user |

### 📦 Shipment
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/Api/Shipment` | Get all shipments |
| GET | `/Api/Shipment/{id}` | Get shipment by ID |
| POST | `/Api/Shipment/CreateShipment` | Create new shipment |
| PUT | `/Api/Shipment/{id}` | Update shipment |
| DELETE | `/Api/Shipment` | Delete shipment |

### 📋 Lookup
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/Api/City` | Get all cities |
| GET | `/Api/Country` | Get all countries |
| GET | `/Api/ShippingType` | Get shipping types |
| GET | `/Api/PaymentMethod` | Get payment methods |
| GET | `/Api/Carrier` | Get carriers |

> 📘 Full API docs available at `/swagger` when running WebApi

---

## 🧪 Testing

```bash
# Run API tests
cd WebApi
dotnet test

# Or use the provided HTTP files
# WebApi/WebApi.http - Test API endpoints
# WebApi/test.http - Additional tests
```

---

## 📈 Roadmap

### Phase 1: Core Features (✅ Complete)
- [x] Authentication & Authorization
- [x] User Management
- [x] Shipment CRUD
- [x] Rate Calculator
- [x] Subscription System
- [x] Payment Methods
- [x] Carrier Management

### Phase 2: Enhancements (🔄 In Progress)
- [ ] Real-time distance calculation (Google Maps API)
- [ ] Shipment tracking with status updates
- [ ] Email notifications
- [ ] PDF report generation

### Phase 3: Advanced Features (📋 Planned)
- [ ] Mobile app (React Native/Flutter)
- [ ] Payment gateway integration
- [ ] Real-time tracking (SignalR)
- [ ] Multi-language support
- [ ] Analytics dashboard

---

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📧 Contact

**Developer**: Abdulrhman  
**GitHub**: [@Abdulrhman774](https://github.com/Abdulrhman774)  
**Project**: Shipping Management System  
**Built With**: ❤️ + .NET 8

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

*Shipping Management System - Built with Clean Architecture and .NET 8* 🚀
