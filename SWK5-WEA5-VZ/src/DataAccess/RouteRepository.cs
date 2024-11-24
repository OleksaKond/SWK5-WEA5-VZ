using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

public class RouteRepository
{
    private readonly string _connectionString;

    public RouteRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Route>> GetAllAsync()
    {
        var routes = new List<Route>();
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT * FROM Routes";
            using (var command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        routes.Add(new Route
                        {
                            RouteId = (int)reader["RouteId"],
                            RouteNumber = (string)reader["RouteNumber"],
                            CompanyId = (int)reader["CompanyId"],
                            ValidFromDate = (DateTime)reader["ValidFromDate"],
                            ValidUntilDate = (DateTime)reader["ValidUntilDate"],
                            OperatesOnWeekdays = (bool)reader["OperatesOnWeekdays"],
                            OperatesOnWeekends = (bool)reader["OperatesOnWeekends"],
                            OperatesOnHolidays = (bool)reader["OperatesOnHolidays"],
                            OperatesDuringSchoolBreaks = (bool)reader["OperatesDuringSchoolBreaks"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            ModifiedAt = (DateTime)reader["ModifiedAt"]
                        });
                    }
                }
            }
        }
        return routes;
    }

    public async Task<Route> GetByIdAsync(int id)
    {
        Route route = null;
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT * FROM Routes WHERE RouteId = @RouteId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RouteId", id);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        route = new Route
                        {
                            RouteId = (int)reader["RouteId"],
                            RouteNumber = (string)reader["RouteNumber"],
                            CompanyId = (int)reader["CompanyId"],
                            ValidFromDate = (DateTime)reader["ValidFromDate"],
                            ValidUntilDate = (DateTime)reader["ValidUntilDate"],
                            OperatesOnWeekdays = (bool)reader["OperatesOnWeekdays"],
                            OperatesOnWeekends = (bool)reader["OperatesOnWeekends"],
                            OperatesOnHolidays = (bool)reader["OperatesOnHolidays"],
                            OperatesDuringSchoolBreaks = (bool)reader["OperatesDuringSchoolBreaks"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            ModifiedAt = (DateTime)reader["ModifiedAt"]
                        };
                    }
                }
            }
        }

        if (route == null)
        {
            throw new Exception("Route not found");
        }

        return route;
    }

    public async Task AddAsync(Route route)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"INSERT INTO Routes (RouteNumber, CompanyId, ValidFromDate, ValidUntilDate, OperatesOnWeekdays, OperatesOnWeekends, OperatesOnHolidays, OperatesDuringSchoolBreaks)
                          VALUES (@RouteNumber, @CompanyId, @ValidFromDate, @ValidUntilDate, @OperatesOnWeekdays, @OperatesOnWeekends, @OperatesOnHolidays, @OperatesDuringSchoolBreaks)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RouteNumber", route.RouteNumber);
                command.Parameters.AddWithValue("@CompanyId", route.CompanyId);
                command.Parameters.AddWithValue("@ValidFromDate", route.ValidFromDate);
                command.Parameters.AddWithValue("@ValidUntilDate", route.ValidUntilDate);
                command.Parameters.AddWithValue("@OperatesOnWeekdays", route.OperatesOnWeekdays);
                command.Parameters.AddWithValue("@OperatesOnWeekends", route.OperatesOnWeekends);
                command.Parameters.AddWithValue("@OperatesOnHolidays", route.OperatesOnHolidays);
                command.Parameters.AddWithValue("@OperatesDuringSchoolBreaks", route.OperatesDuringSchoolBreaks);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateAsync(Route route)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"UPDATE Routes 
                          SET RouteNumber = @RouteNumber, CompanyId = @CompanyId, ValidFromDate = @ValidFromDate, ValidUntilDate = @ValidUntilDate, 
                              OperatesOnWeekdays = @OperatesOnWeekdays, OperatesOnWeekends = @OperatesOnWeekends, OperatesOnHolidays = @OperatesOnHolidays, 
                              OperatesDuringSchoolBreaks = @OperatesDuringSchoolBreaks
                          WHERE RouteId = @RouteId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RouteId", route.RouteId);
                command.Parameters.AddWithValue("@RouteNumber", route.RouteNumber);
                command.Parameters.AddWithValue("@CompanyId", route.CompanyId);
                command.Parameters.AddWithValue("@ValidFromDate", route.ValidFromDate);
                command.Parameters.AddWithValue("@ValidUntilDate", route.ValidUntilDate);
                command.Parameters.AddWithValue("@OperatesOnWeekdays", route.OperatesOnWeekdays);
                command.Parameters.AddWithValue("@OperatesOnWeekends", route.OperatesOnWeekends);
                command.Parameters.AddWithValue("@OperatesOnHolidays", route.OperatesOnHolidays);
                command.Parameters.AddWithValue("@OperatesDuringSchoolBreaks", route.OperatesDuringSchoolBreaks);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task DeleteAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "DELETE FROM Routes WHERE RouteId = @RouteId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RouteId", id);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
