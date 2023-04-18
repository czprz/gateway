namespace Gateway.Routing.Storage.Rational;

public static class StorageConnectionString
{
    private static string? _connectionString;

    public static string? Get()
    {
        return _connectionString;
    }
    
    public static void Set(string connectionString)
    {
        _connectionString = connectionString;
    }
}