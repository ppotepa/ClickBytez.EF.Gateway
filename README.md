# ClickBytez.EF.Gateway

A lightweight Proof-of-Concept demonstrating a **Generic API Gateway** for performing CRUD operations on any EF Core entity via a single endpoint.

---

## Table of Contents

- [Overview](#overview)  
- [Features](#features)  
- [Architecture](#architecture)  
- [Getting Started](#getting-started)  
  - [Prerequisites](#prerequisites)  
  - [Installation](#installation)  
  - [Configuration](#configuration)  
- [Usage](#usage)  
  - [Sample Payloads & Responses](#sample-payloads--responses)  
- [Extending](#extending)  
- [Contributing](#contributing)  
- [License](#license)  

---

## Overview

**ClickBytez.EF.Gateway** exposes a single HTTP endpoint that interprets a `"type"` (for action + entity) and an `"entity"` JSON payload, 
then routes your request through EF Core to:

- Create new records  
- Read existing data  
- Update records  
- Soft-delete or hard-delete entries  

This approach can be useful for:

- Rapid prototyping of data-driven microservices  
- Building internal admin dashboards without writing separate controllers  
- Exposing a headless-CMS style API for multiple entity types  
- Integrating dynamic front-ends (e.g. no-code tools) with a single integration point  

## Features

- **Generic handler**: No per-entity controllers—just one API handler  
- **Action routing** via a simple `"type": "<action>.<entityName>"` convention  
- **Entity Framework Core** under the hood for data access  
- **Extensible**: plug in new entities by adding them to your EF model  
- **Soft-delete support** (optional)  
- **Audit fields**: automatically track `CreatedOn`, `CreatedBy`, `ModifiedOn`, `DeletedOn`, etc.  

## Architecture

```
┌──────────────────────────────┐
│    HTTP POST /api/gateway    │
│  { "type": "...", "entity": {} }  │
└──────────────────────────────┘
               │
               ▼
┌──────────────────────────────┐
│  GenericApiHandler (API)    │
│  • Parses "type"            │
│  • Resolves EF DbSet<>      │
│  • Invokes Create/Read/Update/Delete logic in Core  
└──────────────────────────────┘
               │
               ▼
┌──────────────────────────────┐
│   EF Core DbContext         │
│   • Tracks entities         │
│   • Saves changes           │
└──────────────────────────────┘
```

- **API Project** (`ClickBytez.EF.Gateway.API`):  
  - `Program.cs` / `Startup.cs` – configures services, DbContext, routing  
  - `Controllers/GenericController.cs` – single POST endpoint  
- **Core Project** (`ClickBytez.EF.Gateway.Core`):  
  - `Handlers/EntityHandler.cs` – generic CRUD logic  
  - `Models/` – your EF Core entity classes  
  - `Services/AuditService.cs` – optional audit metadata  

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) (or later)  
- A SQL Server / PostgreSQL / SQLite database  
- EF Core provider for your database  

### Installation

1. **Clone the repo**  
   ```bash
   git clone https://github.com/ppotepa/ClickBytez.EF.Gateway.git
   cd ClickBytez.EF.Gateway
   ```

2. **Restore packages & build**  
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Apply migrations** (if any)  
   ```bash
   cd ClickBytez.EF.Gateway.API
   dotnet ef database update
   ```

### Configuration

In `appsettings.json` (API project), set your connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MyDb;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## Usage

### Sending Requests

All requests go to the same endpoint:

```
POST /api/gateway
Content-Type: application/json
```

Body:

```json
{
  "type": "<action>.<entityName>",
  "entity": { /* your entity JSON */ }
}
```

#### Supported Actions

| Action   | Behavior                                   |
| -------- | ------------------------------------------ |
| `create` | Adds a new record, returns created entity  |
| `read`   | Fetches records (e.g. all or by key)       |
| `update` | Updates matching entity by ID or key       |
| `delete` | (Soft) deletes entity by key               |

#### Sample Payloads & Responses

**Create a User**

```json
POST /api/gateway
{
  "type": "create.user",
  "entity": {
    "name": "John",
    "surname": "Doe"
  }
}
```

_Response:_

```json
{
  "recordCount": 1,
  "entity": {
    "id": "e2f7ac5a-4561-4bec-117a-08d9d6afca83",
    "name": "John",
    "surname": "Doe",
    "dateOfBirth": "0001-01-01T00:00:00",
    "friends": null,
    "createdBy": "5c45daad-4b5c-482b-9d37-e848bdd0a4ff",
    "createdOn": "2022-01-13T17:26:53.8805596+01:00",
    "deletedBy": "00000000-0000-0000-0000-000000000000",
    "deletedOn": null,
    "modifiedBy": "00000000-0000-0000-0000-000000000000",
    "modifiedOn": null
  }
}
```

**Read All Users**

```json
{
  "type": "read.user",
  "entity": {}
}
```

_Response:_

```json
{
  "recordCount": 5,
  "entities": [ /* list of users */ ]
}
```

## Extending

1. **Add a new entity** class in `Models` and include it in your `DbContext`.  
2. Update any mappings or configuration (e.g. Fluent API).  
3. You can optionally override or hook into the generic handler (e.g. add custom business rules).  

## Contributing

1. Fork the repo  
2. Create a feature branch (`git checkout -b feature/my-gateway-extension`)  
3. Commit your changes (`git commit -m "Add support for order.read"`)  
4. Push to your branch (`git push origin feature/my-gateway-extension`)  
5. Open a Pull Request  

Please follow the existing coding style and add tests for new functionality.

## License

This project is for demonstration purposes only.  
_You may choose to apply an open-source license here (e.g. MIT)._


# ClickBytez.EF.Gateway  

A lightweight Proof-of-Concept demonstrating a **Generic API Gateway** for performing CRUD operations on any EF Core entity via a single endpoint.  

This project is designed as a **Minimal API** with a **Generic API Handler**, aiming to simplify CRUD operations for EF Core entities. While it is intentionally minimalistic, it is also somewhat of a reinvention of the wheel, as it resembles the functionality of GraphQL or similar single-endpoint APIs.  

---  

## Table of Contents  

- [Overview](#overview)  
- [Features](#features)  
- [Architecture](#architecture)  
- [Getting Started](#getting-started)  
 - [Prerequisites](#prerequisites)  
 - [Installation](#installation)  
 - [Configuration](#configuration)  
- [Usage](#usage)  
 - [Sample Payloads & Responses](#sample-payloads--responses)  
- [Extending](#extending)  
- [Contributing](#contributing)  
- [License](#license)  

---  

## Overview  

**ClickBytez.EF.Gateway** exposes a single HTTP endpoint that interprets a `"type"` (for action + entity) and an `"entity"` JSON payload, then routes your request through EF Core to:  

- Create new records  
- Read existing data  
- Update records  
- Soft-delete or hard-delete entries  

This approach can be useful for:  

- Rapid prototyping of data-driven microservices  
- Building internal admin dashboards without writing separate controllers  
- Exposing a headless-CMS style API for multiple entity types  
- Integrating dynamic front-ends (e.g. no-code tools) with a single integration point  

While this project is minimal and generic, it draws inspiration from tools like GraphQL by providing a single endpoint for multiple operations.  

## Features  

- **Minimal API**: Built with .NET 8's minimal API approach for simplicity and performance  
- **Generic handler**: No per-entity controllers—just one API handler  
- **Action routing** via a simple `"type": "<action>.<entityName>"` convention  
- **Entity Framework Core** under the hood for data access  
- **Extensible**: plug in new entities by adding them to your EF model  
- **Soft-delete support** (optional)  
- **Audit fields**: automatically track `CreatedOn`, `CreatedBy`, `ModifiedOn`, `DeletedOn`, etc.
