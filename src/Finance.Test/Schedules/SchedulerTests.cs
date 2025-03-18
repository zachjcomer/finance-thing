using Finance.Transactions.Schedules;
using Finance.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Finance.Library;

namespace Finance.Test.Schedules;

[TestClass]
public class SchedulerTests
{
    [TestMethod]
    public void GetEnumerator_WithYearlyInterval_GeneratesCorrectDates()
    {
        // Arrange
        var income = new Income("Salary", 50000m, new DateTime(2024, 1, 1));
        var scheduler = new Schedule.Builder(income)
            .WithInterval(TimeInterval.Year)
            .WithEndDate(new DateTime(2026, 1, 1))
            .Build();

        // Act
        var transactions = scheduler.ToList();

        // Assert
        Assert.AreEqual(3, transactions.Count);
        Assert.AreEqual(new DateTime(2024, 1, 1), transactions[0].Date);
        Assert.AreEqual(new DateTime(2025, 1, 1), transactions[1].Date);
        Assert.AreEqual(new DateTime(2026, 1, 1), transactions[2].Date);
    }

    [TestMethod]
    public void GetEnumerator_WithDailyInterval_GeneratesCorrectDates()
    {
        // Arrange
        var expense = new Expense("Coffee", 5m, new DateTime(2024, 1, 1));
        var scheduler = new Schedule.Builder(expense)
            .WithInterval(TimeInterval.Day)
            .WithEndDate(new DateTime(2024, 1, 3))
            .Build();

        // Act
        var transactions = scheduler.ToList();

        // Assert
        Assert.AreEqual(3, transactions.Count);
        Assert.AreEqual(new DateTime(2024, 1, 1), transactions[0].Date);
        Assert.AreEqual(new DateTime(2024, 1, 2), transactions[1].Date);
        Assert.AreEqual(new DateTime(2024, 1, 3), transactions[2].Date);
    }

    [TestMethod]
    public void GetEnumerator_PreservesTransactionType()
    {
        // Arrange
        var expense = new Expense("Rent", 1000m, new DateTime(2024, 1, 1));
        var scheduler = new Schedule.Builder(expense)
            .WithInterval(TimeInterval.Month)
            .WithEndDate(new DateTime(2024, 3, 1))
            .Build();

        // Act
        var transactions = scheduler.ToList();

        // Assert
        Assert.AreEqual(3, transactions.Count);
        Assert.IsInstanceOfType(transactions[0], typeof(Expense));
        Assert.IsInstanceOfType(transactions[1], typeof(Expense));
        Assert.IsInstanceOfType(transactions[2], typeof(Expense));
        Assert.AreEqual(-1000m, transactions[0].Amount); // Expense amounts are negative
    }

    [TestMethod]
    public void GetEnumerator_WithDefaultEndDate_GeneratesUpToTenYears()
    {
        // Arrange
        var income = new Income("Salary", 50000m, new DateTime(2024, 1, 1));
        var scheduler = new Schedule.Builder(income)
            .WithInterval(TimeInterval.Year)
            .Build(); // No end date specified, should default to 10 years

        // Act
        var transactions = scheduler.ToList();

        // Assert
        Assert.AreEqual(new DateTime(2024, 1, 1), transactions[0].Date);
        Assert.AreEqual(new DateTime(2034, 1, 1), transactions[^1].Date);
        Assert.AreEqual(11, transactions.Count); // Initial + 10 years
    }

    [TestMethod]
    public void GetEnumerator_WithDefaultEndDate_PreservesTransactionDetails()
    {
        // Arrange
        var expense = new Expense("Rent", 1000m, new DateTime(2024, 1, 1));
        var scheduler = new Schedule.Builder(expense)
            .WithInterval(TimeInterval.Month)
            .Build(); // Using default end date

        // Act & Assert
        var firstYear = scheduler.Take(12).ToList(); // Take first year
        Assert.AreEqual(12, firstYear.Count);
        foreach (var transaction in firstYear)
        {
            Assert.IsInstanceOfType(transaction, typeof(Expense));
            Assert.AreEqual("Rent", ((Expense)transaction).Name);
            Assert.AreEqual(-1000m, transaction.Amount);
        }
    }
}