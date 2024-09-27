# Bank System

App running at:
  - Local:   http://localhost:3000/

### Project setup

At *frontend folder* run:
```
npm install
npm start
```
Start the backed project and run data base migrations

At *AI folder* run:
```
python app.py
```


# Server Architecture

The server architecture of the application is built using **Entity Framework Core (Version 8.0.8)**, **ASP.NET Core**, and the **.NET Framework 8.0**. The database is created using the **Code-First** approach, followed by the implementation of APIs to facilitate communication between the backend and frontend.

## Database Creation
Using the Code-First methodology, we defined the database schema directly within the application code. This allows for flexibility in creating and modifying database structures directly from the application, ensuring that the data models align with the business logic.

## API Endpoints

The following API endpoints are exposed to allow interaction with the system's data. The endpoints are managed via four main controllers:
- **UsersController**: Manages user-related operations.
- **AccountsController**: Manages bank account-related operations.
- **CardsController**: Manages card-related operations.
- **DepositCalculatorController**: Manages deposit calculations.

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
- **Authorization**: Required. Only administrators or the user themselves can access the details.
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
- **Authorization**: Required. Only administrators or the user themselves can update the details.
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
      "balance": 500.0,
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

# CardsController Documentation

The `CardsController` is responsible for managing card-related operations within the Bank Management System. It includes endpoints for creating, reading, updating, and deleting cards associated with user accounts.

## **Endpoints**

### **GET**: `/api/Cards`
- **Description**: Retrieves a list of all cards in the system.
- **Authorization**: Required. Non-administrators can only view their own cards.
- **Response**:
  - **Status Code**: `200 OK`
  - **Body**: A list of card objects.
  - **Example response**:
    ```json
    [
      {
        "cardNumber": "4111111111111111",
        "cardType": "Debit",
        "expiryDate": "2025-12-31",
        "status": "Active",
        "issueDate": "2023-01-01",
        "cvv": "Hidden"
      },
      ...
    ]
    ```

### **GET**: `/api/Cards/{id}`
- **Description**: Retrieves the details of a specific card by `id`.
- **Authorization**: Required. Only administrators or the card owner can view the details.
- **Parameters**:
  - `id` (integer, required): The unique identifier of the card.
- **Response**:
  - **Status Code**: `200 OK` (if the card exists) or `404 Not Found` (if the card is not found).
  - **Body**: The details of the card.
  - **Example response**:
    ```json
    {
      "cardNumber": "4111111111111111",
      "cardType": "Debit",
      "expiryDate": "2025-12-31",
      "status": "Active",
      "issueDate": "2023-01-01",
      "cvv": "Hidden"
    }
    ```

### **POST**: `/api/Cards`
- **Description**: Creates a new card in the system.
- **Authorization**: Required. Non-administrators can only create cards for their own accounts.
- **Request Body**:
  - `cardNumber` (string, required): The card number.
  - `cardType` (string, required): The type of card (e.g., `Debit`, `Credit`).
  - `expiryDate` (string, required): The card's expiry date (in `YYYY-MM-DD` format).
  - `issueDate` (string, required): The date the card was issued (in `YYYY-MM-DD` format).
  - `status` (string, required): The status of the card (e.g., `Active`, `Blocked`).
  - `cvv` (string, required): The card's CVV number.
  - `accountId` (integer, required): The ID of the account to which the card belongs.
- **Response**:
  - **Status Code**: `201 Created`
  - **Body**: The created card object with its `id`.
  - **Example request**:
    ```json
    {
      "cardNumber": "4111111111111111",
      "cardType": "Debit",
      "expiryDate": "2025-12-31",
      "issueDate": "2023-01-01",
      "status": "Active",
      "cvv": "123",
      "accountId": 1
    }
    ```

### **PUT**: `/api/Cards/{id}`
- **Description**: Updates the details of a card.
- **Authorization**: Required. Only administrators or the card owner can update the card details.
- **Parameters**:
  - `id` (integer, required): The unique identifier of the card.
- **Request Body**:
  - `cardNumber` (string, required): The updated card number.
  - `cardType` (string, required): The updated card type.
  - `expiryDate` (string, required): The updated expiry date.
  - `status` (string, required): The updated status.
  - `cvv` (string, required): The updated CVV number.
