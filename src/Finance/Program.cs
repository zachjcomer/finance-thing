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
        ITransaction bhSalary1 = new Income("BH Salary", 1000, new(2023, 3, 5));
        ITransaction bhSalary2 = new Income("BH Salary", 1000, new(2023, 3, 20));

        var bhSalarySchedule1 = new Schedule.Builder(bhSalary1).Monthly().WithEndDate(new(2025, 3, 21)).Build();
        var bhSalarySchedule2 = new Schedule.Builder(bhSalary2).Monthly().WithEndDate(new(2025, 3, 21)).Build();

        var salaryTotal = MeasureHelper.Bin(TimeInterval.Month, bhSalarySchedule1, bhSalarySchedule2).Accumulate(x => x.Sum(y => y.Amount), (x, y) => x + y, 0m).ToList();

        Console.WriteLine(string.Join(", ", salaryTotal));

        // var dollarRollingAverage = dollarTotal.SlidingWindow(x => x.Average(y => y), 2).ToList();
    } 
}