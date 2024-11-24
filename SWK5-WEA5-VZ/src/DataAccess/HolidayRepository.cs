using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace NextStop.Data.DataAccess
{
    public class HolidayRepository
    {
        private readonly DatabaseConnector _dbConnector;

        public HolidayRepository(DatabaseConnector dbConnector)
        {
            _dbConnector = dbConnector;
        }

        public IEnumerable<Holiday> GetAll()
        {
            var holidays = new List<Holiday>();

            using (var connection = _dbConnector.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, Name, Date FROM Holidays", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        holidays.Add(new Holiday
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Date = reader.GetDateTime(2)
                        });
                    }
                }
            }

            return holidays;
        }
    }
}
