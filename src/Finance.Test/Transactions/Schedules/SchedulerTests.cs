using Finance.Transactions.Schedules;
using Finance.Transactions;
using Finance.Library;

namespace Finance.Test.Transactions.Schedules;

[TestClass]
public class SchedulerTests
{
    [TestMethod]
    public void GetEnumerator_WithYearlyInterval_GeneratesCorrectDates()
    {
        ITransaction income = new Income("Salary", 50000m, new(2024, 1, 1));
        IEnumerable<ITransaction> scheduler = new Schedule.Builder(income)
            .WithInterval(TimeInterval.Year)
            .WithEndDate(new(2026, 1, 1))
            .Build();
        List<ITransaction> transactions = scheduler.ToList();

        Assert.AreEqual(3, transactions.Count);
        Assert.AreEqual(new DateTime(2024, 1, 1), transactions[0].Date);
        Assert.AreEqual(new DateTime(2025, 1, 1), transactions[1].Date);
        Assert.AreEqual(new DateTime(2026, 1, 1), transactions[2].Date);
    }

    [TestMethod]
    public void GetEnumerator_WithDailyInterval_GeneratesCorrectDates()
    {
        ITransaction expense = new Expense("Coffee", 5m, new(2024, 1, 1));
        IEnumerable<ITransaction> scheduler = new Schedule.Builder(expense)
            .WithInterval(TimeInterval.Day)
            .WithEndDate(new(2024, 1, 3))
            .Build();
        List<ITransaction> transactions = scheduler.ToList();

        Assert.AreEqual(3, transactions.Count);
        Assert.AreEqual(new DateTime(2024, 1, 1), transactions[0].Date);
        Assert.AreEqual(new DateTime(2024, 1, 2), transactions[1].Date);
        Assert.AreEqual(new DateTime(2024, 1, 3), transactions[2].Date);
    }

    [TestMethod]
    public void GetEnumerator_PreservesTransactionType()
    {
        ITransaction expense = new Expense("Rent", 1000m, new(2024, 1, 1));
        IEnumerable<ITransaction> scheduler = new Schedule.Builder(expense)
            .WithInterval(TimeInterval.Month)
            .WithEndDate(new(2024, 3, 1))
            .Build();
        List<ITransaction> transactions = scheduler.ToList();

        Assert.AreEqual(3, transactions.Count);
        Assert.IsInstanceOfType(transactions[0], typeof(Expense));
        Assert.IsInstanceOfType(transactions[1], typeof(Expense));
        Assert.IsInstanceOfType(transactions[2], typeof(Expense));
        Assert.AreEqual(-1000m, transactions[0].Amount);
    }

    [TestMethod]
    public void GetEnumerator_WithDefaultEndDate_GeneratesUpToTenYears()
    {
        ITransaction income = new Income("Salary", 50000m, new(2024, 1, 1));
        IEnumerable<ITransaction> scheduler = new Schedule.Builder(income)
            .WithInterval(TimeInterval.Year)
            .Build();
        List<ITransaction> transactions = scheduler.ToList();

        Assert.AreEqual(new DateTime(2024, 1, 1), transactions[0].Date);
        Assert.AreEqual(new DateTime(2034, 1, 1), transactions[^1].Date);
        Assert.AreEqual(11, transactions.Count);
    }

    [TestMethod]
    public void GetEnumerator_WithDefaultEndDate_PreservesTransactionDetails()
    {
        ITransaction expense = new Expense("Rent", 1000m, new(2024, 1, 1));
        IEnumerable<ITransaction> scheduler = new Schedule.Builder(expense)
            .WithInterval(TimeInterval.Month)
            .Build();
        List<ITransaction> firstYear = scheduler.Take(12).ToList();

        Assert.AreEqual(12, firstYear.Count);
        foreach (ITransaction transaction in firstYear)
        {
            Assert.IsInstanceOfType(transaction, typeof(Expense));
            Assert.AreEqual("Rent", ((Expense)transaction).Name);
            Assert.AreEqual(-1000m, transaction.Amount);
        }
    }
}