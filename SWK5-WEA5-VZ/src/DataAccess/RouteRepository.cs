using NextStop.Data.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace NextStop.Data.DataAccess
{
    public class RouteRepository
    {
        private readonly DatabaseConnector _dbConnector;

        public RouteRepository(DatabaseConnector dbConnector)
        {
            _dbConnector = dbConnector;
        }

        public IEnumerable<Route> GetAll()
        {
            var routes = new List<Route>();

            using (var connection = _dbConnector.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, Name, StartLocation, EndLocation FROM Routes", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        routes.Add(new Route
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            StartLocation = reader.GetString(2),
                            EndLocation = reader.GetString(3)
                        });
                    }
                }
            }

            return routes;
        }
    }
}
