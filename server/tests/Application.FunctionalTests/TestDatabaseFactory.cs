namespace server.Application.FunctionalTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync()
    {
        // Testcontainers requires Docker. To use a local SQL Server database instead,
        // switch to `SqlTestDatabase` and update appsettings.json.
        var database = new SqlTestcontainersTestDatabase();

        await database.InitialiseAsync();

        return database;
    }
}
