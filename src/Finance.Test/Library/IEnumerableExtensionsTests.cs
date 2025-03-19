using Finance.Library;

namespace Finance.Test.Library;

[TestClass]
public class IEnumerableExtensionsTests
{
    [TestMethod]
    public void Accumulate_ReturnsCorrectAccumulatedValues()
    {
        int[] source = [1, 2, 3, 4, 5];
        IEnumerable<int> result = source.Accumulate(x => x, (x, y) => x + y, 0);

        CollectionAssert.AreEqual(new int[] {1, 3, 6, 10, 15}, result.ToArray());
    }

    [TestMethod]
    public void Accumulate_WithNoRightOperand_ReturnsWitLeftOperand()
    {
        int[] source = [1, 2, 3, 4, 5];
        IEnumerable<int> result = source.Accumulate<int, int, int>(x => x, (x, _) => x);

        CollectionAssert.AreEqual(new int[] {1, 2, 3, 4, 5}, result.ToArray());
    }

    [TestMethod]
    public void Accumulate_WithEmptySource_ReturnsEmptySequence()
    {
        int[] source = [];
        IEnumerable<int> result = source.Accumulate(x => x, (x, y) => x + y, 0);

        CollectionAssert.AreEqual(new int[] {}, result.ToArray());
    }

    [TestMethod]
    public void SlidingWindow_ReturnsCorrectWindow()
    {
        int[] source = [1, 2, 3, 4, 5];
        IEnumerable<List<int>> result = source.SlidingWindow(x => x.ToList(), 3);

        List<List<int>> expected = [
            [1],
            [1, 2],
            [1, 2, 3],
            [2, 3, 4],
            [3, 4, 5]
        ];

        Assert.AreEqual(expected.Count, result.Count());
        for (int i = 0; i < expected.Count; i++)
        {
            CollectionAssert.AreEqual(expected[i], result.ElementAt(i));
        }
    }

    [TestMethod]
    public void SlidingWindow_ReturnsCorrectOperand()
    {
        int[] source = [1, 2, 3, 4, 5];
        IEnumerable<int> result = source.SlidingWindow(x => x.Max(), 3);

        Assert.AreEqual(1, result.ElementAt(0));
        Assert.AreEqual(2, result.ElementAt(1));
        Assert.AreEqual(3, result.ElementAt(2));
        Assert.AreEqual(4, result.ElementAt(3));
        Assert.AreEqual(5, result.ElementAt(4));
    }
}