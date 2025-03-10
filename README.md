# Library Management System API

This is a **Library Management System API** built with ASP.NET Core. It provides endpoints for managing books, patrons, and borrowing records. The API supports **JWT-based authentication** for secure access to protected endpoints.

---

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Running the Application](#running-the-application)
3. [Interacting with API Endpoints](#interacting-with-api-endpoints)
4. [JWT Authentication](#jwt-authentication)
5. [API Endpoints](#api-endpoints)
6. [Testing the API](#testing-the-api)
7. [Contributing](#contributing)

---

## Prerequisites

Before running the application, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Postman](https://www.postman.com/) (for testing API endpoints) or test it with Swagger in the Browser
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

---

## Running the Application

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/reem-brimo/E-Library.git
   cd library-management-system
2. **Set Up the Database**:
   - If using SQL Server, update the connection string in `appsettings.json`:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=your-server;Database=LibraryDB;User Id=your-user;Password=your-password;"
     }
     ```

3. **Run the Application**:
   - Using Visual Studio:
     - Open the solution file (`LibraryManagementSystem.sln`).
     - Press `F5` to run the application.
   - Using the Command Line:
     ```bash
     dotnet run --project Library.API
     ```

4. **Access the API**:
   - The API will be available at `https://localhost:5001` (or `http://localhost:5000`).

---

## Interacting with API Endpoints

You can interact with the API using tools like **Postman** or **cURL**. Below are examples of common operations.

### 1. **Get All Books**
   - **Endpoint**: `GET /api/books`
   - **Example**:
     ```bash
     curl -X GET https://localhost:5001/api/books
     ```

### 2. **Get a Book by ID**
   - **Endpoint**: `GET /api/books/{id}`
   - **Example**:
     ```bash
     curl -X GET https://localhost:5001/api/books/1
     ```

### 3. **Create a Book**
   - **Endpoint**: `POST /api/books`
   - **Example**:
     ```bash
     curl -X POST https://localhost:5001/api/books \
     -H "Content-Type: application/json" \
     -d '{
           "title": "Test Book",
           "author": "Test Author",
           "isbn": "1234567890",
           "publicationYear": 2023
         }'
     ```

### 4. **Update a Book**
   - **Endpoint**: `PUT /api/books/{id}`
   - **Example**:
     ```bash
     curl -X PUT https://localhost:5001/api/books/1 \
     -H "Content-Type: application/json" \
     -d '{
           "title": "Updated Book",
           "author": "Updated Author",
           "isbn": "1234567890",
           "publicationYear": 2023
         }'
     ```

### 5. **Delete a Book**
   - **Endpoint**: `DELETE /api/books/{id}`
   - **Example**:
     ```bash
     curl -X DELETE https://localhost:5001/api/books/1
     ```

---

## JWT Authentication

The API uses **JWT (JSON Web Tokens)** for authentication. Follow these steps to authenticate and access protected endpoints.

### 1. **Register a User**
   - **Endpoint**: `POST /api/auth/register`
   - **Example**:
     ```bash
     curl -X POST https://localhost:5001/api/auth/register \
     -H "Content-Type: application/json" \
     -d '{
           "username": "testuser",
           "password": "Test@123"
         }'
     ```

### 2. **Login and Get JWT Token**
   - **Endpoint**: `POST /api/auth/login`
   - **Example**:
     ```bash
     curl -X POST https://localhost:5001/api/auth/login \
     -H "Content-Type: application/json" \
     -d '{
           "username": "testuser",
           "password": "Test@123"
         }'
     ```
   - **Response**:
     ```json
     {
       "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
     }
     ```

### 3. **Access Protected Endpoints**
   - Include the JWT token in the `Authorization` header:
     ```bash
     curl -X GET https://localhost:5001/api/books \
     -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
     ```

---

## API Endpoints

### **Books**
- `GET /api/Books` - Get all books.
- `GET /api/Books/{id}` - Get a book by ID.
- `POST /api/Books` - Create a new book.
- `PUT /api/Books/{id}` - Update a book.
- `DELETE /api/Books/{id}` - Delete a book.

### **Patrons**

- `GET /api/Patrons` - Get all Patrons.
- `GET /api/Patrons/{id}` - Get a patron by ID.
- `POST /api/Patrons` - Create a new patron.
- `PUT /api/Patrons/{id}` - Update a patron.
- `DELETE /api/Patrons/{id}` - Delete a patron.

### **Borrowing**
- `POST /api/borrow/{bookId}/patron/{patronId}` - Allow a patron to borrow a book.
- `PUT /api/return/{bookId}/patron/{patronId}` - Record the return of a borrowed book by a patron.

### **Authentication**
- `POST /api/Accounts/register` - Register a new user.
- `POST /api/Accounts/login` - Login and get a JWT token.

---

## Testing the API

You can test the API using **Postman** or **cURL**. Follow these steps:

1. **Register a User**:
   - Use the `/api/Accounts/register` endpoint to create a new user.

2. **Login and Get JWT Token**:
   - Use the `/api/Accounts/login` endpoint to get a JWT token.

3. **Access Protected Endpoints**:
   - Include the JWT token in the `Authorization` header to access protected endpoints.

---

Thank you for using the Library Management System API! 🚀