- **Response**:
  - **Status Code**: `204 No Content` (if the update is successful) or `400 Bad Request` (if the input data is invalid).
  - **Example request**:
    ```json
    {
      "cardNumber": "4111111111111111",
      "cardType": "Debit",
      "expiryDate": "2026-12-31",
      "status": "Active",
      "cvv": "456"
    }
    ```

### **DELETE**: `/api/Cards/{id}`
- **Description**: Deletes a card by `id`. Only administrators or card owners can delete cards.
- **Authorization**: Required.
- **Parameters**:
  - `id` (integer, required): The unique identifier of the card.
- **Response**:
  - **Status Code**: `204 No Content` (if the deletion is successful) or `404 Not Found` (if the card is not found).

---

# LoansController Documentation

The `LoansController` is responsible for managing loan-related operations within the Bank Management System. It provides endpoints to create, read, update, and delete loans associated with users.

## **Endpoints**

### **GET**: `/api/Loans`
- **Description**: Retrieves a list of all loans in the system. Only loans associated with the logged-in user are returned for non-administrators.
- **Authorization**: Required.
- **Response**:
  - **Status Code**: `200 OK`
  - **Body**: A list of loan objects.
  - **Example response**:
    ```json
    [
      {
        "id": 1,
        "amount": 5000,
        "interestRate": 5.0,
        "currencyType": "USD",
        "dateApproved": "2023-01-01",
        "startDate": "2023-01-15",
        "nextPaymentDate": "2023-02-01",
        "endDate": "2024-01-15",
        "loanType": "Personal",
        "loanStatus": "Active",
        "userId": 1
      },
      ...
    ]
    ```

### **GET**: `/api/Loans/{id}`
- **Description**: Retrieves the details of a specific loan by `id`.
- **Authorization**: Required. Only administrators or the loan owner can view the details.
- **Parameters**:
  - `id` (integer, required): The unique identifier of the loan.
- **Response**:
  - **Status Code**: `200 OK` (if the loan exists) or `404 Not Found` (if the loan is not found).
  - **Body**: The details of the loan.
  - **Example response**:
    ```json
    {
      "id": 1,
      "amount": 5000,
      "interestRate": 5.0,
      "currencyType": "USD",
      "dateApproved": "2023-01-01",
      "startDate": "2023-01-15",
      "nextPaymentDate": "2023-02-01",
      "endDate": "2024-01-15",
      "loanType": "Personal",
      "loanStatus": "Active",
      "userId": 1
    }
    ```

### **POST**: `/api/Loans`
- **Description**: Creates a new loan in the system.
- **Authorization**: Required. Non-administrators can only create loans for their own accounts.
- **Request Body**:
  - `Amount` (decimal, required): The amount of the loan.
  - `InterestRate` (decimal, required): The annual interest rate of the loan.
  - `CurrencyType` (string, required): The currency type of the loan.
  - `DateApproved` (DateTime, required): The date the loan was approved.
  - `StartDate` (DateTime, required): The start date of the loan.
  - `EndDate` (DateTime, required): The end date of the loan.
  - `LoanType` (string, required): The type of loan.
  - `LoanStatus` (string, required): The status of the loan.
- **Response**:
  - **Status Code**: `201 Created`
  - **Body**: The created loan object with its `id`.
  - **Example request**:
    ```json
    {
      "Amount": 5000,
      "InterestRate": 5.0,
      "CurrencyType": "USD",
      "DateApproved": "2023-01-01",
      "StartDate": "2023-01-15",
      "EndDate": "2024-01-15",
      "LoanType": "Personal",
      "LoanStatus": "Active"
    }
    ```

### **PUT**: `/api/Loans/{id}`
- **Description**: Updates the details of a loan.
- **Authorization**: Required. Only administrators or the loan owner can update the loan details.
- **Parameters**:
  - `id` (integer, required): The unique identifier of the loan.
- **Request Body**:
  - `Amount` (decimal, required): The updated amount of the loan.
  - `InterestRate` (decimal, required): The updated interest rate of the loan.
  - `CurrencyType` (string, required): The updated currency type of the loan.
  - `DateApproved` (DateTime, required): The updated date the loan was approved.
  - `StartDate` (DateTime, required): The updated start date of the loan.
  - `EndDate` (DateTime, required): The updated end date of the loan.
  - `LoanType` (string, required): The updated type of loan.
  - `LoanStatus` (string, required): The updated status of the loan.
