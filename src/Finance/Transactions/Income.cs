namespace Finance.Transactions;

public record Income(string Name, decimal Value, DateTime Date) : ITransaction
{
    public decimal Amount => Value;
} 