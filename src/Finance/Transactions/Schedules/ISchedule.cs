using System.Collections;
using Finance.Transactions;

namespace Finance.Transactions.Schedules;

public interface ISchedule : IEnumerable<ITransaction> 
{
    string Name { get; }
}
