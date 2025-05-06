# ClickBytez.EF.Gateway

**ClickBytez.EF.Gateway** is an experimental, minimal API built with ASP.NET Core and EF Core. It provides a *single endpoint* that handles CRUD operations for any entity, inspired by GraphQL-style data access.  
This project is a proof-of-concept and not production-ready — think of it as a learning exercise in building a generic API handler.

## How it works

- **Single endpoint:** All requests go to `POST /api/gateway` with a JSON body.  
- **Type-based routing:** The request JSON includes a `"type": "action.entity"` (e.g. `"create.user"` or `"read.product"`).  
  The handler parses this to determine the operation (create, read, update, delete) and the target entity.  
- **Entity payload:** The JSON body has an `"entity"` object that carries the data or filters.  
  For `create` or `update`, it has fields to save; for `read` or `delete`, it can specify criteria (e.g. an ID).  
- **EF Core under the hood:** The handler uses Entity Framework Core to perform the requested operation on the database.  

Because it’s a simplified, generic handler, it does “reinvent the wheel” of routing logic. It can be useful for rapid prototyping or simple admin tools where a single entry point for data operations is desirable.

## Example Usage

**Create a new user:** Send a POST with `"type": "create.user"` and an entity payload. The response returns the created record.

```json
POST /api/gateway
Content-Type: application/json

{
  "type": "create.user",
  "entity": { "name": "Alice", "email": "alice@example.com" }
}
```

**Response:**

```json
{
  "recordCount": 1,
  "entity": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "Alice",
    "email": "alice@example.com"
  }
}
```

**Read all users:** Use `"type": "read.user"` with an empty `entity` filter (or specific criteria). The response returns matching users.

```json
POST /api/gateway
Content-Type: application/json

{
  "type": "read.user",
  "entity": {}
}
```

**Response:**

```json
{
  "recordCount": 3,
  "entities": [
    { "id": "550e8400-e29b-41d4-a716-446655440000", "name": "Alice", "email": "alice@example.com" },
    { "id": "660e8400-e29b-41d4-a716-446655440001", "name": "Bob",   "email": "bob@example.com" },
    { "id": "770e8400-e29b-41d4-a716-446655440002", "name": "Carol", "email": "carol@example.com" }
  ]
}
```


## Basic Filtering Added (Work in Progress)

You can now issue a query request that only uses dynamic filters, without example entity matching. For example:

```json
{
  "type": "read.user",
  "entity": {},
  "filters": [
    "name.contains(a)"
  ]
}

> **Note:** This project is a work-in-progress. It’s intentionally minimal and lacks features like input validation, authentication, or complex querying. It’s more of a GraphQL-inspired experiment than a finished product.
