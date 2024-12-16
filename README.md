 # ExpensesAPI

## Overview

ExpensesAPI is a simple RESTful API built using .NET 9.0 that allows users to manage and track their expenses.

### Supported Operations

- **Add an Expense**: Users can add an expense with details such as:
  - Type of expense
  - Amount
  - Currency
  - Date
  - Comment

- **Retrieve Expenses by User**: Fetch all expenses recorded by a specific user based on their unique user ID.

### Input Validation

The API includes comprehensive input validation:

- Validates expense type (limited to "Restaurant", "Hotel", "Misc")
- Ensures expense date:
  - Is not in the future
  - Is within the last 3 months
- Checks that expense currency matches the user's currency
- Requires essential fields like comment and amount
- Prevents duplicate expense entries

The API also includes basic unit tests that check the main functionalities and edge cases.

## Features

### Expense Types
- "Restaurant"
- "Hotel"
- "Misc"

### User Management
- Each user has a unique ID and currency
- Users are automatically created when expenses are added

### Predefined Users
1. **Anthony Stark**
   - ID: 1
   - Currency: USD

2. **Natasha Romanova**
   - ID: 2
   - Currency: RUB

## Prerequisites

- **.NET 9.0 SDK or higher**
- Code editor (Visual Studio or Visual Studio Code)
- Terminal (PowerShell, bash, etc.)

## Instructions

### Clone, Build, and Run

```bash
git clone https://github.com/uaupetit/ExpensesAPI.git
cd ExpensesAPI
dotnet build
dotnet run
```

### Add an expense

```bash
curl -X POST http://localhost:5000/api/expense -H "Content-Type: application/json" -d '{ "user": { "id": 1, "firstName": "Anthony", "lastName": "Stark", "currency": "USD" }, "date": "2024-12-01", "type": "Restaurant", "
```

### Response

{"message":"Expense created successfully!"}

### Run tests

```bash
dotnet test
```

The servicetestfile can be modifie for testing all the cases.