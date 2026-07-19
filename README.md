# 🚚 Shipping Management System

A comprehensive **Shipping Management System** built with **.NET 8** and **Clean Architecture** principles. Designed to manage shipments, users, carriers, payments, subscriptions, and more with a robust **Result Pattern** for error handling.

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
- ✅ **Currency Exchange** - Multi-currency support (Optional module)

### 🎯 Business Features
- 📦 **Shipment Tracking** - Track shipments via unique tracking numbers
- 💰 **Cost Calculation** - Volumetric weight calculation with distance-based pricing
- 📊 **Usage Analytics** - Track subscription usage (weight, distance, shipment count)
- 🔄 **Transaction Logging** - Full audit trail for all transactions
- 📱 **Multi-platform** - RESTful API with MVC frontend

---

## 🏗️ Architecture

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
├── Database/                        # Database scripts
│   └── ShippingDB.sql               # SQL Server database script
│
├── Docs/                            # Documentation
│   ├── CSS/                         # Template styles
│   ├── JS/                          # JavaScript files
│   └── Images/                      # Project images
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

### Patterns & Practices
- ✅ **Clean Architecture** - Separation of concerns
- ✅ **Repository Pattern** - Data access abstraction
- ✅ **Unit of Work** - Transaction management
- ✅ **Result Pattern** - Explicit error handling
- ✅ **Dependency Injection** - Loose coupling
- ✅ **CQRS-like** - Command/Query separation

---

## 🚀 How to Run

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

### Step 1: Clone the Repository
```bash
git clone https://github.com/Abdulrhman774/Shipping-System.git
cd Shipping-System
```

### Step 2: Database Setup
1. Update the connection string in `WebApi/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ShippingDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

2. Run migrations:
```bash
cd WebApi
dotnet ef database update
```

Or use the provided database script:
- Run `Database/ShippingDB.sql` in SQL Server Management Studio

### Step 3: Run the Application
**Run both services:**

#### Web API (Backend)
```bash
cd WebApi
dotnet run
```

#### UI (Frontend)
```bash
cd UI
dotnet run
```

### Step 4: Default Credentials
After seeding:
- **Username**: `admin@gmail.com`
- **Password**: `Admin@12345`
- **Role**: `Admin`

---

## 🔑 Key Features Explained

### 🧮 Rate Calculator
The system calculates shipping rates based on:
1. **Actual Weight** - Physical weight in kg
2. **Volumetric Weight** - (Length × Width × Height) / 5000
3. **Billable Weight** - Max(Actual Weight, Volumetric Weight)
4. **Distance** - Between sender and receiver cities
5. **Shipping Type** - Multiplier factor

### 📦 Subscription Packages
Users can subscribe to packages with:
- **Shipment Count** - Number of shipments allowed
- **Total Weight** - Maximum weight (kg)
- **Total Distance** - Maximum distance (km)
- **Tracking** - Used shipment count, weight, and distance

### 🔐 Authentication Flow
1. **Register** → Create user account
2. **Login** → Receive AccessToken + RefreshToken
3. **AccessToken** → Used for API calls (short-lived)
4. **RefreshToken** → Used to get new AccessToken (long-lived)
5. **Rotate** → RefreshToken rotation for security

---

## 📊 API Endpoints

### Auth Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/Api/Auth/Register` | Register new user |
| POST | `/Api/Auth/Login` | Login user |
| POST | `/Api/Auth/RotateRefreshToken` | Rotate refresh token |
| POST | `/Api/Auth/Refresh-AccessToken` | Get new access token |
| POST | `/Api/Auth/Logout` | Logout user |

### Shipment Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/Api/Shipment` | Get all shipments |
| GET | `/Api/Shipment/{id}` | Get shipment by ID |
| POST | `/Api/Shipment/CreateShipment` | Create new shipment |
| PUT | `/Api/Shipment/{id}` | Update shipment |
| DELETE | `/Api/Shipment` | Delete shipment |

### Lookup Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/Api/City` | Get all cities |
| GET | `/Api/Country` | Get all countries |
| GET | `/Api/ShippingType` | Get shipping types |
| GET | `/Api/PaymentMethod` | Get payment methods |
| GET | `/Api/Carrier` | Get carriers |

*Full API documentation available at `/swagger` when running the WebApi*

---

## 🧪 Testing

### API Testing
Use the provided HTTP files:
- `WebApi/WebApi.http` - Test API endpoints
- `WebApi/test.http` - Additional tests

### Postman Collection
Import the Postman collection from `Docs/` (if available)

---

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 🐛 Known Issues & TODOs

- [ ] Implement actual distance calculation (Google Maps API integration)
- [ ] Add real-time shipment tracking
- [ ] Implement email notifications
- [ ] Add PDF report generation
- [ ] Complete missing methods in `GetShipmentByTrackingNumberAsync` and `GetShipmentsForUserAsync`
- [ ] Add comprehensive unit tests

---

## 📧 Contact

**Developer**: Abdulrhman  
**Project**: Shipping Management System  
**Built With**: ❤️ + .NET 8

---

## 📄 License

This project is for educational purposes only.

---

*Shipping Management System - Built with Clean Architecture and .NET 8* 🚀
