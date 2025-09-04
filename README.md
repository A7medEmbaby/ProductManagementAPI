# Product Management API

A comprehensive Product Management system built with .NET 8, implementing Clean Architecture, CQRS pattern, and Domain-Driven Design principles.

## 🏗️ Architecture Overview

This application follows **Clean Architecture** principles with clear separation of concerns across four main layers:

### Domain Layer
- **Entities**: `Product` and `Category` aggregates with rich domain logic
- **Value Objects**: Strongly-typed identifiers (`ProductId`, `CategoryId`, `ProductName`, `CategoryName`, `Money`)
- **Domain Events**: Inter-aggregate communication without direct relationships
- **Repository Interfaces**: Contracts for data access

### Application Layer
- **CQRS Implementation**: Commands for write operations, Queries for read operations
- **MediatR Integration**: Centralized request/response handling
- **Command/Query Handlers**: Business logic execution
- **DTOs**: Data transfer objects for API communication
- **Validators**: FluentValidation for input validation
- **Event Handlers**: Domain event processing

### Infrastructure Layer
- **Entity Framework Core**: Data persistence with SQLite
- **Repository Pattern**: Concrete implementations of domain repositories
- **Domain Event Dispatcher**: Automatic domain event publishing
- **Database Configurations**: EF Core entity configurations

### API Layer
- **Controllers**: RESTful API endpoints
- **Middleware**: Global exception handling and validation
- **Swagger Integration**: API documentation
- **CORS Configuration**: Cross-origin resource sharing

## 🚀 Key Features

- ✅ **Clean Architecture** with proper layer separation
- ✅ **CQRS Pattern** using MediatR
- ✅ **Domain Events** for aggregate communication
- ✅ **No Direct Foreign Keys** between aggregates (as required)
- ✅ **RESTful API Design** with proper HTTP conventions
- ✅ **Comprehensive Validation** using FluentValidation
- ✅ **Global Exception Handling**
- ✅ **Swagger Documentation**
- ✅ **Entity Framework Core** with SQLite
- ✅ **Rich Domain Models** with business logic encapsulation

## 🛠️ Technology Stack

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQLite Database**
- **MediatR** (CQRS implementation)
- **FluentValidation**
- **Swagger/OpenAPI**

## 📋 Prerequisites

- **.NET 8 SDK** or later
- **Visual Studio 2022** / **VS Code** / **JetBrains Rider**
- **Git**

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/A7medEmbaby/ProductManagementAPI.git
cd ProductManagement
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Database Setup
The application uses SQLite with an auto-created database. No manual migration is needed as the database will be created automatically in the `Database` folder.

**Database Location**: `../Database/ProductManagement.db`

### 4. Run the Application
```bash
cd ProductManagement
dotnet run
```

### 5. Access the API
- **Swagger UI**: `https://localhost:7xxx/swagger` (port may vary)
- **API Base URL**: `https://localhost:7xxx/api`

## 📚 API Endpoints

### Categories
- `GET /api/Categories/GetAllCategories` - Get all categories
- `GET /api/Categories/GetCategoryById/{id}` - Get category by ID
- `POST /api/Categories/CreateCategory` - Create new category
- `PUT /api/Categories/UpdateCategoryById/{id}` - Update category
- `DELETE /api/Categories/DeleteCategoryById/{id}` - Delete category

### Products
- `GET /api/Products/GetAllProducts` - Get all products (paginated)
- `GET /api/Products/GetProductBy/{id}` - Get product by ID
- `GET /api/Products/GetProductsByCategoryId/{categoryId}` - Get products by category
- `POST /api/Products/CreateProduct` - Create new product
- `PUT /api/Products/UpdateProductById/{id}` - Update product
- `DELETE /api/Products/DeleteProductById/{id}` - Delete product

## 🏛️ Project Structure

```
ProductManagement/
├── API/                          # Presentation Layer
│   ├── Controllers/              # API Controllers
│   ├── Middleware/              # Custom middleware
│   └── Program.cs               # Application entry point
├── Application/                 # Application Layer
│   ├── Categories/              # Category features
│   │   ├── Commands/           # Write operations
│   │   ├── Queries/            # Read operations
│   │   ├── Handlers/           # Business logic handlers
│   │   ├── DTOs/               # Data transfer objects
│   │   ├── Validators/         # Input validation
│   │   └── EventHandlers/      # Domain event handlers
│   ├── Products/               # Product features
│   │   └── [Same structure as Categories]
│   └── Common/                 # Shared application concerns
├── Domain/                     # Domain Layer
│   ├── Categories/             # Category aggregate
│   ├── Products/               # Product aggregate
│   ├── ValueObjects/           # Domain value objects
│   └── Common/                 # Domain base classes
├── Infrastructure/             # Infrastructure Layer
│   ├── Data/                   # Data persistence
│   │   ├── Configurations/     # EF Core configurations
│   │   └── Repositories/       # Repository implementations
│   └── Events/                 # Domain event infrastructure
└── Database/                   # SQLite database files
```

## 🎯 Design Patterns & Principles

### Clean Architecture
- **Dependency Inversion**: Higher-level layers depend on abstractions
- **Layer Separation**: Clear boundaries between concerns
- **Independent Testability**: Each layer can be tested in isolation

### CQRS (Command Query Responsibility Segregation)
- **Commands**: Handle write operations and business logic
- **Queries**: Handle read operations and data retrieval
- **Separation**: Clear distinction between reads and writes

### Domain-Driven Design (DDD)
- **Aggregates**: Product and Category as consistency boundaries
- **Value Objects**: Immutable objects representing domain concepts
- **Domain Events**: Communication between aggregates
- **Rich Domain Models**: Business logic encapsulated in entities

### Repository Pattern
- **Abstraction**: Domain defines interfaces, Infrastructure implements
- **Testability**: Easy to mock for unit testing
- **Flexibility**: Can switch data access technologies

## 🔧 Configuration

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=../Database/ProductManagement.db"
  }
}
```

### CORS Policy
The application includes a permissive CORS policy for development:
```csharp
policy.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader();
```



## 🔍 Validation & Error Handling

### Input Validation
- **FluentValidation**: Comprehensive input validation
- **Model State**: ASP.NET Core model validation
- **Custom Validators**: Business rule validation

### Error Handling
- **Global Exception Middleware**: Centralized error handling
- **HTTP Status Codes**: Proper REST conventions
- **Structured Responses**: Consistent error response format

## 📊 Domain Events

The application implements domain events for aggregate communication:

### Product Events
- `ProductCreatedEvent`
- `ProductUpdatedEvent`
- `ProductDeletedEvent`
- `ProductCategoryChangedEvent`

### Category Events
- `CategoryCreatedEvent`
- `CategoryUpdatedEvent`
- `CategoryDeletedEvent`

## 🚦 Business Rules

### Category Management
- Category names must be unique
- Categories with products cannot be deleted
- Category names are limited to 100 characters

### Product Management
- Products must belong to an existing category
- Product names are limited to 200 characters
- Prices must be positive values
- Currency codes must be 3 uppercase letters

## 🔒 Security Considerations

- **Input Validation**: All inputs are validated
- **SQL Injection Protection**: EF Core provides parameterized queries
- **Exception Information**: Sensitive information is not exposed in errors


## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request



## 📞 Support

For questions or support, please open an issue in the repository.
