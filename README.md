# 📚 LMS — Library Metadata System

A clean-architecture **ASP.NET Core Web API** for managing digital library metadata — resources, items, vocabularies, templates, and media — with full JWT authentication, role-based access control, localization, and a microservice logging integration.

---

## 🏗️ Architecture Overview

The solution follows **Clean Architecture** (Domain → Application → Infrastructure → API), split into four projects:

```
LMS/
├── LMS.Domain/        # Core entities & repository interfaces
├── LMS.App/           # CQRS handlers (MediatR), DTOs, validators, AutoMapper profiles
├── LMS.infra/         # EF Core DbContexts, repositories, JWT, media storage
├── LMS.API/           # ASP.NET Core controllers, middleware, startup
└── LMS.Tests/         # Integration & unit tests
```

### Layer Responsibilities

| Layer | Responsibility |
|---|---|
| **Domain** | Entities (`Resource`, `Item`, `ItemSet`, `Media`, `Vocabulary`, `Property`, `Value`, `ResourceTemplate`, `TemplateProperty`, `ApplicationUser`) and repository contracts |
| **Application** | MediatR commands/queries, FluentValidation validators, AutoMapper profiles, pipeline behaviors |
| **Infrastructure** | EF Core (two SQL Server databases), Identity, JWT service, local media storage, Unit of Work, repository implementations |
| **API** | REST controllers, global exception handler, localization middleware, Swagger/OpenAPI |

---

## ✨ Features

- 🔐 **JWT Authentication** — Bearer token auth with ASP.NET Core Identity (RBAC)
- 📦 **CQRS with MediatR** — Commands and queries cleanly separated per feature
- ✅ **FluentValidation** — Validation pipeline behavior with localized error messages
- 🗺️ **AutoMapper** — Entity ↔ DTO mapping with dedicated profiles per feature
- 🌐 **Localization** — English (`en-US`) & Arabic (`ar-SA`) support; switchable via `?lang=` query string
- 🗄️ **Dual Database** — Separate SQL Server databases for library metadata and identity/security
- 🖼️ **Media Storage** — Local file storage for media uploads (`wwwroot/uploads`)
- 📋 **Swagger UI** — Auto-generated interactive API docs with JWT support
- 🔄 **Unit of Work** — Atomic database transactions across multiple repositories
- 🧪 **Testing** — Integration tests using a custom `WebApplicationFactory` + unit tests
- 📡 **Microservice Logging** — HTTP client forwarding logs to an external logging service

---

## 📁 Project Structure

### `LMS.Domain`
```
Entities/
├── ApplicationUser.cs
├── Item.cs
├── ItemSet.cs
├── ItemSetWithMembers.cs
├── Media.cs
├── Property.cs
├── Resource.cs
├── ResourceTemplate.cs
├── TemplateProperty.cs
├── Value.cs
└── Vocabulary.cs
interfaces/          # IGenericRepository, IItemRepository, IItemSetRepository,
                     # IMediaRepository, IResourceRepository, IResourceTemplateRepository,
                     # IUnitOfWork, IVocabularyRepository
Constants/
```

### `LMS.App`
```
features/
├── Items/
├── ItemSets/
├── Login/
├── Logout/
├── Medias/
├── Register/
├── Resources/
├── ResourceTemplates/
├── Vocabularies/
└── users/
DTOs/
Validators/
Profiles/
Behaviors/           # ValidationBehavior (MediatR pipeline)
Interface/
microservice/        # Logging microservice client
shared_resources/    # Localization (.resx) files (ErrorMessages)
```

### `LMS.infra`
```
Database/            # LibraryDbContext, AppIdentityDbContext
Migrations/
Repository/          # GenericRepository, ItemRepository, ItemSetRepository,
                     # MediaRepository, ResourceRepository, ResourceTemplateRepository,
                     # VocabularyRepository, UnitOfWork
Services/            # JwtService
ServiceStorage/      # LocalMediaStorageService, MediaProcessingService
DatabaseInitializer.cs
DependencyInjection.cs
```

### `LMS.API`
```
Controllers/
├── AuthController.cs
├── ItemSetsController.cs
├── ItemsController.cs
├── MediaController.cs
├── ResourceController.cs
├── ResourceTemplateController.cs
├── UserController.cs
└── VocabulariesController.cs
middlewares/         # LoggingMiddleware
Program.cs
appsettings.json
```

