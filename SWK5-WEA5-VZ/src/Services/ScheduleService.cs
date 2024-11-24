using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextStop.Services
{
    public class ScheduleService
    {
        private readonly HolidayRepository _holidayRepo;
        private readonly RouteRepository _routeRepo;

        public ScheduleService(HolidayRepository holidayRepo, RouteRepository routeRepo)
        {
            _holidayRepo = holidayRepo;
            _routeRepo = routeRepo;
        }

        public async Task PrintHolidayRoutesAsync()
        {
            try
            {
                var holidaysTask = _holidayRepo.GetAllAsync();
                var routesTask = _routeRepo.GetAllAsync();

                var holidays = await holidaysTask;
                var routes = await routesTask;

                if (!holidays.Any())
                {
                    Console.WriteLine("No holidays found.");
                    return;
                }

                if (!routes.Any())
                {
                    Console.WriteLine("No routes found.");
                    return;
                }

                foreach (var holiday in holidays)
                {
                    Console.WriteLine($"Holiday: {holiday.Description} ({holiday.StartDate:yyyy-MM-dd} to {holiday.EndDate:yyyy-MM-dd})");
                    var holidayRoutes = routes.Where(route => route.OperatesOnHolidays).ToList();

                    if (!holidayRoutes.Any())
                    {
                        Console.WriteLine("No routes operate on holidays.");
                    }
                    else
                    {
                        foreach (var route in holidayRoutes)
                        {
                            Console.WriteLine($"  Route {route.RouteNumber} operates during holidays.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
