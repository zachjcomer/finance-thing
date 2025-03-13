using Finance.Schedules;
using Finance.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Finance.Test.Schedules;

[TestClass]
public class SchedulerBuilderTests
{
    [TestMethod]
    public void Builder_WithValidInput_CreatesScheduler()
    {
        // Arrange
        var income = new Income("Test", 100m, new DateTime(2024, 1, 1));
        
        // Act
        var scheduler = new Schedule.Builder(income)
            .WithInterval(TimeInterval.Month)
            .WithEndDate(new DateTime(2024, 12, 31))
            .Build();

        // Assert
        Assert.IsNotNull(scheduler);
    }

    [TestMethod]
    public void Builder_WithDefaultValues_UsesMonthlyIntervalAndTenYearEnd()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1);
        var income = new Income("Test", 100m, startDate);
        
        // Act
        var scheduler = new Schedule.Builder(income).Build();
        var transactions = scheduler.ToList();

        // Assert
        var expectedEndDate = startDate.AddYears(10);
        Assert.AreEqual(121, transactions.Count); // 10 years * 12 months + initial transaction
        Assert.AreEqual(startDate, transactions[0].Date);
        Assert.AreEqual(expectedEndDate, transactions[^1].Date);
    }

    [TestMethod]
    public void Builder_WithoutInterval_UsesMonthlyDefault()
    {
        // Arrange
        var income = new Income("Test", 100m, new DateTime(2024, 1, 1));
        var scheduler = new Schedule.Builder(income)
            .WithEndDate(new DateTime(2024, 3, 1))
            .Build();

        // Act
        var transactions = scheduler.ToList();

        // Assert
        Assert.AreEqual(3, transactions.Count); // Jan, Feb, Mar
        Assert.AreEqual(new DateTime(2024, 1, 1), transactions[0].Date);
        Assert.AreEqual(new DateTime(2024, 2, 1), transactions[1].Date);
        Assert.AreEqual(new DateTime(2024, 3, 1), transactions[2].Date);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Builder_WithDuplicateInterval_ThrowsException()
    {
        // Arrange
        var income = new Income("Test", 100m, new DateTime(2024, 1, 1));
        
        // Act
        new Schedule.Builder(income)
            .WithInterval(TimeInterval.Month)
            .WithInterval(TimeInterval.Year); // Should throw
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Builder_WithEndDateBeforeStartDate_ThrowsException()
    {
        // Arrange
        var income = new Income("Test", 100m, new DateTime(2024, 1, 1));
        
        // Act
        new Schedule.Builder(income)
            .WithEndDate(new DateTime(2023, 12, 31)); // Should throw
    }
} 