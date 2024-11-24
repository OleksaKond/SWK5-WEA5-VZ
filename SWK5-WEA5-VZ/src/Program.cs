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

            string connectionString = "Server=localhost,1433;Database=transport_db;User Id=sa;Password=Swk5-rocks!;TrustServerCertificate=True;";
           
            var holidayRepo = new HolidayRepository(connectionString);
            var routeRepo = new RouteRepository(connectionString);

            var scheduleService = new ScheduleService(holidayRepo, routeRepo);

            Console.WriteLine("\nFetching holiday routes...");
            await scheduleService.PrintHolidayRoutesAsync();

            Console.WriteLine("\nProcess complete. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
