using Microsoft.Data.SqlClient; // Use System.Data.SqlClient if that's your package
using System;

class Program
{
    static void Main()
    {
        string connectionString = "Server=localhost,1433;Database=transport_db;User Id=sa;Password=Swk5-rocks!;TrustServerCertificate=True;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connection to the database was successful!");

                // Example query
                string query = "SELECT * FROM Holidays";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Holiday: {reader["Name"]}, Date: {reader["Date"]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
