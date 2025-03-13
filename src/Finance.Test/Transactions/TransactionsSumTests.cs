using Finance.Transactions;

namespace Finance.Test.Transactions;

[TestClass]
public class TransactionsSumTests
{
    [TestMethod]
    public void SumOfEmptyTransactions_ReturnsZero()
    {
        var transactions = Array.Empty<ITransaction>();
        var sum = transactions.Sum(t => t.Amount);
        Assert.AreEqual(0m, sum);
    }

    [TestMethod]
    public void SumOfSingleIncome_ReturnsIncomeAmount()
    {
        var income = new Income("Salary", 1000m, DateTime.Today);
        var transactions = new[] { income };
        var sum = transactions.Sum(t => t.Amount);
        Assert.AreEqual(1000m, sum);
    }

    [TestMethod]
    public void SumOfSingleExpense_ReturnsNegativeExpenseValue() 
    {
        var expense = new Expense("Groceries", 50m, DateTime.Today);
        var transactions = new[] { expense };
        var sum = transactions.Sum(t => t.Amount);
        Assert.AreEqual(-50m, sum);
    }

    [TestMethod]
    public void SumOfIncomeAndExpense_ReturnsCorrectBalance()
    {
        var income = new Income("Salary", 1000m, DateTime.Today);
        var expense1 = new Expense("Rent", 500m, DateTime.Today);
        var expense2 = new Expense("Food", 200m, DateTime.Today);
        
        var transactions = new ITransaction[] { income, expense1, expense2 };
        var sum = transactions.Sum(t => t.Amount);
        
        Assert.AreEqual(300m, sum); // 1000 - 500 - 200 = 300
    }
}
