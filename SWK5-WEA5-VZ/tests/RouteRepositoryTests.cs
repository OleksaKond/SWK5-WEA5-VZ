using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class RouteRepositoryTests
{
    private readonly RouteRepository _repository;
    private readonly string _connectionString = "Server=localhost,1433;Database=transport_db;User Id=sa;Password=Swk5-rocks!;TrustServerCertificate=True;";

    public RouteRepositoryTests()
    {
        _repository = new RouteRepository(_connectionString);
    }

    [Fact]
    public async Task AddRoute_Should_Add_New_Route()
    {
        // Arrange
        var route = new Route
        {
            RouteNumber = "123A",
            CompanyId = 1, // Ensure this CompanyId exists in the database
            ValidFromDate = DateTime.Today,
            ValidUntilDate = DateTime.Today.AddYears(1),
            OperatesOnWeekdays = true,
            OperatesOnWeekends = false,
            OperatesOnHolidays = true,
            OperatesDuringSchoolBreaks = true
        };

        // Act
        await _repository.AddAsync(route);

        // Assert
        var routes = await _repository.GetAllAsync();
        Assert.Contains(routes, r => r.RouteNumber == "123A" && r.CompanyId == 1);
    }

    [Fact]
    public async Task GetAllRoutes_Should_Return_All_Routes()
    {
        // Act
        var routes = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(routes);
        Assert.IsAssignableFrom<IEnumerable<Route>>(routes);
    }

    [Fact]
    public async Task GetRouteById_Should_Return_Specific_Route()
    {
        // Arrange
        var route = new Route
        {
            RouteNumber = "456B",
            CompanyId = 1, // Ensure this CompanyId exists in the database
            ValidFromDate = DateTime.Today,
            ValidUntilDate = DateTime.Today.AddYears(1),
            OperatesOnWeekdays = true,
            OperatesOnWeekends = false,
            OperatesOnHolidays = true,
            OperatesDuringSchoolBreaks = false
        };

        await _repository.AddAsync(route);
        var routes = await _repository.GetAllAsync();
        var addedRoute = routes.FirstOrDefault(r => r.RouteNumber == "456B");

        // Act
        var fetchedRoute = await _repository.GetByIdAsync(addedRoute.RouteId);

        // Assert
        Assert.NotNull(fetchedRoute);
        Assert.Equal("456B", fetchedRoute.RouteNumber);
    }

    [Fact]
    public async Task UpdateRoute_Should_Modify_Existing_Route()
    {
        // Arrange
        var route = new Route
        {
            RouteNumber = "789C",
            CompanyId = 1, // Ensure this CompanyId exists in the database
            ValidFromDate = DateTime.Today,
            ValidUntilDate = DateTime.Today.AddYears(1),
            OperatesOnWeekdays = true,
            OperatesOnWeekends = false,
            OperatesOnHolidays = false,
            OperatesDuringSchoolBreaks = false
        };

        await _repository.AddAsync(route);
        var routes = await _repository.GetAllAsync();
        var addedRoute = routes.FirstOrDefault(r => r.RouteNumber == "789C");

        addedRoute.OperatesOnWeekends = true;

        // Act
        await _repository.UpdateAsync(addedRoute);
        var updatedRoute = await _repository.GetByIdAsync(addedRoute.RouteId);

        // Assert
        Assert.True(updatedRoute.OperatesOnWeekends);
    }

    [Fact]
    public async Task DeleteRoute_Should_Remove_Route()
    {
        // Arrange
        var route = new Route
        {
            RouteNumber = "101D",
            CompanyId = 1, // Ensure this CompanyId exists in the database
            ValidFromDate = DateTime.Today,
            ValidUntilDate = DateTime.Today.AddYears(1),
            OperatesOnWeekdays = true,
            OperatesOnWeekends = false,
            OperatesOnHolidays = false,
            OperatesDuringSchoolBreaks = false
        };

        await _repository.AddAsync(route);
        var routes = await _repository.GetAllAsync();
        var addedRoute = routes.FirstOrDefault(r => r.RouteNumber == "101D");

        // Act
        await _repository.DeleteAsync(addedRoute.RouteId);
        var routesAfterDelete = await _repository.GetAllAsync();

        // Assert
        Assert.DoesNotContain(routesAfterDelete, r => r.RouteId == addedRoute.RouteId);
    }
}
