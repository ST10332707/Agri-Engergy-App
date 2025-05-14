# Agri-Engergy App
Agri-Energy App is an ASP.NET Core MVC web application designed for managing farmer products and employee shortlisting.
It enables authenticated users to view, filter, and manage product lists, with the employee being able to shortlist farmers.
## Techologies used:
- ASP.NET Core MVC
- SQLite
- c#
- Razor Views
## 1.Development Environment Setup
### Prerequistes
- .Net 7 SDK or later
- Visual Studio 2022+
- SQLite
- Git
## 2. Build and Run Web Application
### Using Visual Studio 2022
- Unzipp solution file
- Open the solutin file (.sln)
- Set Agri_Energy_App as the startup project
- Press F5 or click Run
## 3. System Functionalities and User Roles
### 3.1 Roles
- Admin: Mange users and system settings
- Employee: Can shortlist a maximum of 10 farmers and can view and filter farmer products
- Farmer: Add and manage thier products listings
### 3.2 Key Features:
#### 3.2.1 Features accessible to Admin
- User Authentication/Login
- View Products
- Filter Products
- Session-Based Access Control
#### 3.2.2 Features accessible to Employee
- User Authentication/Login
- View Products
- Filter Products
- Shortlist Farmers
- Session-Based Access Control
#### 3.2.3 Features accessible to Farmer
- User Authentication/Login
- View Products
- Add Products
- Session-Based Access Control
## 4 Project Structure
### Agri_Energy_App/
#### Controller/
- EmployeeController.cs
- FarmerController.cs
- HomeController.cs
- LoginController.cs
- SignUpController.cs
#### Data/
- AgriDatabase.db
- AgriDatabase.sqbpro
- AppDbContext.cs
#### Models/
- ErrorViewModel.cs
- FarmerProductGroup.cs
- LoginModel.cs
- ProductTable.cs
- ShortlistedFarmer.cs
- ShortlistService.cs
- UserModel.cs
- UserTable.cs
#### Views/
##### Employee/
- DisplayPoduct.cshtml
- Index.cshtml
- ShortlistedFarmers.cshtml
##### Farmer/
- AddProduct.cshtml
- DisplayProduct.cshtml
- Index.cshtml
##### Home/
- Index.cshtml
- Privacy.cshtml
- SignUp.cshtml
##### Login/
- Login.cshtml
- LoginFailed.cshtml
##### wwwroot/
##### Program.cs





