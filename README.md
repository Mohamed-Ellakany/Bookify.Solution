# Bookify

Book catalog management web app built with ASP.NET Core MVC.

## Features
- Manage books, book copies, authors and categories
- User management with roles (ASP.NET Identity, including two-factor pages)
- Book cover upload with Cloudinary
- AJAX modal forms

## Tech stack
C#, ASP.NET Core MVC (.NET 10), Entity Framework Core, SQL Server, Identity, AutoMapper, Cloudinary

## Run locally
Requires the .NET 10 SDK and SQL Server (LocalDB is the default in `appsettings.json`).

1. Set `CloudinarySettings` and `MailSettings` with user secrets or `appsettings.Development.json`.
2. Create the database: `dotnet ef database update --project Bookify.Web`
3. Run: `dotnet run --project Bookify.Web`. Default roles and an admin user are seeded on startup.
