# Student License Management Application

A web application for managing student licenses, featuring a frontend built with Angular and a backend powered by .NET Web API and MySQL.

https://github.com/user-attachments/assets/77b0175b-cfd1-408b-a548-afe7256f7330

## Project Setup

### Frontend

The frontend is built with Angular and can be set up using the following steps:

1. Navigate to the `ui` directory:
   ```bash
   cd ui
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the Angular application:
   ```bash
   npm start
   ```

The frontend application will be available at `http://localhost:4200`.

### Backend

The backend is built using .NET Web API and connects to a MySQL database.

#### Setup Instructions:

1. Set up MySQL and create a database called `license_application`:
   ```sql
   CREATE DATABASE license_application;
   ```

2. Update your MySQL credentials in the `.env` file located in the `Server` directory:
   ```properties
   DB_HOST=your_mysql_host
   DB_USER=your_mysql_user
   DB_PASSWORD=your_mysql_password
``
 DB_NAME=license_application
   JWT_SECRET_KEY=your_jwt_secret_key
   JWT_EXPIRY_HOURS=1
   ADMIN_PASSWORD=your_admin_password
   ``
3. Navigate to the `Server` directory:


4. Install necessary backend packages:
   ```bash
   dotnet restore
   ```

5. Run the backend Web API:
   ```bash
   dotnet run
   ```

The backend application will be available at `http://localhost:5190`.

## Technologies Used

- **Frontend**: Angular, TypeScript, HTML, CSS
- **Backend**: .NET Web API
- **Database**: MySQL

## Project Structure

- `ui`: Contains the Angular frontend application.
- `Server`: Contains the ASP.NET Core backend application.

## Features

- User authentication with JWT
- Student license application submission
- Admin panel for managing applications
- File upload and download functionality

![Screenshot 2024-10-07 225327](https://github.com/user-attachments/assets/aa69a832-7a75-4511-acea-f67fd91c101c)

![Screenshot 2024-10-07 225309](https://github.com/user-attachments/assets/ec1be54e-e1af-4f34-b765-8fa77f44c62d)

![Screenshot 2024-10-07 225252](https://github.com/user-attachments/assets/45d86f51-76c1-439b-9cb4-d1e745b00a82)
