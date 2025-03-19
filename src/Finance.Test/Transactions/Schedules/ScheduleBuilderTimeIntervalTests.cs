using Finance.Transactions.Schedules;
using Finance.Transactions;
using Finance.Library;

namespace Finance.Test.Transactions.Schedules;

[TestClass]
public class ScheduleBuilderTimeIntervalTests
{
    [TestMethod]
    public void Daily_WithValidTransaction_SetsDailyInterval()
    {
        ITransaction income = new Income("Test", 100m, new(2024, 1, 1));
        Schedule.Builder builder = new(income);
        
        IEnumerable<ITransaction> scheduler = builder.Daily().Build();
        List<ITransaction> transactions = scheduler.Take(3).ToList();

        Assert.AreEqual(new DateTime(2024, 1, 1), transactions[0].Date);
        Assert.AreEqual(new DateTime(2024, 1, 2), transactions[1].Date);
        Assert.AreEqual(new DateTime(2024, 1, 3), transactions[2].Date);
    }

    [TestMethod]
    public void Weekly_WithValidTransaction_SetsWeeklyInterval()
    {
        ITransaction income = new Income("Test", 100m, new(2024, 1, 1));
        Schedule.Builder builder = new(income);
        
        IEnumerable<ITransaction> scheduler = builder.Weekly().Build();
        List<ITransaction> transactions = scheduler.Take(3).ToList();

        Assert.AreEqual(new DateTime(2024, 1, 1), transactions[0].Date);
        Assert.AreEqual(new DateTime(2024, 1, 8), transactions[1].Date);
        Assert.AreEqual(new DateTime(2024, 1, 15), transactions[2].Date);
    }

    [TestMethod]
    public void Monthly_WithValidTransaction_SetsMonthlyInterval()
    {
        ITransaction income = new Income("Test", 100m, new(2024, 1, 1));
        Schedule.Builder builder = new(income);

        IEnumerable<ITransaction> scheduler = builder.Monthly().Build();
        List<ITransaction> transactions = scheduler.Take(3).ToList();

        Assert.AreEqual(new DateTime(2024, 1, 1), transactions[0].Date);
        Assert.AreEqual(new DateTime(2024, 2, 1), transactions[1].Date);
        Assert.AreEqual(new DateTime(2024, 3, 1), transactions[2].Date);
    }

    [TestMethod]
    public void Yearly_WithValidTransaction_SetsYearlyInterval()
    {
        ITransaction income = new Income("Test", 100m, new(2024, 1, 1));
        Schedule.Builder builder = new(income);

        IEnumerable<ITransaction> scheduler = builder.Yearly().Build();
        List<ITransaction> transactions = scheduler.Take(3).ToList();

        Assert.AreEqual(new DateTime(2024, 1, 1), transactions[0].Date);
        Assert.AreEqual(new DateTime(2025, 1, 1), transactions[1].Date);
        Assert.AreEqual(new DateTime(2026, 1, 1), transactions[2].Date);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Daily_AfterIntervalSet_ThrowsException()
    {
        ITransaction income = new Income("Test", 100m, new(2024, 1, 1));
        Schedule.Builder builder = new(income);
        builder.Monthly().Daily();
    }
}
