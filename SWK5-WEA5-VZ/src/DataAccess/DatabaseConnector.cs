using Microsoft.Data.SqlClient;

public class DatabaseConnector
{
    private readonly string _connectionString;

    public DatabaseConnector(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
