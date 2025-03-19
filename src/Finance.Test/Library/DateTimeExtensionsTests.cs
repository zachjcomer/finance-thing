using Finance.Library;

namespace Finance.Test.Library;

[TestClass]
public class DateTimeExtensionsTests
{
    [TestMethod]
    public void Truncate_Day_ReturnsSameDate()
    {
        DateTime date = new(2025, 3, 19);
        DateTime result = date.Truncate(TimeInterval.Day);

        Assert.AreEqual(new DateTime(2025, 3, 19), result); 
    }

    [TestMethod]
    public void Truncate_Week_ReturnsFirstDateOfWeek()
    {
        DateTime date = new(2025, 3, 19);
        DateTime result = date.Truncate(TimeInterval.Week);

        Assert.AreEqual(new DateTime(2025, 3, 16), result);
    }

    [TestMethod]
    public void Truncate_Month_ReturnsFirstDateOfMonth()
    {
        DateTime date = new(2025, 3, 19);
        DateTime result = date.Truncate(TimeInterval.Month);

        Assert.AreEqual(new DateTime(2025, 3, 1), result);
    }

    [TestMethod]
    public void Truncate_Year_ReturnsFirstDateOfYear()
    {
        DateTime date = new(2025, 3, 19);
        DateTime result = date.Truncate(TimeInterval.Year);

        Assert.AreEqual(new DateTime(2025, 1, 1), result);
    }

    [TestMethod]
    public void Truncate_EarlyWeek_ReturnsWeekFromPreviousMonth()
    {
        DateTime date = new(2025, 3, 1);
        DateTime result = date.Truncate(TimeInterval.Week);

        Assert.AreEqual(new DateTime(2025, 2, 23), result);
    }

    [TestMethod]
    public void AddMonthsWithClamp_WithZeroMonths_ReturnsSameDate()
    {
        DateTime date = new(2024, 1, 31);
        DateTime result = date.AddMonthsWithClamp(0);

        Assert.AreEqual(date, result);
    }

    [TestMethod]
    public void AddMonthsWithClamp_StartingOnFirst_HandlesAllMonths()
    {
        DateTime date = new(2024, 1, 1);
        DateTime result = date.AddMonthsWithClamp(1);

        Assert.AreEqual(new DateTime(2024, 2, 1), result);
    }

    [TestMethod]
    public void AddMonthsWithClamp_StartingOn31st_ClampsToLastDay()
    {
        DateTime date = new(2024, 1, 31);
        DateTime result = date.AddMonthsWithClamp(1);
        
        Assert.AreEqual(new DateTime(2024, 2, 29), result); // 2024 is a leap year
    }

    [TestMethod]
    public void AddMonthsWithClamp_StartingOn28th_HandlesNonLeapYear()
    {
        DateTime date = new(2023, 1, 28);
        DateTime result = date.AddMonthsWithClamp(1);

        Assert.AreEqual(new DateTime(2023, 2, 28), result);
    }

    [TestMethod]
    public void AddMonthsWithClamp_StartingOn31st_HandlesMultipleMonths()
    {
        DateTime date = new(2024, 1, 31);
        DateTime result = date.AddMonthsWithClamp(3);

        Assert.AreEqual(new DateTime(2024, 4, 30), result);
    }

    [TestMethod]
    public void AddMonthsWithClamp_WithNegativeMonths_ClampsToLastDay()
    {
        DateTime date = new(2024, 3, 31);
        DateTime result = date.AddMonthsWithClamp(-1);
        
        Assert.AreEqual(new DateTime(2024, 2, 29), result);
    }

    [TestMethod]
    public void AddMonthsWithClamp_WithMultipleNegativeMonths_ClampsToLastDay()
    {
        DateTime date = new(2024, 3, 31);
        DateTime result = date.AddMonthsWithClamp(-3);
        
        Assert.AreEqual(new DateTime(2023, 12, 31), result);
    }

    [TestMethod]
    public void AddMonthsWithClamp_CrossingYearBoundary_HandlesLeapYear()
    {
        DateTime date = new(2023, 12, 31);
        DateTime result = date.AddMonthsWithClamp(2);
        
        Assert.AreEqual(new DateTime(2024, 2, 29), result);
    }
}
