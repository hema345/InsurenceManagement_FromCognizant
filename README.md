## Prerequisites
 
Before you begin, ensure you have the following installed:
 
### Backend 
* [.NET SDK](https://dotnet.microsoft.com/download) (specify version, e.g., **.NET 8.0 SDK**)
* **SQL Server 2022 Express or Developer Edition** with **SQL Server Management Studio (SSMS)**
* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (Recommended IDE for C# development)
 
### Frontend 
* [Node.js](https://nodejs.org/en/download/) (latest LTS version recommended)
* [Angular CLI](https://angular.io/cli) (Install globally: `npm install -g @angular/cli`)
* [Visual Studio Code](https://code.visualstudio.com/) (Recommended IDE for Angular development)
 
---
 
## Technologies Used
 
This project harnesses the power of the following technologies:
 
### Backend 
* **.NET 8**
* **C# 12**
* **ASP.NET Web API**
* **EF Core 8** (Entity Framework Core)
* **SQL Server 2022**
* **AutoMapper**
* **JWT** (JSON Web Tokens)
 
### Frontend 
* **Angular 19**
* **TypeScript**
* **HTML5**
* **CSS3**, **Angular Material**
 
---
 
## Getting Started
 
Once you have cloned the repository, follow these instructions to get the application running on your local machine for development and testing purposes.
 
### Backend Setup
 
After cloning the repository:
 
1.  **Open and Build Solution:**
    Open the `<Sln Name>.sln` solution file in **Visual Studio 2022** and build the solution. This will restore all necessary NuGet packages.
 
2.  **Configure Database Connection:**
    Locate the `appsettings.json` file in the Backend project. Update the `ConnectionStrings` section to point to your SQL Server instance.
 
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=YourServerName;Database=YourDatabaseName;Integrated Security=True;TrustServerCertificate=True;"
      //  Change the details based on your SQL Server configuration and requirements.
      // Example for SQL Server Express: "Server=.\\SQLEXPRESS;Database=YourDatabaseName;Integrated Security=True;TrustServerCertificate=True;"
    }
    ```
 
3.  **Configuring Database (using EF Core Migrations):**
    To create the database and its schema in your local machine using Entity Framework Core migrations, follow these steps:
 
    * **Step 1:** In Visual Studio, go to `Tools` > `NuGet Package Manager` > `Package Manager Console`.
    * **Step 2:** Ensure the default project in the Package Manager Console is set to your Backend project (e.g., `YourProjectName.Api` or the project containing your `DbContext`).
    * **Step 3:** Type the command `Add-Migration "InitialCreate"` and press Enter. (You can choose any descriptive name for your migration).
    * **Step 4:** Type the command `Update-Database` and press Enter.
        This will create the database (if it doesn't exist) and apply all pending migrations, setting up your tables and seeding initial data if configured in your `DbContext`.
 
    Now your backend is all set up for running!
 
4.  **Run the Backend Application:**
    You have a few options to run the backend:
 
    * **Using `dotnet run` (Terminal):**
        Open a terminal in the Backend project's directory (e.g., `cd YourProjectName/Backend/YourProjectName.Api`) and run:
        ```bash
        dotnet run
        ```
    * **Using Visual Studio (IDE):**
        Simply click on the 'HTTPS' written button (the green play button) in Visual Studio 2022.
 
    The backend API should now be running, typically on `https://localhost:7153`. The exact port will be displayed in the console or in Visual Studio's output window.
 
### Frontend Setup
 
1.  **Navigate to the Frontend directory:**
    ```bash
    cd ../Frontend
    ```
    *(Assuming you are currently in the Backend directory after cloning)*
 
2.  **Install npm packages:**
    ```bash
    npm install
    ```
 
3.  **Configure API Base URL:**
    Open `src/environments/environment.ts`. Update the `baseApiUrl` variable to match your backend API URL (e.g., `https://localhost:7153/api`).
 
    ```typescript
    export const environment = {
      production: false,
      baseApiUrl: 'https://localhost:7153/api' //  Adjust this to your backend API URL
    };
    ```
 
4.  **Run the Frontend Application:**
    ```bash
    ng serve --open
    ```
    This command will compile the Angular application and open it in your default web browser (usually `http://localhost:4200`). The `--open` flag automatically opens the browser.
 
---
 
## How to Test the Application
 
The admin data and some initial data are already seeded in the database. First, you need to log in accordingly using the provided credentials.
 
### Admin Credentials 
* **Username**: `Krishna_20`
* **Password**: `Krishna@20`
 
### Agent Credentials 
* **Username**: `Seethayya_1980`
* **Password**: `Seethayya@1980`
 
### Customer Credentials 
* **Username**: `Rathna_2004`
* **Password**: `Rathna@2004`
 
You can explore all the functionality using these credentials. Enjoy testing!