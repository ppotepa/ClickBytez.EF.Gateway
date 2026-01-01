# PortalZ  

**PortalZ** is an experimental, minimal API built with ASP.NET Core and EF Core. It provides a *single endpoint* that handles CRUD operations for any entity, inspired by GraphQL-style data access.  
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
**Response:**
**Read all users:** Use `"type": "read.user"` with an empty `entity` filter (or specific criteria). The response returns matching users.
**Response:**
## Basic Filtering Added (Work in Progress)  

You can now issue a query request that uses dynamic filters to refine results. Filters are applied to the queryable data source using the logic defined in `QueryableFilter.cs`. This allows for flexible, runtime-defined filtering based on property values and operators.  

### Supported Filter Syntax  

Filters are specified as strings in the format:
- **`<property>`**: The name of the property to filter on (case-insensitive).  
- **`<operator>`**: The comparison operator to use (e.g., `contains`, `startswith`, `eq`).  
- **`<value>`**: The value to compare against.  

### Supported Operators  

The following operators are supported:  

| Operator     | Description                          | Example Filter          |  
|--------------|--------------------------------------|-------------------------|  
| `contains`   | Checks if a string contains a value | `name.contains(Alice)`  |  
| `startswith` | Checks if a string starts with a value | `name.startswith(A)` |  
| `endswith`   | Checks if a string ends with a value | `name.endswith(e)`      |  
| `eq`         | Checks for equality                 | `age.eq(30)`            |  
| `gt`         | Greater than                        | `age.gt(25)`            |  
| `lt`         | Less than                           | `age.lt(40)`            |  
| `gte`        | Greater than or equal to            | `age.gte(30)`           |  
| `lte`        | Less than or equal to               | `age.lte(35)`           |  

### Example Usage  

To filter users whose names contain the letter "a":
To filter users older than 25:
### How It Works  

The filtering logic is implemented in the `QueryableExtensions.ApplyRequest` method. It dynamically builds an expression tree based on the provided filters and applies it to the queryable data source using LINQ's `Where` method. Here's a high-level overview of the process:  

1. **Parse Filters**: Each filter string is parsed using a regular expression to extract the property, operator, and value.  
2. **Validate Property**: The property name is validated against the target entity's properties.  
3. **Build Expression**: An expression is created for the specified operator and value.  
4. **Combine Expressions**: Multiple filters are combined using `AND` logic.  
5. **Apply to Queryable**: The final expression is applied to the queryable source using the `Where` method.  

This approach allows for flexible, runtime-defined filtering while leveraging the power of LINQ and expression trees.  

> **Note:** Filters are case-insensitive for property names but case-sensitive for string values. Ensure the filter syntax matches the expected format.
