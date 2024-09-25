# Server architecture
The server architecture of the application has been created using EntityFrameworkCore (Version 8.0.8) ASP.NET Core and .NET Framework 8.0.
We created the database using code-first approach and then we did the APIs to connect to the front-end. The API endpoints and their expected outputs are as follows:
**UsersController**
-GET: /api/Users
--This should return all of the entries in the database for the users
-GET: /api/Users/{id}
--This should return the user entry with a certain id given to it as an argument,
-POST: /api/Users
--This should insert an entry into the database with the values given to it by the user (it expects "email", "username", "fullname", "password", "dateofbirth", "")
