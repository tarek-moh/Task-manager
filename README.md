# Task Manager Website

This is a web-based task management system built with ASP.NET Core and SQL Server. The application allows users to manage tasks, assign them to employees, and track their progress.

## Features

- Task management (Create, Read, Update, Delete tasks)
- User authentication and role-based access
- Assign tasks to employees
- Comments on tasks
- Task filtering and prioritization

## Prerequisites

Before running the project, ensure you have **SQL Server** installed and running on your machine. 

All other tasks like building the project and running the server can be done via the command line using the .NET CLI.

### Installing SQL Server

- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- **.NET Core SDK** ([Download the latest version](https://dotnet.microsoft.com/download)).

## Getting Started

### 1. Clone the repository

First, clone the project from GitHub:

```bash
git clone https://github.com/tarek-moh/task-manager.git
cd task-manager
```
### 2. Database Setup
1. Open SQL Server Management Studio (SSMS).
2. Connect to your local SQL Server instance (e.g., (local)\local, localhost, etc.)
   -make sure this is the same as the servername in the connections string in appsettings.json file
3. In the Object Explorer, right-click on Databases and select New Database...
   -Name the database "Task Manager".
4.Open the TaskManagerDB_Script.sql file, located in the Database/ folder of this repository.
   -In SSMS, go to File > Open > File and select the TaskManagerDB_Script.sql file.
5. Execute the SQL script by clicking the Execute button (or pressing F5).
   -This will create the required tables and insert admin account into the database.
### 3. Configure the Connection String
1. Open the appsettings.json file in the project root directory.
2. Update the ConnectionStrings section to match your local SQL Server instance:
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=local;Database=TaskManagerDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```
-Replace localhost with your SQL Server instance name if it's different, such as (local)\SQLEXPRESS.
### 4. Running the Application
1. In the project directory, open a terminal or command prompt and run the following commands:
```bash
dotnet restore
dotnet run
```
2. Navigate to the link provided after runing the command. eg: https://localhost:1000/
3. login with the admin account email: admin@Task-manager.com    password: verySecret
