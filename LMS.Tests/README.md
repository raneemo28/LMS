# LMS Integration Tests — Setup & Run Guide

## What is this project?

`LMS.Tests` is a professional integration test suite for your LMS API.
It uses **xUnit** (the same framework used at Microsoft and Amazon) and
**WebApplicationFactory**, which boots your real API in memory — no SQL Server,
no IIS, no Postman needed. Every test runs against your actual controllers,
handlers, validators, and AutoMapper profiles.

---

## Project Structure

```
LMS.Tests/
├── Infrastructure/
│   ├── LmsApiFactory.cs     ← Boots API in memory, swaps SQL Server for InMemory DB
│   └── TestFixture.cs       ← Base class: creates authenticated + admin HTTP clients
│
├── Tests/
│   ├── AuthControllerTests.cs           ← Register, Login, Logout
│   ├── VocabulariesControllerTests.cs   ← Vocabularies & Properties CRUD
│   ├── ResourceTemplateControllerTests.cs ← Templates + property links
│   ├── ItemsControllerTests.cs          ← Items CRUD
│   ├── ItemSetsControllerTests.cs       ← Sets CRUD + member management
│   ├── MediaControllerTests.cs          ← Media records + file upload
│   ├── ResourceControllerTests.cs       ← Values CRUD on any resource
│   └── UsersControllerTests.cs          ← Admin user management
│
├── LMS.Tests.csproj
├── run-tests.sh    ← Linux / Mac run script
└── run-tests.ps1   ← Windows PowerShell run script
```

---

## Step 1 — Add the test project to your solution

Open a terminal in your **solution root** (`C:\...\LMS`) and run:

```bash
dotnet sln add LMS.Tests/LMS.Tests.csproj
```

---

## Step 2 — Restore packages

```bash
dotnet restore LMS.Tests/LMS.Tests.csproj
```

---

## Step 3 — Make Program.cs test-friendly

Your `Program.cs` ends with `app.Run()`. The test factory needs to reference the
`Program` class. Add this one line at the very **bottom** of `Program.cs`:

```csharp
// Required for WebApplicationFactory in integration tests
public partial class Program { }
```

---

## Step 4 — Make your DbContexts public (if they aren't already)

In `LMS.Infra/Database/LibraryDbContext.cs` and `AppIdentityDbContext.cs`, confirm
the class access modifier is `public`:

```csharp
public class LibraryDbContext : DbContext { ... }
public class AppIdentityDbContext : IdentityDbContext<ApplicationUser> { ... }
```

---

## Step 5 — Run the tests

### Option A — Simple (just see pass/fail)
```bash
dotnet test LMS.Tests/LMS.Tests.csproj --logger "console;verbosity=detailed"
```

### Option B — With a `.trx` report file (opens in Visual Studio Test Explorer)
```bash
dotnet test LMS.Tests/LMS.Tests.csproj \
  --logger "trx;LogFileName=TestResults.trx" \
  --results-directory TestResults
```
Then open `TestResults/TestResults.trx` in Visual Studio.

### Option C — Full script with HTML coverage report
```bash
# Linux / Mac
bash LMS.Tests/run-tests.sh

# Windows PowerShell
.\LMS.Tests\run-tests.ps1
```

---

## Step 6 — (Optional) Install ReportGenerator for HTML coverage

```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
```

Then rerun the script — it will produce a beautiful HTML report at
`TestResults/Coverage/html/index.html`.

---

## What each test does

| Test Class | # Tests | What is verified |
|---|---|---|
| `AuthControllerTests` | 8 | Register (valid/dup/weak pw), Login (ok/wrong pw/missing), Logout (auth/anon) |
| `VocabulariesControllerTests` | 9 | Public reads, admin CRUD, member/anon blocked |
| `ResourceTemplateControllerTests` | 9 | Create, get, update, delete templates; add/update/remove property links |
| `ItemsControllerTests` | 9 | Create, read, filter, update, delete; auth enforcement |
| `ItemSetsControllerTests` | 12 | Full set lifecycle; add/remove members; ownership check |
| `MediaControllerTests` | 9 | Create, get, edit, delete; file upload (valid + empty); by-mime; by-owner |
| `ResourceControllerTests` | 5 | Add values, get values, update value, remove value; anon blocked |
| `UsersControllerTests` | 9 | Admin-only enforcement; promote/demote; deactivate/reactivate |
| **Total** | **~70** | **Full API coverage** |

---

## How it works (the magic explained)

```
Your Test Code
     │
     │  HTTP POST /api/auth/register
     ▼
  HttpClient (in-memory)
     │
     ▼
 WebApplicationFactory
  ├─ Starts your real ASP.NET Core pipeline
  ├─ Loads your real DI container
  ├─ Loads your real MediatR handlers
  ├─ Loads your real FluentValidation validators
  ├─ Loads your real AutoMapper profiles
  ├─ Loads your real JWT middleware
  └─ BUT replaces SQL Server with in-memory DB (no real DB needed!)
     │
     ▼
  Your real AuthController.Register()
     │
     ▼
  Returns 200 OK + JWT token ← Test asserts this
```

No SQL Server is needed. No running server is needed. Just `dotnet test`.
