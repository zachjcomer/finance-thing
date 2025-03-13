using Finance.Library;

namespace Finance.Transactions.Measures;

public static class MeasureHelper
{
    public static IEnumerable<IGrouping<DateTime, ITransaction>> Bin(TimeInterval interval, params IEnumerable<ITransaction>[] transactions)
    {
        return transactions.SelectMany(x => x).GroupBy(x => x.Date.Truncate(interval));
    }
    
}