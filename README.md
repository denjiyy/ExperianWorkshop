# Server Architecture

The server architecture of the application is built using **Entity Framework Core (Version 8.0.8)**, **ASP.NET Core**, and the **.NET Framework 8.0**. The database is created using the **Code-First** approach, followed by the implementation of APIs to facilitate communication between the backend and frontend.

## Database Creation
Using the Code-First methodology, we defined the database schema directly within the application code. This allows for flexibility in creating and modifying database structures directly from the application, ensuring that the data models align with the business logic.

## API Endpoints

The following API endpoints are exposed to allow interaction with the system's data. The endpoints are managed via two main controllers:
- **UsersController**: Manages user-related operations.
- **AccountsController**: Manages bank account-related operations.

---

# UsersController Documentation

The `UsersController` is responsible for managing users within the Bank Management System. It includes operations for creating, reading, updating, and deleting users. Authorization is required for most of the endpoints to ensure proper security.

## **Endpoints**

### **GET**: `/api/Users`
- **Description**: Retrieves a list of all users in the system.
- **Authorization**: Required.
- **Response**:
  - **Status Code**: `200 OK`
  - **Body**: A list of all user objects.
  - **Example response**:
    ```json
    [
      {
        "id": 1,
        "email": "user@example.com",
        "username": "user1",
        "fullname": "John Doe",
        "dateOfBirth": "1990-01-01",
        "status": "Active"
      },
      ...
    ]
    ```

### **GET**: `/api/Users/{id}`
- **Description**: Retrieves the details of a specific user by `id`.
- **Authorization**: Required. Only administrators or the user themself can access the details.
- **Parameters**:
  - `id` (integer, required): The unique identifier of the user.
- **Response**:
  - **Status Code**: `200 OK` (if the user exists) or `404 Not Found` (if the user is not found).
  - **Body**: The details of the user.
  - **Example response**:
    ```json
    {
      "id": 1,
      "email": "user@example.com",
      "username": "user1",
      "fullname": "John Doe",
      "dateOfBirth": "1990-01-01",
      "status": "Active"
    }
    ```

### **POST**: `/api/Users`
- **Description**: Creates a new user in the system.
- **Authorization**: Not required.
- **Request Body**:
  - `email` (string, required): The user's email.
  - `username` (string, required): The user's username.
  - `fullname` (string, required): The user's full name.
  - `password` (string, required): The user's password.
  - `dateOfBirth` (string, required): The user's date of birth (in `YYYY-MM-DD` format).
- **Response**:
  - **Status Code**: `201 Created`
  - **Body**: The newly created user object with an auto-generated `id`.
  - **Example request**:
    ```json
    {
      "email": "newuser@example.com",
      "username": "newuser",
      "fullname": "Jane Doe",
      "password": "securePassword123",
      "dateOfBirth": "1995-06-15"
    }
    ```
  - **Example response**:
    ```json
    {
      "id": 2,
      "email": "newuser@example.com",
      "username": "newuser",
      "fullname": "Jane Doe",
      "dateOfBirth": "1995-06-15",
      "status": "Active"
    }
    ```

### **PUT**: `/api/Users/{id}`
- **Description**: Updates the details of an existing user by `id`.
- **Authorization**: Required. Only administrators or the user themself can update the details.
- **Parameters**:
  - `id` (integer, required): The unique identifier of the user.
- **Request Body**:
  - `fullname` (string, required): The updated full name of the user.
  - `email` (string, required): The updated email of the user.
  - `newPassword` (string, optional): A new password for the user (if they wish to change it).
- **Response**:
  - **Status Code**: `204 No Content` (if the update is successful) or `400 Bad Request` (if the input data is invalid).
  - **Example request**:
    ```json
    {
      "id": 1,
      "fullname": "Johnathan Doe",
      "email": "johnathan.doe@example.com",
      "newPassword": "newSecurePassword"
    }
    ```

### **DELETE**: `/api/Users/{id}`
- **Description**: Marks a user account as "Closed" by changing its status. Only administrators can delete users.
- **Authorization**: Required.
- **Parameters**:
  - `id` (integer, required): The unique identifier of the user to be deleted.
- **Response**:
  - **Status Code**: `204 No Content` (if the deletion is successful) or `404 Not Found` (if the user is not found).

