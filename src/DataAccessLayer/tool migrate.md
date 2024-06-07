# If your EF Tool older than runtime version:
// latest version
dotnet tool update --global dotnet-ef

// full cmd
dotnet tool update --global dotnet-ef --version 3.1.0

# Please make sure you have configured  
ConnectionStrings in appsetting.json of Main Project (API, WPF, BlazorPages)
before you migration or update db

# When you charge database C# code, please add migration:
- dotnet ef migrations add InitialCreate
- dotnet ef migrations add TheChangeOfThisMigrationYouMade

# When you want to create or update database from migration:
dotnet ef database update