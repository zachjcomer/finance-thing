using System.Collections;
using Finance.Library;

namespace Finance.Transactions.Schedules;

public class Schedule : ISchedule
{
    private static readonly string DEFAULT_NAME = "Schedule";
    private static readonly TimeInterval DEFAULT_INTERVAL = TimeInterval.Month;
    private const int DEFAULT_YEARS = 10;

    private string _name { get; init; }
    public string Name => _name;

    private ITransaction _baseTransaction { get; init; }
    private TimeInterval _interval { get; init; }
    private DateTime _endDate { get; init; }

    private ITransaction CurrentTransaction
    {
        get => _currentTransaction ??= _baseTransaction;
        set => _currentTransaction = value;
    }

    private ITransaction? _currentTransaction;

    private Schedule(string name, ITransaction transaction, TimeInterval interval, DateTime endDate)
    {
        _name = name;
        _baseTransaction = transaction;
        _endDate = endDate;
        _interval = interval;
    }

    private static ITransaction GetNext(ITransaction transaction, TimeInterval interval)
    {
        DateTime nextDate = transaction.Date.Add(interval);

        return transaction switch
        {
            { Amount: >= 0 } => new Income(transaction.Name, transaction.Amount, nextDate),
            { Amount: < 0 } => new Expense(transaction.Name, transaction.Amount, nextDate),
            _ => throw new ArgumentException("Unsupported transaction type", nameof(transaction))
        };
    }

    #region IEnumerable
    public IEnumerator<ITransaction> GetEnumerator()
    {
        while (CurrentTransaction != null && CurrentTransaction.Date <= _endDate)
        {
            yield return CurrentTransaction;
            CurrentTransaction = GetNext(CurrentTransaction, _interval);
        }

        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion

    #region Builder
    public class Builder
    {
        private string _name { get; set; } = DEFAULT_NAME;
        private ITransaction _baseTransaction { get; init; }
        private TimeInterval? _interval { get; set; }
        private DateTime? _endDate { get; set; }

        public Builder(ITransaction transaction)
        {
            _baseTransaction = transaction;
        }

        public Builder Name(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            _name = name;
            return this;
        }

        public Builder WithInterval(TimeInterval interval)
        {
            if (_interval is not null)
                throw new InvalidOperationException($"Interval already set: {_interval}");

            if (!Enum.IsDefined(interval))
                throw new ArgumentException($"Invalid interval: {interval}", nameof(interval));

            _interval = interval;
            return this;
        }

        public Builder WithEndDate(DateTime endDate)
        {
            if (_endDate is not null)
                throw new InvalidOperationException($"End date already set: {_endDate}");

            if (endDate <= _baseTransaction.Date)
                throw new InvalidOperationException($"End date must be after start date: {_baseTransaction.Date}");

            _endDate = endDate;
            return this;
        }

        public Schedule Build()
        {
            var defaultEndDate = _baseTransaction.Date.AddYears(DEFAULT_YEARS);
            return new Schedule(_name, _baseTransaction, _interval ?? DEFAULT_INTERVAL, _endDate ?? defaultEndDate);
        }
    }
    #endregion
}