namespace Finance.Transactions;

public record Income(string Name, decimal Amount, DateTime Date) : ITransaction;    