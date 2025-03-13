namespace Finance.Transactions;

public interface ITransaction
{
    string Name { get; }
    decimal Amount { get; }
    DateTime Date { get; }
} 