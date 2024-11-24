using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class HolidayRepositoryTests
{
    private readonly HolidayRepository _repository;
    private readonly string _connectionString = "Server=localhost,1433;Database=transport_db;User Id=sa;Password=Swk5-rocks!;TrustServerCertificate=True;";

    public HolidayRepositoryTests()
    {
        _repository = new HolidayRepository(_connectionString);
    }

    [Fact]
    public async Task AddHoliday_Should_Add_Holiday()
    {
        // Arrange
        var holiday = new Holiday
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1),
            Description = "Test Holiday",
            IsSchoolBreak = false
        };

        // Act
        await _repository.AddAsync(holiday);

        // Assert
        var holidays = await _repository.GetAllAsync();
        Assert.Contains(holidays, h => h.Description == "Test Holiday" && h.StartDate == holiday.StartDate);
    }

    [Fact]
    public async Task GetAllHolidays_Should_Return_Holidays()
    {
        // Act
        var holidays = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(holidays);
        Assert.IsAssignableFrom<IEnumerable<Holiday>>(holidays);
    }

    [Fact]
    public async Task GetHolidayById_Should_Return_Correct_Holiday()
    {
        // Arrange
        var holiday = new Holiday
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1),
            Description = "Test Holiday By ID",
            IsSchoolBreak = true
        };

        await _repository.AddAsync(holiday);
        var holidays = await _repository.GetAllAsync();
        var addedHoliday = holidays.First(h => h.Description == "Test Holiday By ID");

        // Act
        var fetchedHoliday = await _repository.GetByIdAsync(addedHoliday.HolidayId);

        // Assert
        Assert.NotNull(fetchedHoliday);
        Assert.Equal(addedHoliday.HolidayId, fetchedHoliday.HolidayId);
        Assert.Equal(holiday.Description, fetchedHoliday.Description);
    }

    [Fact]
    public async Task UpdateHoliday_Should_Modify_Existing_Holiday()
    {
        // Arrange
        var holiday = new Holiday
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1),
            Description = "Original Description",
            IsSchoolBreak = false
        };

        await _repository.AddAsync(holiday);
        var holidays = await _repository.GetAllAsync();
        var addedHoliday = holidays.First(h => h.Description == "Original Description");

        // Modify the holiday
        addedHoliday.Description = "Updated Description";
        addedHoliday.IsSchoolBreak = true;

        // Act
        await _repository.UpdateAsync(addedHoliday);
        var updatedHoliday = await _repository.GetByIdAsync(addedHoliday.HolidayId);

        // Assert
        Assert.Equal("Updated Description", updatedHoliday.Description);
        Assert.True(updatedHoliday.IsSchoolBreak);
    }

    [Fact]
    public async Task DeleteHoliday_Should_Remove_Holiday()
    {
        // Arrange
        var holiday = new Holiday
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1),
            Description = "Holiday To Delete",
            IsSchoolBreak = false
        };

        await _repository.AddAsync(holiday);
        var holidays = await _repository.GetAllAsync();
        var addedHoliday = holidays.First(h => h.Description == "Holiday To Delete");

        // Act
        await _repository.DeleteAsync(addedHoliday.HolidayId);
        var allHolidaysAfterDelete = await _repository.GetAllAsync();

        // Assert
        Assert.DoesNotContain(allHolidaysAfterDelete, h => h.HolidayId == addedHoliday.HolidayId);
    }
}
