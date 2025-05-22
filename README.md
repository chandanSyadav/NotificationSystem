# SQL Server Database Setup

This project uses **two SQL Server databases**. The required scripts to set up these databases are included in the project.

## 📁 Database Scripts Location

You can find the database scripts in the following directory:

/DatabaseScripts/
│
├── entityScript.sql -- Script for the main database (e.g., entities, business logic)
└── userIdentity.sql -- Script for the user identity and authentication system
## ⚙️ How to Set Up the Databases

1. Open **SQL Server Management Studio (SSMS)** or any SQL query tool connected to your SQL Server instance.
2. Open the following scripts one by one:
   - `DatabaseScripts/entityScript.sql`
   - `DatabaseScripts/userIdentity.sql`
3. Run the scripts to create the necessary databases and tables.

> 💡 Ensure your SQL Server has appropriate permissions to create databases and tables.

## 📌 Note

- No additional configuration is needed to initialize the databases.
- After running these scripts, your project will have all required schema setup.

---

Feel free to customize the file names or descriptions based on your actual use case. Let me know if you want to include screenshots, database names, or how to connect from your application.
