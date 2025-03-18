namespace Finance.Transactions;



public record Expense(string Name, decimal Value, DateTime Date) : ITransaction
{
    public decimal Amount => -Math.Abs(Value);
}