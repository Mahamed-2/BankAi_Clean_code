# Bankai.se — Clean Architecture ASP.NET Core Web API

> 万解 (Bankai) — *Final Release* · VG Level Assignment · 2026

A production-grade **ASP.NET Core Web API** demonstrating Clean Architecture principles with full CRUD, CQRS, MediatR, JWT authentication, and Role-Based Access Control.

---

## Architecture Layers

```
Bankai.Domain          ← Entities, Interfaces, Enums (no dependencies)
Bankai.Application     ← CQRS, MediatR, Validators, Behaviors, DTOs
Bankai.Infrastructure  ← EF Core, Repositories, DbContext
Bankai.API             ← Controllers, JWT, Swagger, DI setup
website/               ← bankai.se showcase frontend
```

## G-Level Requirements ✅

| Requirement | Implementation |
|---|---|
| 4-layer Clean Architecture | Domain → Application → Infrastructure → API |
| 2 Entities + Relationship | Product ↔ Category (many-to-one) |
| Full CRUD | ProductsController + CategoriesController |
| CQRS + MediatR | Commands, Queries, Handlers |
| IRepository + EF Core | `IRepository<T>`, `IProductRepository`, `AppDbContext` |
| FluentValidation | `CreateProductValidator` |
| GitHub Branches | 4 feature branches (see below) |

## VG-Level Requirements ✅

| Requirement | Implementation |
|---|---|
| AutoMapper + DTOs | `MappingProfile`, `ProductDto`, `CategoryDto` |
| MediatR Pipeline Behaviors | `ValidationBehavior`, `LoggingBehavior` |
| JWT Authentication | `AuthController` with BCrypt + token generation |
| RBAC | `AdminOnly` and `UserOrAdmin` policies, `UserRole` enum |

## Git Branches

| Branch | Named After | Contents |
|---|---|---|
| `feature/Domain` | `Bankai.Domain` project | Entities, Interfaces, Enums |
| `feature/Application` | `Bankai.Application` project | Commands, Queries, Handlers, Validators, Behaviors, DTOs |
| `feature/Infrastructure` | `Bankai.Infrastructure` project | DbContext, Repositories |
| `feature/API` | `Bankai.API` project | Controllers, Program.cs, JWT, RBAC |

## Running Locally

```bash
cd Bankai.API
dotnet run
# Swagger UI: https://localhost:5001/swagger
```

## API Endpoints

| Method | Endpoint | Auth |
|---|---|---|
| GET | `/api/products` | Public |
| GET | `/api/products/{id}` | Public |
| POST | `/api/products` | Admin JWT |
| PUT | `/api/products/{id}` | Admin JWT |
| DELETE | `/api/products/{id}` | Admin JWT |
| GET | `/api/categories` | Public |
| POST | `/api/auth/register` | Public |
| POST | `/api/auth/login` | Public |
