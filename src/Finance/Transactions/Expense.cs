namespace Finance.Transactions;

public record Expense : ITransaction
{
    private decimal _amount;
    public decimal Amount {
        get => _amount;
        init => _amount = value < 0 ? value : -value;
    }

    public string Name { get; init; }
    public DateTime Date { get; init; }

    public Expense(string name, decimal amount, DateTime date)
    {
        Name = name;
        Amount = amount;
        Date = date;
    }
} 