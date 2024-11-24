using System;
using System.Threading.Tasks;
using NextStop.Services;

namespace NextStop
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to the NextStop Scheduler!");

            // Define the connection string (update it to match your database setup)
            string connectionString = "Server=localhost,1433;Database=transport_db;User Id=sa;Password=Swk5-rocks!;TrustServerCertificate=True;";

            // Initialize repositories
            var holidayRepo = new HolidayRepository(connectionString);
            var routeRepo = new RouteRepository(connectionString);

            // Initialize the service
            var scheduleService = new ScheduleService(holidayRepo, routeRepo);

            // Call the service method to print holiday routes
            Console.WriteLine("\nFetching holiday routes...");
            await scheduleService.PrintHolidayRoutesAsync();

            Console.WriteLine("\nProcess complete. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
