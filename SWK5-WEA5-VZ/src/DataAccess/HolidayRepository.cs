using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

public class HolidayRepository
{
    private readonly string _connectionString;

    public HolidayRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Create
    public async Task AddAsync(Holiday holiday)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"INSERT INTO Holidays (StartDate, EndDate, Description, IsSchoolBreak)
                          VALUES (@StartDate, @EndDate, @Description, @IsSchoolBreak)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StartDate", holiday.StartDate);
                command.Parameters.AddWithValue("@EndDate", holiday.EndDate);
                command.Parameters.AddWithValue("@Description", holiday.Description);
                command.Parameters.AddWithValue("@IsSchoolBreak", holiday.IsSchoolBreak);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    // Read All
    public async Task<IEnumerable<Holiday>> GetAllAsync()
    {
        var holidays = new List<Holiday>();
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT * FROM Holidays";
            using (var command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        holidays.Add(new Holiday
                        {
                            HolidayId = (int)reader["HolidayId"],
                            StartDate = (DateTime)reader["StartDate"],
                            EndDate = (DateTime)reader["EndDate"],
                            Description = (string)reader["Description"],
                            IsSchoolBreak = (bool)reader["IsSchoolBreak"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            ModifiedAt = (DateTime)reader["ModifiedAt"]
                        });
                    }
                }
            }
        }
        return holidays;
    }

    // Read by ID
    public async Task<Holiday> GetByIdAsync(int id)
    {
        Holiday holiday = null; // Use a default value
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT * FROM Holidays WHERE HolidayId = @HolidayId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@HolidayId", id);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        holiday = new Holiday
                        {
                            HolidayId = (int)reader["HolidayId"],
                            StartDate = (DateTime)reader["StartDate"],
                            EndDate = (DateTime)reader["EndDate"],
                            Description = (string)reader["Description"],
                            IsSchoolBreak = (bool)reader["IsSchoolBreak"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            ModifiedAt = (DateTime)reader["ModifiedAt"]
                        };
                    }
                }
            }
        }

        if (holiday == null)
        {
            throw new Exception("Holiday not found");
        }

        return holiday;
    }


    // Update
    public async Task UpdateAsync(Holiday holiday)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"UPDATE Holidays
                          SET StartDate = @StartDate,
                              EndDate = @EndDate,
                              Description = @Description,
                              IsSchoolBreak = @IsSchoolBreak,
                              ModifiedAt = GETDATE()
                          WHERE HolidayId = @HolidayId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@HolidayId", holiday.HolidayId);
                command.Parameters.AddWithValue("@StartDate", holiday.StartDate);
                command.Parameters.AddWithValue("@EndDate", holiday.EndDate);
                command.Parameters.AddWithValue("@Description", holiday.Description);
                command.Parameters.AddWithValue("@IsSchoolBreak", holiday.IsSchoolBreak);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    // Delete
    public async Task DeleteAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "DELETE FROM Holidays WHERE HolidayId = @HolidayId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@HolidayId", id);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
