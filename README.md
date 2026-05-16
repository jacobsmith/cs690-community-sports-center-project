# cs690-community-sports-center-project

## Development

The database is a SQLite database. To create changes to the database:

1. Make any updates in `SportsTracker/Models` necessary (add new model, add new column, etc.)
1a. Make sure any new models are added to the `SportsTracker/Data/AppDbContext.cs` as this class is introspected to build the migrations.
2. Run `dotnet ef migrations add <Name>` - this creates the migration based on the changes in the SportsTracker/Models directory
3. `dotnet ef database update` will apply those updates to the database.