---

## 🔌 API Endpoints

| Controller | Base Route | Description |
|---|---|---|
| `AuthController` | `/api/auth` | Register, Login, Logout |
| `UserController` | `/api/users` | User management |
| `ResourceController` | `/api/resources` | Library resources (CRUD) |
| `ResourceTemplateController` | `/api/resource-templates` | Metadata templates (CRUD) |
| `ItemsController` | `/api/items` | Items within resources (CRUD) |
| `ItemSetsController` | `/api/item-sets` | Item sets / collections (CRUD) |
| `VocabulariesController` | `/api/vocabularies` | Controlled vocabularies (CRUD) |
| `MediaController` | `/api/media` | Media file upload and management |

> Swagger UI is available at `/swagger` in Development mode.

---

## ⚙️ Configuration

Edit `LMS.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "LibraryConnection": "Server=.;Database=DLMS_Library_Metadata;Trusted_Connection=True;TrustServerCertificate=True",
    "IdentityConnection": "Server=.;Database=DLMS_Identity_Security;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "LoggingService": {
    "BaseUrl": "http://localhost:5050"
  },
  "Jwt": {
    "Key": "<your-secret-key-min-32-chars>",
    "Issuer": "LMS.API",
    "Audience": "LMS.Users"
  },
  "Storage": {
    "BasePath": "wwwroot/uploads"
  }
}
```

### Password Policy (ASP.NET Core Identity)
- Minimum **8 characters**
- At least one **digit**
- At least one **non-alphanumeric** character
- **Unique email** required per user

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- _(Optional)_ External logging microservice running on port `5050`

### 1. Clone the repository

```bash
git clone <repository-url>
cd LMS
```

### 2. Configure the database

Update the connection strings in `LMS.API/appsettings.json` to point to your SQL Server instance.

### 3. Apply migrations

The `DatabaseInitializer` runs automatically on startup and seeds the database. If you want to apply migrations manually:

```bash
dotnet ef database update --project LMS.infra --startup-project LMS.API --context LibraryDbContext
dotnet ef database update --project LMS.infra --startup-project LMS.API --context AppIdentityDbContext
```

### 4. Run the API

```bash
dotnet run --project LMS.API
```

The API will be available at `http://localhost:<port>`.  
Swagger UI: `http://localhost:<port>/swagger`

---

## 🧪 Running Tests

```bash
dotnet test LMS.Tests
```

- **Integration tests** use `LmsWebApplicationFactory` with the `Testing` environment, which skips the production `DatabaseInitializer` and uses its own seeding helpers.
- **Unit tests** are located in `LMS.Tests/UnitTests/`.

---

## 🌐 Localization

The API supports two languages out of the box:

| Language | Culture Code |
|---|---|
| English | `en-US` (default) |
| Arabic | `ar-SA` |

Switch language via:
- **Query string**: `?lang=ar-SA`
- **`Accept-Language` HTTP header**: `Accept-Language: ar-SA`

---

## 🔐 Authentication

All protected endpoints require a JWT Bearer token in the `Authorization` header:

```
Authorization: Bearer <your-jwt-token>
```

Obtain a token via `POST /api/auth/login`.

---

## 🛡️ Error Handling

Global exception handling middleware returns structured JSON for all errors:

| Exception | HTTP Status |
|---|---|
| `ValidationException` (FluentValidation) | `400 Bad Request` |
| `UnauthorizedAccessException` | `403 Forbidden` |
| `KeyNotFoundException` | `404 Not Found` |
| `FileNotFoundException` | `404 Not Found` |
| `InvalidOperationException` | `400 Bad Request` |
| Unhandled exceptions | `500 Internal Server Error` |

---

## 🧰 Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core 8 | Web API framework |
| Entity Framework Core | ORM + Migrations |
| SQL Server | Primary database |
| ASP.NET Core Identity | User management & RBAC |
| MediatR | CQRS mediator |
| FluentValidation | Input validation |
| AutoMapper | Object mapping |
| JWT Bearer | Stateless authentication |
| Swagger / Swashbuckle | API documentation |
| xUnit / WebApplicationFactory | Testing |