---

# AccountsController Documentation

The `AccountsController` is responsible for managing bank accounts. It allows users to create, read, update, and delete bank accounts within the system. This controller ensures that only authorized users (or administrators) can view or modify accounts.

## **Endpoints**

### **GET**: `/api/Accounts`
- **Description**: Retrieves a list of all accounts.
- **Authorization**: Not required.
- **Response**:
  - **Status Code**: `200 OK`
  - **Body**: A list of account details.
  - **Example response**:
    ```json
    [
      {
        "accountNumber": "12345678",
        "balance": 1000.0,
        "accountType": "Savings",
        "status": "Active",
        "dateOpened": "2023-01-01"
      },
      ...
    ]
    ```

### **GET**: `/api/Accounts/{id}`
- **Description**: Retrieves the details of a specific account by `id`.
- **Authorization**: Required. Only administrators or the account owner can view the details.
- **Parameters**:
  - `id` (integer, required): The unique identifier of the account.
- **Response**:
  - **Status Code**: `200 OK` (if the account exists) or `404 Not Found` (if the account is not found).
  - **Body**: The details of the account.
  - **Example response**:
    ```json
    {
      "id": 1,
      "accountNumber": "12345678",
      "balance": 1000.0,
      "accountType": "Savings",
      "status": "Active",
      "dateOpened": "2023-01-01"
    }
    ```

### **POST**: `/api/Accounts`
- **Description**: Creates a new account.
- **Authorization**: Required.
- **Request Body**:
  - `accountNumber` (string, required): The account number.
  - `accountType` (string, required): The type of account (e.g., `Savings`, `Checking`).
  - `balance` (decimal, required): Initial balance for the account (admins only).
  - `status` (string, required): The status of the account (`Active`, `Closed`, etc.).
  - `currencyType` (string, required): The currency of the account.
  - `userId` (integer, required): The ID of the user to whom the account belongs.
- **Response**:
  - **Status Code**: `201 Created`
  - **Body**: The created account.
  - **Example request**:
    ```json
    {
      "accountNumber": "12345678",
      "accountType": "Savings",
      "balance": 0,
      "status": "Active",
      "currencyType": "USD",
      "userId": 1
    }
    ```

### **PUT**: `/api/Accounts/{id}`
- **Description**: Updates the details of an account.
- **Authorization**: Required. Only administrators can update the account balance.
- **Parameters**:
  - `id` (integer, required): The unique identifier of the account.
- **Request Body**:
  - `accountType` (string, required): The updated account type.
  - `balance` (decimal, optional): The updated balance (admins only).
  - `status` (string, required): The updated status.
- **Response**:
  - **Status Code**: `204 No Content` (if the update is successful) or `400 Bad Request` (if the input data is invalid).
  - **Example request**:
    ```json
    {
      "id": 1,
      "accountType": "Checking",
      "balance": 1000.0,
      "status": "Active"
    }
    ```

### **DELETE**: `/api/Accounts/{id}`
- **Description**: Deletes an account by `id`. Only administrators or account owners can delete accounts.
- **Authorization**: Required.
- **Parameters**:
  - `id` (integer, required): The unique identifier of the account.
- **Response**:
  - **Status Code**: `204 No Content` (if the deletion is successful) or `404 Not Found` (if the account is not found).

---

# Data Models

## User

The `User` model represents a system user and has the following properties:

- `Id` (integer): The unique identifier for the user.
- `Email` (string): The user's email.
- `Username` (string): The user's username.
- `Fullname` (string): The user's full name.
- `PasswordHash` (string): The user's password, stored securely.
- `DateOfBirth` (DateTime): The user's date of birth.
- `Status` (enum): The user's status (e.g., Active, Closed).

## Account

The `Account` model represents a bank account and has the following properties:

- `Id` (integer): The unique identifier for the account.
- `AccountNumber` (string): The account number.
- `Balance` (decimal): The current balance of the account.
- `AccountType` (enum): The type of account (e.g., Savings, Checking).
- `Status` (enum): The current status of the account (e.g., Active, Closed).
- `DateOpened` (DateOnly): The date the account was opened.
- `CurrencyType` (enum): The currency used for the account.
- `UserId` (integer): The identifier of the user who owns the account.
