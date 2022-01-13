# ClickBytez.EF.Gateway

Very small PoC on how to implement Generic API Handler thats is responsible for creating, updating, deleting and reading entities using Entity Framework.

example payload

{
  "type": "create.user",
  "entity": {
    "name": "John",
    "surname": "Doe"
  }
}
