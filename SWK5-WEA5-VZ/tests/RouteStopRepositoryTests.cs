using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class RouteStopRepositoryTests
{
    private readonly RouteStopRepository _repository;
    private readonly string _connectionString = "Server=localhost,1433;Database=transport_db;User Id=sa;Password=Swk5-rocks!;TrustServerCertificate=True;";

    public RouteStopRepositoryTests()
    {
        _repository = new RouteStopRepository(_connectionString);
    }

    [Fact]
    public async Task AddRouteStop_Should_Add_New_RouteStop()
    {
        // Arrange
        var routeStop = new RouteStop
        {
            RouteId = 1, // Ensure this Route exists in the database
            StopId = 1,  // Ensure this Stop exists in the database
            ScheduledTime = new TimeSpan(10, 0, 0),
            StopSequence = 1
        };

        // Act
        await _repository.AddAsync(routeStop);

        // Assert
        var routeStops = await _repository.GetAllByRouteIdAsync(1);
        Assert.Contains(routeStops, rs => rs.RouteId == 1 && rs.StopId == 1 && rs.ScheduledTime == new TimeSpan(10, 0, 0));
    }

    [Fact]
    public async Task GetAllByRouteId_Should_Return_RouteStops()
    {
        // Act
        var routeStops = await _repository.GetAllByRouteIdAsync(1); // Ensure RouteId 1 exists

        // Assert
        Assert.NotNull(routeStops);
        Assert.IsAssignableFrom<IEnumerable<RouteStop>>(routeStops);
    }

    [Fact]
    public async Task UpdateRouteStop_Should_Modify_Existing_RouteStop()
    {
        // Arrange
        var routeStop = new RouteStop
        {
            RouteId = 1, // Ensure this Route exists in the database
            StopId = 1,  // Ensure this Stop exists in the database
            ScheduledTime = new TimeSpan(10, 0, 0),
            StopSequence = 1
        };

        await _repository.AddAsync(routeStop);

        var routeStops = await _repository.GetAllByRouteIdAsync(1);
        var addedRouteStop = routeStops.FirstOrDefault(rs => rs.StopSequence == 1);

        addedRouteStop.ScheduledTime = new TimeSpan(11, 0, 0);
        addedRouteStop.StopSequence = 2;

        // Act
        await _repository.UpdateAsync(addedRouteStop);

        var updatedRouteStops = await _repository.GetAllByRouteIdAsync(1);
        var updatedRouteStop = updatedRouteStops.FirstOrDefault(rs => rs.RouteStopId == addedRouteStop.RouteStopId);

        // Assert
        Assert.Equal(new TimeSpan(11, 0, 0), updatedRouteStop.ScheduledTime);
        Assert.Equal(2, updatedRouteStop.StopSequence);
    }

    [Fact]
    public async Task DeleteRouteStop_Should_Remove_RouteStop()
    {
        // Arrange
        var routeStop = new RouteStop
        {
            RouteId = 1, // Ensure this Route exists in the database
            StopId = 1,  // Ensure this Stop exists in the database
            ScheduledTime = new TimeSpan(10, 0, 0),
            StopSequence = 1
        };

        await _repository.AddAsync(routeStop);

        var routeStops = await _repository.GetAllByRouteIdAsync(1);
        var addedRouteStop = routeStops.FirstOrDefault(rs => rs.StopSequence == 1);

        // Act
        await _repository.DeleteAsync(addedRouteStop.RouteStopId);

        var routeStopsAfterDelete = await _repository.GetAllByRouteIdAsync(1);

        // Assert
        Assert.DoesNotContain(routeStopsAfterDelete, rs => rs.RouteStopId == addedRouteStop.RouteStopId);
    }
}
