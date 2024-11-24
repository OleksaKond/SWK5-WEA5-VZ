using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

public class RouteStopRepository
{
    private readonly string _connectionString;

    public RouteStopRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<RouteStop>> GetAllByRouteIdAsync(int routeId)
    {
        var routeStops = new List<RouteStop>();
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT * FROM RouteStops WHERE RouteId = @RouteId ORDER BY StopSequence";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RouteId", routeId);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        routeStops.Add(new RouteStop
                        {
                            RouteStopId = (int)reader["RouteStopId"],
                            RouteId = (int)reader["RouteId"],
                            StopId = (int)reader["StopId"],
                            ScheduledTime = (TimeSpan)reader["ScheduledTime"],
                            StopSequence = (int)reader["StopSequence"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            ModifiedAt = (DateTime)reader["ModifiedAt"]
                        });
                    }
                }
            }
        }
        return routeStops;
    }

    public async Task AddAsync(RouteStop routeStop)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"INSERT INTO RouteStops (RouteId, StopId, ScheduledTime, StopSequence)
                          VALUES (@RouteId, @StopId, @ScheduledTime, @StopSequence)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RouteId", routeStop.RouteId);
                command.Parameters.AddWithValue("@StopId", routeStop.StopId);
                command.Parameters.AddWithValue("@ScheduledTime", routeStop.ScheduledTime);
                command.Parameters.AddWithValue("@StopSequence", routeStop.StopSequence);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateAsync(RouteStop routeStop)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"UPDATE RouteStops 
                          SET StopId = @StopId, ScheduledTime = @ScheduledTime, StopSequence = @StopSequence 
                          WHERE RouteStopId = @RouteStopId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RouteStopId", routeStop.RouteStopId);
                command.Parameters.AddWithValue("@StopId", routeStop.StopId);
                command.Parameters.AddWithValue("@ScheduledTime", routeStop.ScheduledTime);
                command.Parameters.AddWithValue("@StopSequence", routeStop.StopSequence);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task DeleteAsync(int routeStopId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "DELETE FROM RouteStops WHERE RouteStopId = @RouteStopId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RouteStopId", routeStopId);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
