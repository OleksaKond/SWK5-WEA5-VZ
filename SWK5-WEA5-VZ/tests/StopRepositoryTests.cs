using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class StopRepositoryTests
{
    private readonly StopRepository _repository;
    private readonly string _connectionString = "Server=localhost,1433;Database=transport_db;User Id=sa;Password=Swk5-rocks!;TrustServerCertificate=True;";

    public StopRepositoryTests()
    {
        _repository = new StopRepository(_connectionString);
    }

    [Fact]
    public async Task AddStop_Should_Add_New_Stop()
    {
        // Arrange
        var stop = new Stop
        {
            Name = "Test Stop",
            ShortCode = "TSTOP",
            Latitude = 48.3683M,
            Longitude = 14.5155M,
            CompanyId = 1 // Ensure this company exists in the database
        };

        // Act
        await _repository.AddAsync(stop);

        // Assert
        var stops = await _repository.GetAllAsync();
        Assert.Contains(stops, s => s.Name == "Test Stop" && s.ShortCode == "TSTOP");
    }

    [Fact]
    public async Task GetAllStops_Should_Return_All_Stops()
    {
        // Act
        var stops = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(stops);
        Assert.IsAssignableFrom<IEnumerable<Stop>>(stops);
    }

    [Fact]
    public async Task GetStopById_Should_Return_Correct_Stop()
    {
        // Arrange
        var stop = new Stop
        {
            Name = "Specific Stop",
            ShortCode = "SPEC",
            Latitude = 48.3683M,
            Longitude = 14.5155M,
            CompanyId = 1 // Ensure this company exists in the database
        };

        await _repository.AddAsync(stop);
        var stops = await _repository.GetAllAsync();
        var addedStop = stops.FirstOrDefault(s => s.ShortCode == "SPEC");

        // Act
        var retrievedStop = await _repository.GetByIdAsync(addedStop.StopId);

        // Assert
        Assert.NotNull(retrievedStop);
        Assert.Equal("Specific Stop", retrievedStop.Name);
        Assert.Equal("SPEC", retrievedStop.ShortCode);
    }

    [Fact]
    public async Task UpdateStop_Should_Modify_Existing_Stop()
    {
        // Arrange
        var stop = new Stop
        {
            Name = "Old Stop",
            ShortCode = "OLDSTP",
            Latitude = 48.0000M,
            Longitude = 14.0000M,
            CompanyId = 1 // Ensure this company exists in the database
        };

        await _repository.AddAsync(stop);
        var stops = await _repository.GetAllAsync();
        var addedStop = stops.FirstOrDefault(s => s.ShortCode == "OLDSTP");

        addedStop.Name = "Updated Stop";
        addedStop.ShortCode = "UPDSTP";

        // Act
        await _repository.UpdateAsync(addedStop);
        var updatedStop = await _repository.GetByIdAsync(addedStop.StopId);

        // Assert
        Assert.Equal("Updated Stop", updatedStop.Name);
        Assert.Equal("UPDSTP", updatedStop.ShortCode);
    }

    [Fact]
    public async Task DeleteStop_Should_Remove_Stop()
    {
        // Arrange
        var stop = new Stop
        {
            Name = "Temporary Stop",
            ShortCode = "TEMP",
            Latitude = 47.0000M,
            Longitude = 13.0000M,
            CompanyId = 1 // Ensure this company exists in the database
        };

        await _repository.AddAsync(stop);
        var stops = await _repository.GetAllAsync();
        var addedStop = stops.FirstOrDefault(s => s.ShortCode == "TEMP");

        // Act
        await _repository.DeleteAsync(addedStop.StopId);
        var allStopsAfterDelete = await _repository.GetAllAsync();

        // Assert
        Assert.DoesNotContain(allStopsAfterDelete, s => s.StopId == addedStop.StopId);
    }
}