- **Response**:
  - **Status Code**: `204 No Content` (if the update is successful) or `400 Bad Request` (if the input data is invalid).
  - **Example request**:
    ```json
    {
      "Amount": 6000,
      "InterestRate": 4.5,
      "CurrencyType": "USD",
      "DateApproved": "2023-01-01",
      "StartDate": "2023-01-15",
      "EndDate": "2024-02-15",
      "LoanType": "Home",
      "LoanStatus": "Active"
    }
    ```

### **DELETE**: `/api/Loans/{id}`
- **Description**: Deletes a loan by `id`. Only administrators or loan owners can delete loans.
- **Authorization**: Required.
- **Parameters**:
  - `id` (integer, required): The unique identifier of the loan.
- **Response**:
  - **Status Code**: `204 No Content` (if the deletion is successful) or `404 Not Found` (if the loan is not found).

---

# Data Models

## Loan

The `Loan` model represents a loan in the system and has the following properties:

- `Id` (integer): The unique identifier for the loan.
- `Amount` (decimal): The amount of the loan.
- `InterestRate` (decimal): The annual interest rate of the loan.
- `CurrencyType` (enum): The currency type of the loan.
- `DateApproved` (DateTime): The date the loan was approved.
- `StartDate` (DateTime): The start date of the loan.
- `NextPaymentDate` (DateTime): The next payment date of the loan.
- `EndDate` (DateTime): The end date of the loan.
- `LoanType` (enum): The type of the loan (e.g., Personal, Home).
- `LoanStatus` (enum): The current status of the loan (e.g., Active, PaidOff).
- `UserId` (integer): The identifier of the user who owns the loan.

## LoansDto

The `LoansDto` model is used to create or update loan requests and has the following properties:

- `Amount` (decimal): The amount of the loan.
- `InterestRate` (decimal): The annual interest rate of the loan.
- `CurrencyType` (string): The currency type of the loan.
- `DateApproved` (DateTime): The date the loan was approved.
- `StartDate` (DateTime): The start date of the loan.
- `EndDate` (DateTime): The end date of the loan.
- `LoanType` (string): The type of loan.
- `LoanStatus` (string): The status of the loan.

---

# Notes

- The maximum number of active loans a user can have is limited to 10. If a user tries to create more than 10 active loans, a `400 Bad Request` response is returned with an appropriate message.
- Loans can be denied based on risk assessment; if the calculated risk score is below a certain threshold, a `400 Bad Request` response is returned.

---

# DepositCalculatorController Documentation

The `DepositCalculatorController` is responsible for calculating the total amount accrued from a deposit based on compound interest.

## **Endpoints**

### **POST**: `/api/DepositCalculator/calculate`
- **Description**: Calculates the total amount from a deposit after a specified time period using compound interest.
- **Request Body**:
  - `Amount` (decimal, required): The initial deposit amount.
  - `Rate` (decimal, required): The annual interest rate in percentage.
  - `TimesCompounded` (integer, required): How many times the interest is compounded per year.
  - `TimePeriodInYears` (integer, required): The duration for which the money is invested in years.
- **Response**:
  - **Status Code**: `200 OK`
  - **Body**: The calculated details of the deposit.
  - **Example request**:
    ```json
    {
      "Amount": 1000,
      "Rate": 5,
      "TimesCompounded": 12,
      "TimePeriodInYears": 10
    }
    ```
  - **Example response**:
    ```json
    {
      "InitialAmount": 1000,
      "InterestRate": 5,
      "TimePeriodInYears": 10,
      "TotalAmount": 1648.72
    }
    ```

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

## Card

The `Card` model represents a card associated with a bank account. It has the following properties:

- `Id` (integer): The unique identifier for the card.
- `CardNumber` (string): The card number.
- `CardType` (enum): The type of card (e.g., Debit, Credit).
- `ExpiryDate` (DateTime): The date when the card expires.
- `IssueDate` (DateTime): The date when the card was issued.
- `CVV` (string): The card's CVV number (hashed for security).
- `Status` (enum): The current status of the card (e.g., Active, Blocked).
- `AccountId` (integer): The identifier of the account associated with the card.

## DepositCalculationRequest

The `DepositCalculationRequest` model represents the request for calculating deposit interest and has the following properties:

- `Amount` (decimal): The initial deposit amount.
- `Rate` (decimal): The annual interest rate as a percentage.
- `TimesCompounded` (integer): The number of times the interest is compounded per year.
- `TimePeriodInYears` (integer): The duration of the investment in years.

