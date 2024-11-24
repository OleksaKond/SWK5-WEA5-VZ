using NextStop.Data.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace NextStop.Data.DataAccess
{
    public class StopRepository
    {
        private readonly DatabaseConnector _dbConnector;

        public StopRepository(DatabaseConnector dbConnector)
        {
            _dbConnector = dbConnector;
        }

        public IEnumerable<Stop> GetAll()
        {
            var stops = new List<Stop>();

            using (var connection = _dbConnector.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, LocationName, RouteId FROM Stops", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stops.Add(new Stop
                        {
                            Id = reader.GetInt32(0),
                            LocationName = reader.GetString(1),
                            RouteId = reader.GetInt32(2)
                        });
                    }
                }
            }

            return stops;
        }
    }
}
