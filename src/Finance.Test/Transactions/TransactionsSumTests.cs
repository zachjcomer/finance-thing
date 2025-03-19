using Finance.Transactions;

namespace Finance.Test.Transactions;

[TestClass]
public class TransactionsSumTests
{
    [TestMethod]
    public void SumOfEmptyTransactions_ReturnsZero()
    {
        ITransaction[] transactions = Array.Empty<ITransaction>();
        decimal sum = transactions.Sum(t => t.Amount);

        Assert.AreEqual(0m, sum);
    }

    [TestMethod]
    public void SumOfSingleIncome_ReturnsIncomeAmount()
    {
        ITransaction income = new Income("Salary", 1000m, DateTime.Today);
        ITransaction[] transactions = new[] { income };
        decimal sum = transactions.Sum(t => t.Amount);

        Assert.AreEqual(1000m, sum);
    }

    [TestMethod]
    public void SumOfSingleExpense_ReturnsNegativeExpenseValue() 
    {
        ITransaction expense = new Expense("Groceries", 50m, DateTime.Today);
        ITransaction[] transactions = new[] { expense };
        decimal sum = transactions.Sum(t => t.Amount);

        Assert.AreEqual(-50m, sum);
    }

    [TestMethod]
    public void SumOfIncomeAndExpense_ReturnsCorrectBalance()
    {
        ITransaction income = new Income("Salary", 1000m, DateTime.Today);
        ITransaction expense1 = new Expense("Rent", 500m, DateTime.Today);
        ITransaction expense2 = new Expense("Food", 200m, DateTime.Today);
        
        ITransaction[] transactions = new[] { income, expense1, expense2 };
        decimal sum = transactions.Sum(t => t.Amount);

        Assert.AreEqual(300m, sum);
    }
}
