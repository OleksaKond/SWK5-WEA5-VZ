using NextStop.Data.DataAccess;
using System;

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

        public void PrintHolidayRoutes()
        {
            var holidays = _holidayRepo.GetAll();
            var routes = _routeRepo.GetAll();

            foreach (var holiday in holidays)
            {
                Console.WriteLine($"Holiday: {holiday.Name} on {holiday.Date}");
                foreach (var route in routes)
                {
                    Console.WriteLine($"Route: {route.Name} ({route.StartLocation} -> {route.EndLocation})");
                }
            }
        }
    }
}
