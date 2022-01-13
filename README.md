# ClickBytez.EF.Gateway

Very small PoC on how to implement Generic API Handler thats is responsible for creating, updating, deleting and reading entities using Entity Framework.

example payload
```json
{
  "type": "create.user",
  "entity": {
    "name": "John",
    "surname": "Doe"
  }
}
```
example response
```json
{
  "recordCount": 1,
  "entity": {
    "name": "John",
    "surname": "Doe",
    "dateOfBirth": "0001-01-01T00:00:00",
    "friends": null,
    "createdBy": "5c45daad-4b5c-482b-9d37-e848bdd0a4ff",
    "createdOn": "2022-01-13T17:26:53.8805596+01:00",
    "deletedBy": "00000000-0000-0000-0000-000000000000",
    "deletedOn": null,
    "id": "e2f7ac5a-4561-4bec-117a-08d9d6afca83",
    "modifiedOn": null,
    "modifiedBy": "00000000-0000-0000-0000-000000000000"
  }
}
```

