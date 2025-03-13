using System.Diagnostics;
using Finance.Library;
using Finance.Transactions;
using Finance.Transactions.Measures;
using Finance.Transactions.Schedules;

namespace Finance;

class Program
{
    static void Main(string[] args)
    {
        ITransaction dailyDollar = new Income("Dollar", 1, new(2025, 1, 1));
        var dailyDollarSchedule = new Schedule.Builder(dailyDollar).WithInterval(TimeInterval.Day).WithEndDate(new(2025, 1, 31)).Build();

        ITransaction salary = new Income("Salary", 5000, new(2025, 1, 1));
        var salarySchedule = new Schedule.Builder(salary).WithInterval(TimeInterval.Month).WithEndDate(new(2025, 12, 31)).Build();

        ITransaction groceries = new Expense("Groceries", 100, new(2025, 1, 1));
        var groceriesSchedule = new Schedule.Builder(groceries).WithInterval(TimeInterval.Month).WithEndDate(new(2025, 12, 31)).Build();

        var dollarTotal = dailyDollarSchedule.Accumulate(x => x.Amount, (x, y) => x + y, 0m).ToList();
        var dollarRollingAverage = dollarTotal.SlidingWindow(x => x.Average(y => y), 2).ToList();

        var allTransactions = MeasureHelper.Bin(TimeInterval.Month, salarySchedule, groceriesSchedule);

        // ITransaction[] transactions = {
        //     new Expense("Groceries", 100, new(2023, 1, 1)),
        //     new Income("Salary", 5000, new(2023, 1, 1)),
        // };

        // ISchedule[] schedulers = {
        //     new Schedule.Builder(transactions[0]).WithInterval(TimeInterval.Month).WithEndDate(new(2024, 12, 31)).Build(),
        //     new Schedule.Builder(transactions[1]).WithInterval(TimeInterval.Month).WithEndDate(new(2024, 12, 31)).Build(),
        // };

        // var allTransactions = schedulers.SelectMany(x => x.ToList()).GroupBy(x => x.Date.Truncate(TimeInterval.Month));
        // var netIncome = allTransactions.Accumulate(x => x.Sum(y => y.Amount), (x, y) => x + y, 0m);
    }
}