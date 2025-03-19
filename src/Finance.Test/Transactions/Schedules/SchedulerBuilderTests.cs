using Finance.Transactions.Schedules;
using Finance.Transactions;
using Finance.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Finance.Test.Transactions.Schedules;

[TestClass]
public class SchedulerBuilderTests
{
    [TestMethod]
    public void Builder_WithValidInput_CreatesScheduler()
    {
        ITransaction income = new Income("Test", 100m, new(2024, 1, 1));
        IEnumerable<ITransaction> scheduler = new Schedule.Builder(income)
            .WithInterval(TimeInterval.Month)
            .WithEndDate(new(2024, 12, 31))
            .Build();

        Assert.IsNotNull(scheduler);
    }

    [TestMethod]
    public void Builder_Name_SetsName()
    {
        ITransaction income = new Income("Test", 100m, new(2024, 1, 1));
        Schedule scheduler = new Schedule.Builder(income)
            .Name("Test Schedule")
            .Build();

        Assert.AreEqual("Test Schedule", scheduler.Name);
    }

    [TestMethod]
    public void Builder_WithDefaultValues_UsesMonthlyIntervalAndTenYearEnd()
    {
        DateTime startDate = new(2024, 1, 1);
        ITransaction income = new Income("Test", 100m, startDate);
        IEnumerable<ITransaction> scheduler = new Schedule.Builder(income).Build();
        List<ITransaction> transactions = scheduler.ToList();

        DateTime expectedEndDate = startDate.AddYears(10);
        Assert.AreEqual(121, transactions.Count); // 10 years * 12 months + initial transaction
        Assert.AreEqual(startDate, transactions[0].Date);
        Assert.AreEqual(expectedEndDate, transactions[^1].Date);
    }

    [TestMethod]
    public void Builder_WithoutInterval_UsesMonthlyDefault()
    {
        ITransaction income = new Income("Test", 100m, new(2024, 1, 1));
        IEnumerable<ITransaction> scheduler = new Schedule.Builder(income)
            .WithEndDate(new(2024, 3, 1))
            .Build();
        List<ITransaction> transactions = scheduler.ToList();

        Assert.AreEqual(3, transactions.Count); // Jan, Feb, Mar
        Assert.AreEqual(new DateTime(2024, 1, 1), transactions[0].Date);
        Assert.AreEqual(new DateTime(2024, 2, 1), transactions[1].Date);
        Assert.AreEqual(new DateTime(2024, 3, 1), transactions[2].Date);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Builder_WithDuplicateInterval_ThrowsException()
    {
        ITransaction income = new Income("Test", 100m, new(2024, 1, 1));
        new Schedule.Builder(income)
            .WithInterval(TimeInterval.Month)
            .WithInterval(TimeInterval.Year); // Should throw
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Builder_WithInvalidInterval_ThrowsException()
    {
        ITransaction income = new Income("Test", 100m, new(2024, 1, 1));
        new Schedule.Builder(income)
            .WithInterval((TimeInterval)999); // Should throw
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Builder_WithEndDateAlreadySet_ThrowsException()
    {
        ITransaction income = new Income("Test", 100m, new(2024, 1, 1));
        new Schedule.Builder(income)
            .WithEndDate(new(2024, 1, 1))
            .WithEndDate(new(2024, 1, 1)); // Should throw
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Builder_WithEndDateBeforeStartDate_ThrowsException()
    {
        ITransaction income = new Income("Test", 100m, new(2024, 1, 1));
        new Schedule.Builder(income)
            .WithEndDate(new(2023, 12, 31)); // Should throw
    }
} 