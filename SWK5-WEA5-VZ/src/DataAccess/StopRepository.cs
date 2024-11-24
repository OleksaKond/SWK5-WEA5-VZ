using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

public class StopRepository
{
    private readonly string _connectionString;

    public StopRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Stop>> GetAllAsync()
    {
        var stops = new List<Stop>();
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT * FROM Stops";
            using (var command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        stops.Add(new Stop
                        {
                            StopId = (int)reader["StopId"],
                            Name = (string)reader["Name"],
                            ShortCode = (string)reader["ShortCode"],
                            Latitude = (decimal)reader["Latitude"],
                            Longitude = (decimal)reader["Longitude"],
                            CompanyId = (int)reader["CompanyId"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            ModifiedAt = (DateTime)reader["ModifiedAt"]
                        });
                    }
                }
            }
        }
        return stops;
    }

    public async Task<Stop> GetByIdAsync(int id)
    {
        Stop stop = null;
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT * FROM Stops WHERE StopId = @StopId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StopId", id);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        stop = new Stop
                        {
                            StopId = (int)reader["StopId"],
                            Name = (string)reader["Name"],
                            ShortCode = (string)reader["ShortCode"],
                            Latitude = (decimal)reader["Latitude"],
                            Longitude = (decimal)reader["Longitude"],
                            CompanyId = (int)reader["CompanyId"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            ModifiedAt = (DateTime)reader["ModifiedAt"]
                        };
                    }
                }
            }
        }

        if (stop == null)
        {
            throw new Exception("Stop not found");
        }

        return stop;
    }

    public async Task AddAsync(Stop stop)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"INSERT INTO Stops (Name, ShortCode, Latitude, Longitude, CompanyId)
                          VALUES (@Name, @ShortCode, @Latitude, @Longitude, @CompanyId)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", stop.Name);
                command.Parameters.AddWithValue("@ShortCode", stop.ShortCode);
                command.Parameters.AddWithValue("@Latitude", stop.Latitude);
                command.Parameters.AddWithValue("@Longitude", stop.Longitude);
                command.Parameters.AddWithValue("@CompanyId", stop.CompanyId);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateAsync(Stop stop)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"UPDATE Stops 
                          SET Name = @Name, ShortCode = @ShortCode, Latitude = @Latitude, Longitude = @Longitude, CompanyId = @CompanyId 
                          WHERE StopId = @StopId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StopId", stop.StopId);
                command.Parameters.AddWithValue("@Name", stop.Name);
                command.Parameters.AddWithValue("@ShortCode", stop.ShortCode);
                command.Parameters.AddWithValue("@Latitude", stop.Latitude);
                command.Parameters.AddWithValue("@Longitude", stop.Longitude);
                command.Parameters.AddWithValue("@CompanyId", stop.CompanyId);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task DeleteAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "DELETE FROM Stops WHERE StopId = @StopId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StopId", id);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
